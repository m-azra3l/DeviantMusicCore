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
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DeviantMusicCore.Views.Shared.Components.SoftwareList
{
    public class SoftwareListViewComponent : ViewComponent
    {
        private readonly DeviantContext db;

        public SoftwareListViewComponent( DeviantContext _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke(int count)
        {
            count = 3;
            var software = db.Softwares.Include(bg => bg.Developer)
                                    .Include(bg => bg.SoftwareType)
                                   .Include(bg => bg.ExtrasLicense)
                                    .OrderByDescending(bg => bg.Id)
                                             .Take(count).ToList();
            return View(software);
        }
    }
}