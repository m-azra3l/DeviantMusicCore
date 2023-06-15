using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DeviantMusicCore.Models;
using Microsoft.AspNetCore.Hosting;
using DeviantMusicCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviantMusicCore.Views.Shared.Components.SocialLinks
{
    public class SocialLinksViewComponent :  ViewComponent
    {
        private readonly DeviantContext db;

        public SocialLinksViewComponent(DeviantContext _db)
        {
            db = _db;
        }
        
        public IViewComponentResult Invoke()
        {
            return View(db.Socials.ToList());
        }
    }
}