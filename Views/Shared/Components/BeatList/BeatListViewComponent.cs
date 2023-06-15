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

namespace DeviantMusicCore.Views.Shared.Components.BeatList
{
    public class BeatListViewComponent :  ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly DeviantContext db;

        public BeatListViewComponent(UserManager<ApplicationUser> _userManager, DeviantContext _db)
        {
            userManager = _userManager;
            db = _db;
        }
        
        public IViewComponentResult Invoke(int count)
        {
            count = 3;
            var beat = db.Beats.Include(bg => bg.Producer)
                                    .Include(bg => bg.Genre)
                                   .Include(bg => bg.ExtrasLicense)
                                    .OrderByDescending(bg => bg.Id)
                                             .Take(count).ToList();
            return View(beat);
        }
    }
}