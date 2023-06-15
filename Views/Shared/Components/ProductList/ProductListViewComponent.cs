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

namespace DeviantMusicCore.Views.Shared.Components.ProductList
{
    public class ProductListViewComponent :  ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly DeviantContext db;

        public ProductListViewComponent(UserManager<ApplicationUser> _userManager, DeviantContext _db)
        {
            userManager = _userManager;
            db = _db;
        }
        
        /*public async Task<IViewComponentResult> InvokeAsync(int pageNumber, int pageSize)
        {
            pageNumber = 1;
            pageSize = 3;
            var product = db.Products.Include(p => p.Artiste)
                                .OrderByDescending(p => p.Id);
            return View(await PaginatedList<Product>.CreateAsync(product.AsNoTracking(), pageNumber, pageSize));
        }*/

        public IViewComponentResult Invoke(int count)
        {
            count = 3;
            var product = db.Products.Include(p => p.Artiste)
                            .Include(pd => pd.ProductLicense)
                                .Include(pd => pd.ProductType)
                                .OrderByDescending(p => p.Id)
                                .Take(count).ToList();
                                             
            return View(product);
        }
    }
}