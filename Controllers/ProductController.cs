using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeviantMusicCore.Models;
using DeviantMusicCore.ViewModels;
using DeviantMusicCore.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DeviantMusicCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly DeviantContext db;
        private readonly IConfiguration config;
        private readonly MailSettings mailSettings;

        public ProductController(DeviantContext _db, IConfiguration _config, IOptions<MailSettings> _mailSettings)
        {
            db = _db;
            config = _config;
            mailSettings = _mailSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BeatLibrary(string currentFilter, string searchString, 
                                int? pageNumber, string genre, string license)
        {
            ViewData["GenreId"] = new SelectList(db.Genres, "Name", "Name");
            ViewData["ExtrasLicenseId"] = new SelectList(db.ExtrasLicenses, "License", "License");

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var product = from pd in db.Beats.Include(p => p.Producer)
                                                .Include(p => p.Genre)
                                                .Include(p => p.ExtrasLicense)
                                                .OrderByDescending(p => p.Id)
                            select pd;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(pd => pd.Title.Contains(searchString) || pd.ExternalProducer.Contains(searchString) || pd.Producer.StageName.Contains(searchString));
            }

            if (!String.IsNullOrWhiteSpace(genre))
            {
                product = product.Where(pd => pd.Genre.Name.Contains(genre));
            }

            if (!String.IsNullOrWhiteSpace(license))
            {
                product = product.Where(pd => pd.ExtrasLicense.License.Contains(license));
            }

            int pageSize = 9;

            return View(await PaginatedList<Beat>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> SoftwareLibrary(string currentFilter, string searchString, 
                                int? pageNumber, string license, string tag)
        {
            ViewData["SoftwareType"] = new SelectList(db.SoftwareTypes, "Type", "Type");
            ViewData["ExtrasLicense"] = new SelectList(db.ExtrasLicenses, "License", "License");

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var product = from pd in db.Softwares.Include(p => p.Developer)
                                                .Include(p => p.SoftwareType)
                                                .Include(p => p.ExtrasLicense)
                                                .OrderByDescending(p => p.Id)
                            select pd;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(pd => pd.Name.Contains(searchString) || pd.ExternalDeveloper.Contains(searchString) || pd.Developer.Name.Contains(searchString));
            }

            if (!String.IsNullOrWhiteSpace(tag))
            {
                product = product.Where(pd => pd.SoftwareType.Type.Contains(tag));
            }

            if (!String.IsNullOrWhiteSpace(license))
            {
                product = product.Where(pd => pd.ExtrasLicense.License.Contains(license));
            }

            int pageSize = 10;

            return View(await PaginatedList<Software>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult SoftwareDetail(string url, string alert)
        {
            ViewData["Alert"] = alert;
            var software = db.Softwares.Single(s => s.Url == url);
            return View(software);
        }
        
        [HttpPost]
        public IActionResult SoftwareDetail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BeatDetail(string url, string alert)
        {
            ViewData["Alert"] = alert;
            var beat = db.Beats.Single(b => b.Url == url);
            return View(beat);
        }
        
        [HttpPost]
        public IActionResult BeatDetail()
        {
            return View();
        }

        public async Task<IActionResult> TopFreeBeats(int? pageNumber)
        {
            int pageSize = 9;

            var beat = db.Beats.Include(bg => bg.Producer)
                                    .Include(bg => bg.Genre)
                                   .Include(bg => bg.ExtrasLicense)
                    .Where(bg => bg.ExtrasLicense.License != "Paid")
                        .OrderByDescending(bg => bg.DownloadCount);

            return View(await PaginatedList<Beat>.CreateAsync(beat.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> TopFreeSoftwares(int? pageNumber)
        {
            int pageSize = 10;

            var software = db.Softwares.Include(bg => bg.Developer)
                                    .Include(bg => bg.SoftwareType)
                                   .Include(bg => bg.ExtrasLicense)
                    .Where(bg => bg.ExtrasLicense.License != "Paid")
                        .OrderByDescending(bg => bg.DownloadCount);

            return View(await PaginatedList<Software>.CreateAsync(software.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public FileResult FreeBeatDownload(string url, string email)
        {
            var beat = db.Beats.Single(u => u.Url == url);
            beat.DownloadCount = beat.DownloadCount + 1;
            db.Beats.Update(beat);
            byte[] bytes;
            string fileName, contentType;
            string connectString = config.GetConnectionString("Default");
            var emailExists = db.Subscribers.FirstOrDefault(sub => sub.Email == email);
            if (emailExists != null)
            {
                using(var con = new SqliteConnection(connectString))
                {
                    con.Open();
                    var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Beats where Url=@url", con);
                    cmd.Parameters.AddWithValue("@url", url);
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        bytes = (byte[])dr["Data"];
                        contentType = dr["ContentType"].ToString();
                        fileName = dr["AudioPath"].ToString();
                    }
                    con.Close();
                }
                return File(bytes, contentType, "inline;filename=" + fileName);
            }
            var subscriber = new Subscriber
            { 
                Email = email   
            };
            db.Subscribers.Add(subscriber);
            db.SaveChanges();
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\MailTemplates\\Welcome.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[email]", email);
            var myemail = new MimeMessage();
            myemail.Sender = MailboxAddress.Parse(mailSettings.Mail);
            myemail.To.Add(MailboxAddress.Parse(email));
            myemail.Subject = $"Welcome {email}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            myemail.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            smtp.Send(myemail);
            smtp.Disconnect(true); 
            using(var con = new SqliteConnection(connectString))
            {
                con.Open();
                var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Beats where Url=@url", con);
                cmd.Parameters.AddWithValue("@url", url);
                using (SqliteDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    bytes = (byte[])dr["Data"];
                    contentType = dr["ContentType"].ToString();
                    fileName = dr["AudioPath"].ToString();
                }
                con.Close();
            }
            return File(bytes, contentType, "inline;filename=" + fileName);
        }

        public FileResult FreeSoftwareDownload(string url, string email)
        {
            var p = db.Softwares.Single(u => u.Url == url);
            p.DownloadCount = p.DownloadCount + 1;
            db.Softwares.Update(p);
            byte[] bytes;
            string fileName, contentType;
            string connectString = config.GetConnectionString("Default");
            var emailExists = db.Subscribers.FirstOrDefault(sub => sub.Email == email);
            if (emailExists != null)
            {
                using(var con = new SqliteConnection(connectString))
                {
                    con.Open();
                    var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Softwares where Url=@url", con);
                    cmd.Parameters.AddWithValue("@url", url);
                    using (SqliteDataReader dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        bytes = (byte[])dr["Data"];
                        contentType = dr["ContentType"].ToString();
                        fileName = dr["AudioPath"].ToString();
                    }
                    con.Close();
                }
                return File(bytes, contentType, "inline;filename=" + fileName);
            }
            var subscriber = new Subscriber
            { 
                Email = email   
            };
            db.Subscribers.Add(subscriber);
            db.SaveChanges();
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\MailTemplates\\Welcome.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[email]", email);
            var myemail = new MimeMessage();
            myemail.Sender = MailboxAddress.Parse(mailSettings.Mail);
            myemail.To.Add(MailboxAddress.Parse(email));
            myemail.Subject = $"Welcome {email}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            myemail.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            smtp.Send(myemail);
            smtp.Disconnect(true); 
            using(var con = new SqliteConnection(connectString))
            {
                con.Open();
                var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Softwares where Url=@url", con);
                cmd.Parameters.AddWithValue("@url", url);
                using (SqliteDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    bytes = (byte[])dr["Data"];
                    contentType = dr["ContentType"].ToString();
                    fileName = dr["AudioPath"].ToString();
                }
                con.Close();
            }
            return File(bytes, contentType, "inline;filename=" + fileName);
        }

        public ActionResult DownloadSoftware(string url, string email2)
        {
            var software = db.Softwares.Single(ab => ab.Url == url);
            if(software == null)
            {
                return NotFound();
            }
            var emailExists = db.Subscribers.FirstOrDefault(sub => sub.Email == email2);
            if (emailExists != null)
            {
                software.DownloadCount = software.DownloadCount +1;
                db.Softwares.Update(software);
                return Redirect(software.DownloadURL);
            }
            var subscriber = new Subscriber
            { 
                Email = email2   
            };
            db.Subscribers.Add(subscriber);
            software.DownloadCount = software.DownloadCount +1;
            db.Softwares.Update(software);
            db.SaveChanges();
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\MailTemplates\\Welcome.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[email]", email2);
            var myemail = new MimeMessage();
            myemail.Sender = MailboxAddress.Parse(mailSettings.Mail);
            myemail.To.Add(MailboxAddress.Parse(email2));
            myemail.Subject = $"Welcome {email2}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            myemail.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
            smtp.Send(myemail);
            smtp.Disconnect(true); 
            return Redirect(software.DownloadURL);
        }
    }
}