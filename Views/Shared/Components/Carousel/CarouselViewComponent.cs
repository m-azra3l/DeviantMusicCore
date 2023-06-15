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


namespace DeviantMusicCore.Views.Shared.Components.Carousel
{
    public class CarouselViewComponent :  ViewComponent
    {
        private readonly DeviantContext db;

        public CarouselViewComponent(DeviantContext _db)
        {
            db = _db;
        }
        
        public IViewComponentResult Invoke()
        {
            var homecarousel = db.HomeCarousels.OrderByDescending(p => p.Id)
                                                .ToList();
            return View(homecarousel);
        }
    }
}