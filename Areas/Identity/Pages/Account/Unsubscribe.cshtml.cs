using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using DeviantMusicCore.Models;
using DeviantMusicCore.Data;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DeviantMusicCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class UnsubscribeModel : PageModel
    {
        private readonly DeviantContext db;
        private readonly MailSettings mailSettings;

        public UnsubscribeModel(DeviantContext _db, IOptions<MailSettings> _mailSettings)
        {
            db = _db;
            mailSettings = _mailSettings.Value;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var subscriber = db.Subscribers.FirstOrDefault(sub => sub.Email == Input.Email);
                if (subscriber!= null)
                {
                    db.Subscribers.Remove(subscriber);
                    db.SaveChanges();
                    string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\MailTemplates\\Goodbye.html";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[email]", Input.Email);
                    var email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(mailSettings.Mail);
                    email.To.Add(MailboxAddress.Parse(Input.Email));
                    email.Subject = $"Goodbye {Input.Email}";
                    var builder = new BodyBuilder();
                    builder.HtmlBody = MailText;
                    email.Body = builder.ToMessageBody();
                    using var smtp = new SmtpClient();
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    StatusMessage = "You have successfully unsubscribed.";
                    return RedirectToPage();
                }
                else
                {
                    StatusMessage = "Error, you have no existing subscription.";
                    return RedirectToPage();
                }
            }
            return Page();
        }
    }
}