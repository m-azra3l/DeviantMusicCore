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

namespace DeviantMusicCore.Views.Shared.Components.BoxAdList
{
    public class BoxAdListViewComponent : ViewComponent
    {
        private readonly DeviantContext db;

        public BoxAdListViewComponent( DeviantContext _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke()
        {
            var adb = db.AdsPBs.OrderByDescending(p => p.Id)
                                                .ToList();
            return View(adb);
        }
    }
}