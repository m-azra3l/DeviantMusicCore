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

namespace DeviantMusicCore.Views.Shared.Components.BlogList
{
    public class BlogListViewComponent : ViewComponent
    {
        private readonly DeviantContext db;

        public BlogListViewComponent( DeviantContext _db)
        {
            db = _db;
        }
        
        public IViewComponentResult Invoke(int count)
        {
            count = 3;
            var blogitem = db.BlogItems.Include(bg => bg.Author)
                                .Include(bg => bg.BlogCategory)
                                 .OrderByDescending(bg => bg.Id)
                                          .Take(count).ToList();
            return View(blogitem);
        }
    }
}