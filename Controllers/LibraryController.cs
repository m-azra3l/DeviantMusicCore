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
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace DeviantMusicCore.Controllers
{
    public class LibraryController : Controller
    {
        private readonly DeviantContext db;
        private readonly IConfiguration config;

        public LibraryController(DeviantContext _db, IConfiguration _config)
        {
            db = _db;
            config = _config;           
        }

        public async Task<IActionResult> Index(string currentFilter, string searchString, 
                                int? pageNumber, string genre, string license, string tag)
        {
            ViewData["Genre"] = new SelectList(db.Genres, "Name", "Name");
            ViewData["ProductType"] = new SelectList(db.ProductTypes, "Tag", "Tag");
            ViewData["ProductLicense"] = new SelectList(db.ProductLicenses, "License", "License");

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var product = from pd in db.Products.Include(p => p.Artiste)
                                                .Include(p => p.Genre)
                                                .Include(p => p.ProductType)
                                                .Include(p => p.ProductLicense)
                                                .OrderByDescending(p => p.Id)
                            select pd;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(pd => pd.Title.Contains(searchString) || pd.ArtisteName.Contains(searchString) || pd.Artiste.StageName.Contains(searchString));
            }

            if (!String.IsNullOrWhiteSpace(genre))
            {
                product = product.Where(pd => pd.Genre.Name.Contains(genre));
            }

            if (!String.IsNullOrWhiteSpace(license))
            {
                product = product.Where(pd => pd.ProductLicense.License.Contains(license));
            }

            if (!String.IsNullOrWhiteSpace(tag))
            {
                product = product.Where(pd => pd.ProductType.Tag.Contains(tag));
            }

            int pageSize = 9;

            return View(await PaginatedList<Product>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Details(string url)
        {
            var product = db.Products.Single(pd => pd.Url == url);
            return View(product);
        }

        public IActionResult AlbumDetails(string url)
        {
            var album = db.Products.Include(ab => ab.MyProducts.OrderByDescending(mp => mp.Id))
                        .Single(ab => ab.Url == url);
            return View(album);
        }

        public IActionResult EPDetails(string url)
        {
            var ep = db.Products.Include(ab => ab.MyProducts.OrderByDescending(mp => mp.Id))
                        .Single(ab => ab.Url == url);
            return View(ep);
        }

        public async Task<IActionResult> TopDownloads(int? pageNumber)
        {
            int pageSize = 9;

            var product = db.Products.Include(p => p.Artiste)
                                                .Include(p => p.Genre)
                                                .Include(p => p.ProductType)
                                                .Include(p => p.ProductLicense)
                                                .Where(p => p.ProductType.Tag != "Album" || p.ProductType.Tag != "EP" && p.DownloadCount > 0)
                                                .OrderByDescending(p => p.DownloadCount);

            return View(await PaginatedList<Product>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public FileResult Download(string url)
        {
            var p = db.Products.Single(u => u.Url == url);
            p.DownloadCount = p.DownloadCount + 1;
            db.Products.Update(p);
            db.SaveChanges();
            byte[] bytes;
            string fileName, contentType;
            string connectString = config.GetConnectionString("Default");
            using(var con = new SqliteConnection(connectString))
            {
                con.Open();
                var cmd = new SqliteCommand("select AudioPath, ContentType, Data from Products where Url=@url", con);
                cmd.Parameters.AddWithValue("@url", url);
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
    }
}