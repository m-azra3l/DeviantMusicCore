using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using DeviantMusicCore.Models;
using DeviantMusicCore.Data;
using DeviantMusicCore.Logic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DeviantMusicCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeviantContext db;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;
        private readonly MailSettings mailSettings;

        public HomeController(DeviantContext _db, ILogger<HomeController> logger, IConfiguration _config, IOptions<MailSettings> _mailSettings)
        {
            db = _db;
            config = _config;
            _logger = logger;
            mailSettings = _mailSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult ContactUs(string alert)
        {
            List<SelectListItem> mailto = new()
            {
                new SelectListItem{Value="", Text="Select Mail Destination"},
                new SelectListItem{Value="contact@deviantmusic.one", Text="Contact"},
                new SelectListItem{Value="retail@deviantmusic.one", Text="Retail"},
                new SelectListItem{Value="support@deviantmusic.one", Text="Support"}
            }; 
            ViewData["MailTo"] = mailto;
            ViewData["Alert"] = alert;
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ContactMail contactmail, string alert)
        {
            List<SelectListItem> mailto = new()
            {
                new SelectListItem{Value="", Text="Select Mail Destination"},
                new SelectListItem{Value="contact@deviantmusic.one", Text="Contact"},
                new SelectListItem{Value="retail@deviantmusic.one", Text="Retail"},
                new SelectListItem{Value="support@deviantmusic.one", Text="Support"}
            }; 
            if(ModelState.IsValid)
            {
                var myemail = new MimeMessage();
                myemail.Sender = MailboxAddress.Parse(mailSettings.Mail);
                myemail.To.Add(MailboxAddress.Parse(contactmail.emailTo));
                myemail.Subject = contactmail.Subject;
                var builder = new BodyBuilder();
                if (Request.Form.Files.Count > 0)
                {
                    byte[] fileBytes;
                    foreach (var file in Request.Form.Files)
                    {
                        if (file.Length < 1012000)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                        else
                        {
                            ViewData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "The attachment file is too large.");
                            ViewData["MailTo"] = mailto;
                            return View(contactmail);
                        }
                    }
                }
                builder.HtmlBody = "Name:" +contactmail.Name+ "<br/><br/>Email:"+contactmail.Email+"<br/><br/>Phone:"+ contactmail.Tel+"<br/><br/>"+contactmail.Message;
                myemail.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                smtp.Send(myemail);
                smtp.Disconnect(true);
                ViewData["Alert"] = CommonServices.ShowAlert(Alerts.Success, "Mail sent successfully.");
                ModelState.Clear();
                ViewData["MailTo"] = mailto;
                return View();
            }
            ViewData["MailTo"] = mailto;
            ViewData["Alert"] = CommonServices.ShowAlert(Alerts.Danger, "Fill out all required fields.");
            return View(contactmail);
        }

        public IActionResult Help()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Disclaimer()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult TemporaryDownloads()
        {
            string url = Request.Headers["Referer"].ToString();
            string downloadtoken = url.Split("/").Last();
            var data = from d in db.Downloads where d.DownloadToken == downloadtoken select d;
            Download obj = data.SingleOrDefault();
            if(obj == null)
            {
                return RedirectToAction(nameof(DownloadError));
            }
            if(obj.ExpireAfterDownload.Equals(true))
            {
                if(obj.Downloaded.Equals(true))
                {
                    return RedirectToAction(nameof(DownloadError));
                }
            }
            if (obj.ExpireAfterDownload.Equals(true))
            {
                obj.Downloaded = true;
                obj.Hits = obj.Hits + 1;
                db.SaveChanges();
            }
            int? id = obj.BeatId;
            var beat = db.Beats.Find(id);
            beat.DownloadCount = beat.DownloadCount + 1;
            db.Beats.Update(beat);
            db.SaveChanges();
            byte[] bytes;
            string fileName, contentType;
            string connectString = config.GetConnectionString("Default");
            using(var con = new SqliteConnection(connectString))
            {
                con.Open();
                var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Beats where Id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
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

        public IActionResult Downloads()
        {
            string url = Request.Headers["Referer"].ToString();
            string downloadtoken = url.Split("/").Last();
            var data = from d in db.Downloads where d.DownloadToken == downloadtoken select d;
            Download obj = data.SingleOrDefault();
            if(obj == null)
            {
                return RedirectToAction(nameof(DownloadError));
            }
            int? id = obj.BeatId;
            var beat = db.Beats.Find(id);
            beat.DownloadCount = beat.DownloadCount + 1;
            db.Beats.Update(beat);
            db.SaveChanges();
            byte[] bytes;
            string fileName, contentType;
            string connectString = config.GetConnectionString("Default");
            using(var con = new SqliteConnection(connectString))
            {
                con.Open();
                var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Beats where Id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);
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

        public IActionResult DownloadError()
        {
            return View();
        }

        public ActionResult RedirectToAd(int id)
        {
            var adb = db.AdsBs.Find(id);
            adb.Hits = adb.Hits + 1;
            db.AdsBs.Update(adb);
            db.SaveChanges();
            return Redirect(adb.NavigateUrl);
        }

        public ActionResult RedirectAd(int id)
        {
            var adb = db.AdsPBs.Find(id);
            adb.Hits = adb.Hits + 1;
            db.AdsPBs.Update(adb);
            db.SaveChanges();
            return Redirect(adb.NavigateUrl);
        }

        public IActionResult SignIn()
        {
            return new RedirectToPageResult("/Account/Login", new{area="Identity"});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
