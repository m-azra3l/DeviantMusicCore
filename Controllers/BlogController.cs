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


namespace DeviantMusicCore.Controllers
{
    public class BlogController : Controller
    {   
        private readonly DeviantContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public BlogController(DeviantContext _db, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<ApplicationUser> _signInManager)
        {
            db = _db;
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
        }

        public async Task<IActionResult> Index(string currentFilter,
                                string searchString, int? pageNumber, string category)
        {
            ViewData["Category"] = new SelectList(db.BlogCategories,"Category","Category");

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var blogitem = from blogitems in db.BlogItems.Include(bg => bg.Author)
                                                .Include(bg => bg.BlogCategory)
                                        .OrderByDescending(bg => bg.Id)
                                        select blogitems;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                blogitem = blogitem.Where(blogitems => blogitems.Title.Contains(searchString) || blogitems.Author.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(category))
            {
                blogitem = blogitem.Where(pd => pd.BlogCategory.Category.Contains(category));
            }

            int pageSize = 10;

            return View(await PaginatedList<BlogItem>.CreateAsync(blogitem.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> TopArticles(int? pageNumber)
        {
            int pageSize = 10;

            var blogitem = db.BlogItems.Include(bg => bg.Author)
                                    .Include(bg => bg.BlogCategory)
                                        .OrderByDescending(bg => bg.Views);
            return View(await PaginatedList<BlogItem>.CreateAsync(blogitem.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Article(string id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var blogitem = db.BlogItems.Include(bg => bg.Author)
                                            .Include(bg => bg.BlogCategory)
                                .Single(bg => bg.BlogItemUrl == id);
            if(blogitem==null)
            {
                return NotFound();
            }
            blogitem.Views = blogitem.Views + 1;
            db.BlogItems.Update(blogitem);
            db.SaveChanges();
            return View(blogitem);
        }
    }
}