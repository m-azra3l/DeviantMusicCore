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
using DeviantMusicCore.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DeviantMusicCore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {   
        private readonly DeviantContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger<AdminController> logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly MailSettings mailSettings;

        public AdminController(DeviantContext _db, IWebHostEnvironment _hostingEnvironment, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ILogger<AdminController> _logger, SignInManager<ApplicationUser> _signInManager, IOptions<MailSettings> _mailSettings)
        {
            db = _db;
            userManager = _userManager;
            roleManager = _roleManager;
            logger = _logger;
            signInManager = _signInManager;
            hostingEnvironment = _hostingEnvironment;
            mailSettings = _mailSettings.Value;
        }

        public IActionResult Index()
        {
            return new RedirectToPageResult("/Account/Manage/Index", new{area="Identity"});
        }
        
        #region AdsB

        [Authorize(Roles = Roles.Master)]
        public IActionResult BannerAdsList()
        {
            return View(db.AdsBs.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateAdsB()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateAdsB(AdsB adb)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                foreach(var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if(ms.Length < 512000)
                    {
                        imageData = ms.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File","The file is too large.");
                        return View(adb);
                    }
                }
                adb.ImageUrl = imageData;
                adb.Date = DateTime.Now;
                db.AdsBs.Add(adb);
                db.SaveChanges();
                return RedirectToAction(nameof(BannerAdsList));
            }
            return View(adb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditAdsB(int? id)
        {
            var adb = db.AdsBs.Find(id);
            if(adb==null)
            {
                return NotFound();
            }
            string imageBase64Data = Convert.ToBase64String(adb.ImageUrl);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataUrl;
            return View(adb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost, ActionName("EditAdsB")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdsBPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var adb = await db.AdsBs.FirstOrDefaultAsync(ad => ad.Id == id);
            if (await TryUpdateModelAsync<AdsB>(
                adb,
                "",
                ad => ad.Title, ad => ad.AlternateText, ad => ad.NavigateUrl))
                {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                adb.ImageUrl = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(adb);
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(BannerAdsList));
                }
                catch (DbUpdateException)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. ");
                }
            }
            return View(adb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteAdsB(int? id)
        {
            var adb = db.AdsBs.Find(id);
            if (adb == null)
            {
                return NotFound();
            }
            db.AdsBs.Remove(adb);
            db.SaveChanges();
            return RedirectToAction(nameof(BannerAdsList));
        }

        #endregion

        #region AdsPB

        [Authorize(Roles = Roles.Master)]
        public IActionResult SquareAdsList()
        {
             return View(db.AdsPBs.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateAdsPB()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateAdsPB(AdsPB adpb)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                foreach(var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if(ms.Length < 512000)
                    {
                        imageData = ms.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File","The file is too large.");
                        return View(adpb);
                    }
                }
                adpb.ImageUrl = imageData;
                adpb.Date = DateTime.Now;
                db.AdsPBs.Add(adpb);
                db.SaveChanges();
                return RedirectToAction(nameof(SquareAdsList));
            }
            return View(adpb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditAdsPB(int? id)
        {
            var adpb = db.AdsPBs.Find(id);
            if(adpb==null)
            {
                return NotFound();
            }
            string imageBase64Data = Convert.ToBase64String(adpb.ImageUrl);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataUrl;
            return View(adpb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost, ActionName("EditAdsPB")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdsPBPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var adpb = await db.AdsPBs.FirstOrDefaultAsync(ad => ad.Id == id);
            if (await TryUpdateModelAsync<AdsPB>(
                adpb,
                "",
                ad => ad.Title, ad => ad.AlternateText, ad => ad.NavigateUrl))
                {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                adpb.ImageUrl = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(adpb);
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(SquareAdsList));
                }
                catch (DbUpdateException)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(adpb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteAdsPB(int? id)
        {
            var adpb = db.AdsPBs.Find(id);
            if (adpb == null)
            {
                return NotFound();
            }
            db.AdsPBs.Remove(adpb);
            db.SaveChanges();
            return RedirectToAction(nameof(SquareAdsList));
        }

        #endregion

        #region AppUsers

        [Authorize(Roles = Roles.Master)]
        public IActionResult UsersList()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }
            IdentityResult result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{id}'.");
            }

            logger.LogInformation("User with ID '{UserId}' was deleted.", id);

            return RedirectToAction(nameof(UsersList));
        }
       
        #endregion

        #region Beat

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public async Task<IActionResult> BeatList()
        {
            var beat = db.Beats.Include(b => b.Genre).Include(b => b.ExtrasLicense);
            return View(await beat.ToListAsync());
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult CreateBeat()
        {
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name");
            ViewData["LicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License");
            ViewData["ProducerId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName");
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBeat(Beat beat)
        {
            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    byte[] imageData = null;
                    foreach(var imagefile in Request.Form.Files)
                    {
                        MemoryStream ms = new MemoryStream();
                        imagefile.CopyTo(ms);
                        if(ms.Length < 512000)
                        {
                            imageData = ms.ToArray();
                            beat.Image = imageData;
                        }
                        else
                        {
                            ModelState.AddModelError("File","The file is too large.");
                            return View(beat);
                        }
                    }
                }
                beat.DownloadCount = 0;
                beat.UploadDate = DateTime.Now;
                db.Add(beat);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(BeatList));
            }
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",beat.GenreId);
            ViewData["LicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",beat.ExtrasLicenseId);
            ViewData["ProducerId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", beat.ProducerId);
            return View(beat);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditBeat(int? id)
        {
            var beat = db.Beats.Find(id);     
            if(beat==null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",beat.GenreId);
            ViewData["LicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",beat.ExtrasLicenseId);
            ViewData["ProducerId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", beat.ProducerId);
            return View(beat);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost, ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBeatPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var beat = await db.Beats.FirstOrDefaultAsync(p => p.Id == id);
            if (await TryUpdateModelAsync<Beat>(
                beat,
                "",
                p => p.Title, p => p.Description, p => p.Url, p => p.ExternalProducer,
                p => p.DownloadCount, p => p.ReleaseDate, p => p.Price,
                p => p.PurchaseURL, p => p.ExtrasLicenseId, p => p.GenreId, p => p.ProducerId))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                beat.Image = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(beat);
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    /*List<ApplicationUser> user = new List<ApplicationUser>();
                    user = (from users in userManager.Users select users).ToList();
                    user.Insert(0, new ApplicationUser { Id = null, Name = "--Select--"});
                    ViewBag.AuthorList = user; */
                    return RedirectToAction(nameof(BeatList));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",beat.GenreId);
            ViewData["LicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",beat.ExtrasLicenseId);
            ViewData["ProducerId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", beat.ProducerId);
            return View(beat);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult AttachBeat(int? id)
        {
            var beat = db.Products.Find(id);     
            if(beat==null)
            {
                return NotFound();
            }
            return View(beat);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost, ActionName("AttachBeat")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachBeatPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var beat = await db.Beats.FirstOrDefaultAsync(b => b.Id == id);
            if (await TryUpdateModelAsync<Beat>(beat))
                {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        var filePath = Path.GetFileName(file.FileName);
                        var contentType = Path.GetExtension(filePath);
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 20120000)
                            {
                                beat.Data = dataStream.ToArray();
                                beat.AudioPath = filePath;
                                beat.ContentType = contentType;
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(beat);
                            }
                        }
                    }
                    await db.SaveChangesAsync();
                    /*List<ApplicationUser> user = new List<ApplicationUser>();
                    user = (from users in userManager.Users select users).ToList();
                    user.Insert(0, new ApplicationUser { Id = null, Name = "--Select--"});
                    ViewBag.AuthorList = user; */
                    return RedirectToAction(nameof(BeatList));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(beat);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteBeat(int? id)
        {
            Beat beat = db.Beats.Find(id);
            if (beat == null)
            {
                return NotFound();
            }
            db.Beats.Remove(beat);
            db.SaveChanges();
            return RedirectToAction(nameof(BeatList));
        }

        #endregion

        #region BlogCategory
         
        [Authorize(Roles = Roles.Master)]
        public IActionResult CategoryList()
        {
            return View(db.BlogCategories.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateCategory(BlogCategory blogcategory)
        {
            if (ModelState.IsValid)
            {
                db.BlogCategories.Add(blogcategory);
                db.SaveChanges();
                return RedirectToAction(nameof(CategoryList));
            }
            return View(blogcategory);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            var category = db.BlogCategories.Find(id);
            if(category==null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditCategory(BlogCategory blogcategory)
        {
            if (ModelState.IsValid)
            {
                db.Update(blogcategory);
                db.SaveChanges();
                return RedirectToAction(nameof(CategoryList));
            }
            return View(blogcategory);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var category = db.BlogCategories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            db.BlogCategories.Remove(category);
            db.SaveChanges();
            return RedirectToAction(nameof(CategoryList));
        }

        #endregion

        #region BlogItem
        
        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public async Task<IActionResult> MasterBlogList()
        {
            var blogitem = db.BlogItems.Include(bg => bg.Author);
            return View(await blogitem.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> UserBlogList(ApplicationUser user)
        {
            user = await userManager.GetUserAsync(User);
            var userId = user.Id;
            var blogitem = db.BlogItems.Where(bg => bg.AuthorId.Contains(userId));
            return View(await blogitem.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateBlogPost()
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            ViewData["CategoryId"] = new SelectList(db.BlogCategories,"Id","Category");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlogPost(BlogItem blogitem, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                foreach(var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if(ms.Length < 512000)
                    {
                        imageData = ms.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File","The file is too large.");
                        return View(blogitem);
                    }
                }
                blogitem.PostImage = imageData;
                blogitem.AuthorId = userManager.GetUserId(this.User);
                blogitem.Views = 0;
                blogitem.PublishDate = DateTime.Now;
                db.Add(blogitem);
                await db.SaveChangesAsync(userManager.GetUserName(this.User));
                return Redirect(returnUrl);
            }
            ViewData["CategoryId"] = new SelectList(db.BlogCategories,"Id","Category", blogitem.CategoryId);
            return View(blogitem);
        }

        [HttpGet]
        public IActionResult EditBlogPost(int? id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var blogitem = db.BlogItems.Find(id);     
            if(blogitem==null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(userManager.Users, "Id", "UserName", blogitem.AuthorId);
            ViewData["CategoryId"] = new SelectList(db.BlogCategories,"Id","Category", blogitem.CategoryId); 
            return View(blogitem);
        }
        
        [HttpPost, ActionName("EditBlogPost")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlogPosts(int? id, string returnUrl)
        {
            var blogitem = await db.BlogItems.FindAsync(id);     
            if(blogitem==null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<BlogItem>(blogitem, "", p => p.Title, p => p.Article,
                                        p => p.BlogItemUrl, p => p.CategoryId, p => p.AuthorId))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                blogitem.PostImage = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(blogitem);
                            }
                        }
                    }
                    var oldblogitem = await db.BlogItems.FindAsync(id);
                    db.Entry(oldblogitem).CurrentValues.SetValues(blogitem);
                    await db.SaveChangesAsync(userManager.GetUserName(this.User));
                    return Redirect(returnUrl);
                }
                catch (DbUpdateException)
                {
                    
                }
            }
            ViewData["CategoryId"] = new SelectList(db.BlogCategories,"Id","Category", blogitem.CategoryId);
            ViewData["AuthorId"] = new SelectList(userManager.Users, "Id", "UserName", blogitem.AuthorId);
            return View(blogitem);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteBlogPost(int? id)
        {
            BlogItem blogitem = db.BlogItems.Find(id);
            if (blogitem == null)
            {
                return NotFound();
            }
            db.BlogItems.Remove(blogitem);
            db.SaveChanges();
            return RedirectToAction(nameof(MasterBlogList));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteUserPost(int? id)
        {
            BlogItem blogitem = db.BlogItems.Find(id);
            if (blogitem == null)
            {
                return NotFound();
            }
            db.BlogItems.Remove(blogitem);
            db.SaveChanges();
            return RedirectToAction(nameof(UserBlogList));
        }

        #endregion

        #region Carousel
        
        [Authorize(Roles = Roles.Master)]
        public IActionResult CarouselList()
        {
             return View(db.HomeCarousels.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult CreateCarousel()
        {
            return View();
        }
        
        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateCarousel(HomeCarousel homecarousel)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                foreach(var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if(ms.Length < 512000)
                    {
                        imageData = ms.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File","The file is too large.");
                        return View(homecarousel);
                    }
                }
                homecarousel.CarouselImage = imageData;
                db.HomeCarousels.Add(homecarousel);
                db.SaveChanges();
                return RedirectToAction(nameof(CarouselList));
            }
            return View(homecarousel);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditCarousel(int? id)
        {
            var homecarousel = db.HomeCarousels.Find(id);
            if(homecarousel==null)
            {
                return NotFound();
            }
            string imageBase64Data = Convert.ToBase64String(homecarousel.CarouselImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            ViewBag.ImageDataUrl = imageDataUrl;
            return View(homecarousel);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditCarousel(HomeCarousel homecarousel)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyToAsync(dataStream);
                        if(dataStream.Length < 512000)
                        {
                            homecarousel.CarouselImage = dataStream.ToArray();
                        }
                        else
                        {
                            ModelState.AddModelError("", "The picture file is too large");
                            return View(homecarousel);
                        }
                    }
    
                } 
                db.Update(homecarousel);
                db.SaveChanges();
                return RedirectToAction(nameof(CarouselList));
            }
            return View(homecarousel);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteCarousel(int? id)
        {
            var adpb = db.HomeCarousels.Find(id);
            if (adpb == null)
            {
                return NotFound();
            }
            db.HomeCarousels.Remove(adpb);
            db.SaveChanges();
            return RedirectToAction(nameof(CarouselList));
        }
        #endregion

        #region Downloads

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadList()
        {
            var download = db.Downloads.Include(d => d.Beat);
            return View(await download.ToListAsync());
        }

        private string GetDownloadToken(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strDownloadToken = "";
            Random objRandom = new Random(System.DateTime.Now.Millisecond);
            while (intCount < length)
            {
                intRandomNumber = objRandom.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) &&
                    (intRandomNumber <= intNine) ||
                    (intRandomNumber >= intA) &&
                    (intRandomNumber <= intZ)))
                {
                    strDownloadToken = strDownloadToken + (char)intRandomNumber;
                    intCount++;
                }
            }
            return strDownloadToken;
        }

        [HttpGet]
        public IActionResult CreateToken()
        {
            string plicense = "Paid";
            ViewData["BeatId"] = new SelectList(db.Beats.Where( p => p.ExtrasLicense.License == plicense ), "Id", "Title");
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateToken(Download download, bool tempodown)
        {
            string plicense = "Paid";
            if(ModelState.IsValid)
            {
                if(ViewBag.tempodown = true)
                {
                    download.ExpireAfterDownload = true;
                    download.DownloadToken = GetDownloadToken(10);
                    download.Url = "/TemporaryDownloads/" + GetDownloadToken(10);
                    download.Hits = 0;
                    download.Downloaded = false;
                }
                else
                {
                    download.ExpireAfterDownload = false;
                    download.DownloadToken = GetDownloadToken(10);
                    download.Url = "/Downloads/" + GetDownloadToken(10);
                    download.Hits = 0;
                    download.Downloaded = false;
                }
                db.Add(download);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(DownloadList));
            }
            ViewData["BeatId"] = new SelectList(db.Beats.Where( p => p.ExtrasLicense.License == plicense ), "Id", "Title", download.BeatId);
            return View(download);
        }
        
        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult ViewToken(int? id)
        {
            string plicense = "Paid";
            var download = db.Downloads.Find(id);
            if(download==null)
            {
                return NotFound();
            }
            ViewData["BeatId"] = new SelectList(db.Beats.Where( p => p.ExtrasLicense.License == plicense ), "Id", "Title", download.BeatId);
            return View(download);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteToken(int? id)
        {
           var download = db.Downloads.Find(id);
            if (download == null)
            {
                return NotFound();
            }
            db.Downloads.Remove(download);
            db.SaveChanges();
            return RedirectToAction(nameof(DownloadList)); 
        }

        #endregion

        #region ExtrasLicense

        [Authorize(Roles = Roles.Master)]
        public IActionResult ExtrasList()
        {
            return View(db.ExtrasLicenses.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateExtras()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateExtras(ExtrasLicense mylicense)
        {
            if (ModelState.IsValid)
            {
                db.ExtrasLicenses.Add(mylicense);
                db.SaveChanges();
                return RedirectToAction(nameof(ExtrasList));
            }
            return View(mylicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditExtras(int? id)
        {
            var mylicense = db.ExtrasLicenses.Find(id);
            if(mylicense==null)
            {
                return NotFound();
            }
            return View(mylicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditExtras(ExtrasLicense mylicense)
        {
            if (ModelState.IsValid)
            {
                db.Update(mylicense);
                db.SaveChanges();
                return RedirectToAction(nameof(ExtrasList));
            }
            return View(mylicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteExtras(int? id)
        {
            var mylicense = db.ExtrasLicenses.Find(id);
            if (mylicense == null)
            {
                return NotFound();
            }
            db.ExtrasLicenses.Remove(mylicense);
            db.SaveChanges();
            return RedirectToAction(nameof(ExtrasList));
        }

        #endregion 

        #region Genre

        [Authorize(Roles = Roles.Master)]
        public IActionResult GenreList()
        {
            return View(db.Genres.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateGenre()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                db.SaveChanges();
                return RedirectToAction(nameof(GenreList));
            }
            return View(genre);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditGenre(int? id)
        {
            var genre = db.Genres.Find(id);
            if(genre==null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Update(genre);
                db.SaveChanges();
                return RedirectToAction(nameof(GenreList));
            }
            return View(genre);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteGenre(int? id)
        {
            var genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }
            db.Genres.Remove(genre);
            db.SaveChanges();
            return RedirectToAction(nameof(GenreList));
        }

        #endregion 

        #region Log

        [Authorize(Roles = Roles.Master)]
        public IActionResult LogList()
        {
            return View(db.AuditLogs.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult ViewLog(int? id)
        {
            var adb = db.AuditLogs.Find(id);
            if(adb==null)
            {
                return NotFound();
            }
            return View(adb);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLog(int? id)
        {
            var adb = db.AuditLogs.Find(id);
            if (adb == null)
            {
                return NotFound();
            }
            db.AuditLogs.Remove(adb);
            db.SaveChanges();
            return RedirectToAction(nameof(LogList));
        }

        #endregion

        #region Product

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public async Task<IActionResult> MasterProductList()
        {
            var product = db.Products.Include(pd => pd.Artiste)
                                    .Include(pd => pd.ProductLicense)
                                    .Include(pd => pd.ProductType);
            return View(await product.ToListAsync());
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpGet]
        public async Task<IActionResult> UserProductList(ApplicationUser user)
        {
            user = await userManager.GetUserAsync(User);
            var userId = user.UserName;
            var product = db.Products.Where(pd => pd.Uploader.Contains(userId))
                                                    .Include(pd => pd.Artiste)
                                                    .Include(pd => pd.ProductLicense)
                                                    .Include(pd => pd.ProductType);
            return View(await product.ToListAsync());
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name");
            ViewData["AlbumId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("Album")),"Id","Title"); 
            ViewData["EPId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("EP")),"Id","Title");
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes,"Id","Tag");
            ViewData["ProductLicenseId"] = new SelectList(db.ProductLicenses,"Id","License");
            ViewData["ArtisteId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName");
            return View();
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    byte[] imageData = null;
                    foreach(var imagefile in Request.Form.Files)
                    {
                        MemoryStream ms = new MemoryStream();
                        imagefile.CopyTo(ms);
                        if(ms.Length < 512000)
                        {
                            imageData = ms.ToArray();
                            product.ProductImage = imageData;
                        }
                        else
                        {
                            ModelState.AddModelError("File","The file is too large.");
                            return View(product);
                        }
                    }
                }
                product.Uploader = userManager.GetUserName(this.User);
                product.DownloadCount = 0;
                product.UploadDate = DateTime.Now;
                db.Add(product);
                await db.SaveChangesAsync(userManager.GetUserName(this.User));
                return Redirect(returnUrl);
            }
            ViewData["AlbumId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("Album")),"Id","Title",product.AlbumorEPId); 
            ViewData["EPId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("EP")),"Id","Title",product.AlbumorEPId);
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",product.GenreId);
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes,"Id","Tag", product.ProductTypeId);
            ViewData["ProductLicenseId"] = new SelectList(db.ProductLicenses,"Id","License",product.ProductLicenseId);
            ViewData["Uploader"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "UserName", "UserName", product.Uploader);
            ViewData["ArtisteId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", product.ArtisteId);
            return View(product);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var product = db.Products.Find(id);     
            if(product==null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("Album")),"Id","Title",product.AlbumorEPId); 
            ViewData["EPId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("EP")),"Id","Title",product.AlbumorEPId);
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",product.GenreId);
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes,"Id","Tag", product.ProductTypeId);
            ViewData["ProductLicenseId"] = new SelectList(db.ProductLicenses,"Id","License",product.ProductLicenseId);
            ViewData["Uploader"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "UserName", "UserName", product.Uploader);
            ViewData["ArtisteId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", product.ArtisteId);
            return View(product);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpPost, ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, string returnUrl)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Product>(product, "", p => p.Title, p => p.Description, p => p.Url,
                            p => p.ProductLicenseId, p => p.GenreId, p => p.ProductTypeId, p => p.ReleaseDate,
                            p => p.ArtisteName, p => p.ArtisteId, p => p.StreamPlatformUrl, p => p.Fanlink,
                            p => p.DownloadCount, p => p.Featuring, p => p.Producer, p => p.Writer, p => p.TrackNumber,
                            p => p.AlbumTrack, p => p.EPTrack, p => p.AlbumorEPId, p => p.Uploader))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                product.ProductImage = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(product);
                            }
                        }
                    }
                    var oldproduct = await db.Products.FindAsync(id);
                    db.Entry(oldproduct).CurrentValues.SetValues(product);
                    await db.SaveChangesAsync(userManager.GetUserName(this.User));
                    return Redirect(returnUrl);
                }
                catch (DbUpdateException)
                {
                    
                }
            }
            ViewData["AlbumId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("Album")),"Id","Title",product.AlbumorEPId); 
            ViewData["EPId"] = new SelectList(db.Products.Where(p => p.ProductType.Tag.Equals("EP")),"Id","Title",product.AlbumorEPId);
            ViewData["GenreId"] = new SelectList(db.Genres,"Id","Name",product.GenreId);
            ViewData["ProductTypeId"] = new SelectList(db.ProductTypes,"Id","Tag", product.ProductTypeId);
            ViewData["ProductLicenseId"] = new SelectList(db.ProductLicenses,"Id","License",product.ProductLicenseId);
            ViewData["Uploader"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "UserName", "UserName", product.Uploader);
            ViewData["ArtisteId"] = new SelectList(userManager.Users.Where(user => user.IsArtiste.Equals(true)), "Id", "StageName", product.ArtisteId);
            return View(product);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpGet]
        public IActionResult AttachAudio(int? id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var product = db.Products.Find(id);     
            if(product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpPost, ActionName("AttachAudio")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachAudioPost(int? id, string returnUrl)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Product>(product))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        var filePath = Path.GetFileName(file.FileName);
                        var contentType = Path.GetExtension(filePath);
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 5120000)
                            {
                                product.Data = dataStream.ToArray();
                                product.AudioPath = filePath;
                                product.ContentType = contentType;
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(product);
                            }
                        }
                    }
                    var oldproduct = await db.Products.FindAsync(id);
                    db.Entry(oldproduct).CurrentValues.SetValues(product);
                    await db.SaveChangesAsync(userManager.GetUserName(this.User));
                    return Redirect(returnUrl);
                }
                catch (DbUpdateException)
                {
                    
                }
            }
            return View(product);
        }
        
        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> MasterDelete(int? id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync(userManager.GetUserName(this.User));
            return RedirectToAction(nameof(MasterProductList));
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin+","+Roles.Admin)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
           Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync(userManager.GetUserName(this.User));
            return RedirectToAction(nameof(UserProductList));
        }

        #endregion

        #region ProductLicense

        [Authorize(Roles = Roles.Master)]
        public IActionResult LicenseList()
        {
            return View(db.ProductLicenses.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateLicense()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateLicense(ProductLicense plicense)
        {
            if (ModelState.IsValid)
            {
                db.ProductLicenses.Add(plicense);
                db.SaveChanges();
                return RedirectToAction(nameof(LicenseList));
            }
            return View(plicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditLicense(int? id)
        {
            var plicense = db.ProductLicenses.Find(id);
            if(plicense==null)
            {
                return NotFound();
            }
            return View(plicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditLicense(ProductLicense plicense)
        {
            if (ModelState.IsValid)
            {
                db.Update(plicense);
                db.SaveChanges();
                return RedirectToAction(nameof(LicenseList));
            }
            return View(plicense);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteLicense(int? id)
        {
            var plicense = db.ProductLicenses.Find(id);
            if (plicense == null)
            {
                return NotFound();
            }
            db.ProductLicenses.Remove(plicense);
            db.SaveChanges();
            return RedirectToAction(nameof(LicenseList));
        }

        #endregion 

        #region ProductType

        [Authorize(Roles = Roles.Master)]
        public IActionResult TagList()
        {
            return View(db.ProductTypes.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateTag()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateTag(ProductType ptype)
        {
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(ptype);
                db.SaveChanges();
                return RedirectToAction(nameof(TagList));
            }
            return View(ptype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditTag(int? id)
        {
            var ptype = db.ProductTypes.Find(id);
            if(ptype==null)
            {
                return NotFound();
            }
            return View(ptype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditTag(ProductType ptype)
        {
            if (ModelState.IsValid)
            {
                db.Update(ptype);
                db.SaveChanges();
                return RedirectToAction(nameof(TagList));
            }
            return View(ptype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteTag(int? id)
        {
            var ptype = db.ProductTypes.Find(id);
            if (ptype == null)
            {
                return NotFound();
            }
            db.ProductTypes.Remove(ptype);
            db.SaveChanges();
            return RedirectToAction(nameof(TagList));
        }

        #endregion 

        #region Roles
        
        [Authorize(Roles = Roles.Master)]
        public async Task<IActionResult> UserRoles()
        {
            var users = await userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.UserName = user.UserName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        public async Task<IActionResult> ManageRoles(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction(nameof(UserRoles));
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await userManager.GetRolesAsync(user));
        }

        #endregion

        #region Socials

        [Authorize(Roles = Roles.Master)]
        public IActionResult SMList()
        {
            return View(db.Socials.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateSM()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateSM(Social sm)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                foreach(var file in Request.Form.Files)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    if(ms.Length < 512000)
                    {
                        imageData = ms.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File","The file is too large.");
                        return View(sm);
                    }
                }
                sm.SMImage = imageData;
                db.Socials.Add(sm);
                db.SaveChanges();
                return RedirectToAction(nameof(SMList));
            }
            return View(sm);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditSM(int? id)
        {
            var sm = db.Socials.Find(id);
            if(sm==null)
            {
                return NotFound();
            }
            return View(sm);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditSM(Social sm)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        file.CopyToAsync(dataStream);
                        if(dataStream.Length < 512000)
                        {
                            sm.SMImage = dataStream.ToArray();
                        }
                        else
                        {
                            ModelState.AddModelError("", "The picture file is too large");
                            return View(sm);
                        }
                    }
                }                  
                db.Update(sm);
                db.SaveChanges();
                return RedirectToAction(nameof(SMList));
            }
            return View(sm);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteSM(int? id)
        {
            var sm = db.Socials.Find(id);
            if (sm == null)
            {
                return NotFound();
            }
            db.Socials.Remove(sm);
            db.SaveChanges();
            return RedirectToAction(nameof(SMList));
        }

        #endregion 
    
        #region Software

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public async Task<IActionResult> MasterSoftwareList()
        {
            var software = db.Softwares.Include(pd => pd.Developer)
                                    .Include(pd => pd.SoftwareType)
                                    .Include(pd => pd.ExtrasLicense);
            return View(await software.ToListAsync());
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> UserSoftwareList(ApplicationUser user)
        {
            user = await userManager.GetUserAsync(User);
            var userId = user.UserName;
            var software = db.Softwares.Where(pd => pd.Uploader.Contains(userId))
                                                    .Include(pd => pd.Developer)
                                                    .Include(pd => pd.SoftwareType)
                                                    .Include(pd => pd.ExtrasLicense);
            return View(await software.ToListAsync());
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult CreateSoftware()
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            ViewData["SoftwareTypeId"] = new SelectList(db.SoftwareTypes,"Id","Type");
            ViewData["ExtrasLicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License");
            ViewData["DeveloperId"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "Id", "Name");
            return View();
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSoftware(Software software, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if(Request.Form.Files.Count > 0)
                {
                    byte[] imageData = null;
                    foreach(var imagefile in Request.Form.Files)
                    {
                        MemoryStream ms = new MemoryStream();
                        imagefile.CopyTo(ms);
                        if(ms.Length < 512000)
                        {
                            imageData = ms.ToArray();
                            software.Image = imageData;
                        }
                        else
                        {
                            ModelState.AddModelError("File","The file is too large.");
                            return View(software);
                        }
                    }
                }
                software.Uploader = userManager.GetUserName(this.User);
                software.DownloadCount = 0;
                software.UploadDate = DateTime.Now;
                db.Add(software);
                await db.SaveChangesAsync(userManager.GetUserName(this.User));
                return Redirect(returnUrl);
            }
            ViewData["SoftwareTypeId"] = new SelectList(db.SoftwareTypes,"Id","Type", software.SoftwareTypeId);
            ViewData["ExtrasLicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",software.ExtrasLicenseId);
            ViewData["DeveloperId"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "Id", "Name", software.DeveloperId);
            return View(software);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult EditSoftware(int? id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var software = db.Softwares.Find(id);     
            if(software==null)
            {
                return NotFound();
            }
            ViewData["SoftwareTypeId"] = new SelectList(db.SoftwareTypes,"Id","Type", software.SoftwareTypeId);
            ViewData["ExtrasLicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",software.ExtrasLicenseId);
            ViewData["Uploader"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "UserName", "UserName", software.Uploader);
            ViewData["DeveloperId"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "Id", "Name", software.DeveloperId);
            return View(software);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpPost, ActionName("EditSoftware")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSoftwarePost(int? id, string returnUrl)
        {
            var software = await db.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Software>(software, "", p => p.Name, p => p.Description, p => p.Url,
                            p => p.PurchaseURL, p => p.ExtrasLicenseId, p => p.SoftwareTypeId, p => p.ReleaseDate,
                            p => p.ExternalDeveloper, p => p.DownloadURL, p => p.DownloadCount, p => p.DeveloperId, p => p.Uploader, p => p.Price))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 512000)
                            {
                                software.Image = dataStream.ToArray();
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(software);
                            }
                        }
                    }
                    var oldsoftware = await db.Softwares.FindAsync(id);
                    db.Entry(oldsoftware).CurrentValues.SetValues(software);
                    await db.SaveChangesAsync(userManager.GetUserName(this.User));
                    return Redirect(returnUrl);
                }
                catch (DbUpdateException)
                {
                   
                }
            }
            ViewData["SoftwareTypeId"] = new SelectList(db.SoftwareTypes,"Id","Type", software.SoftwareTypeId);
            ViewData["ExtrasLicenseId"] = new SelectList(db.ExtrasLicenses,"Id","License",software.ExtrasLicenseId);
            ViewData["Uploader"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "UserName", "UserName", software.Uploader);
            ViewData["DeveloperId"] = new SelectList(userManager.Users.Where(user => user.IsTeam.Equals(true)), "Id", "Name", software.DeveloperId);
            return View(software);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpGet]
        public IActionResult AttachFile(int? id)
        {
            ViewBag.returnUrl = Request.Headers["Referer"].ToString();
            var software = db.Softwares.Find(id);     
            if(software==null)
            {
                return NotFound();
            }
            return View(software);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpPost, ActionName("AttachFile")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AttachFilePost(int? id, string returnUrl)
        {
            var software = await db.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Software>(software))
            {
                try
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault();
                        var filePath = Path.GetFileName(file.FileName);
                        var contentType = Path.GetExtension(filePath);
                        using (var dataStream = new MemoryStream())
                        {
                            await file.CopyToAsync(dataStream);
                            if(dataStream.Length < 20000000)
                            {
                                software.Data = dataStream.ToArray();
                                software.FilePath = filePath;
                                software.ContentType = contentType;
                            }
                            else
                            {
                                ModelState.AddModelError("", "The picture file is too large");
                                return View(software);
                            }
                        }
                    }
                    var oldsoftware = await db.Softwares.FindAsync(id);
                    db.Entry(oldsoftware).CurrentValues.SetValues(software);
                    await db.SaveChangesAsync(userManager.GetUserName(this.User));
                    return Redirect(returnUrl);
                }
                catch (DbUpdateException)
                {
                   
                }
            }
            return View(software);
        }

        [Authorize(Roles = Roles.Master+","+Roles.SuperAdmin)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSoftware(int? id)
        {
            Software software = await db.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            db.Softwares.Remove(software);
            await db.SaveChangesAsync(userManager.GetUserName(this.User));
            return RedirectToAction(nameof(UserSoftwareList));
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> MasterDeleteSoftware(int? id)
        {
            Software software = await db.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            db.Softwares.Remove(software);
            await db.SaveChangesAsync(userManager.GetUserName(this.User));
            return RedirectToAction(nameof(MasterSoftwareList));
        }
        
        #endregion

        #region SoftwareType

        [Authorize(Roles = Roles.Master)]
        public IActionResult TypeList()
        {
            return View(db.SoftwareTypes.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult CreateType()
        {
            return View();
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult CreateType(SoftwareType stype)
        {
            if (ModelState.IsValid)
            {
                db.SoftwareTypes.Add(stype);
                db.SaveChanges();
                return RedirectToAction(nameof(TypeList));
            }
            return View(stype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpGet]
        public IActionResult EditType(int? id)
        {
            var stype = db.SoftwareTypes.Find(id);
            if(stype==null)
            {
                return NotFound();
            }
            return View(stype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditType(SoftwareType stype)
        {
            if (ModelState.IsValid)
            {
                db.Update(stype);
                db.SaveChanges();
                return RedirectToAction(nameof(TypeList));
            }
            return View(stype);
        }

        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteType(int? id)
        {
            var stype = db.SoftwareTypes.Find(id);
            if (stype == null)
            {
                return NotFound();
            }
            db.SoftwareTypes.Remove(stype);
            db.SaveChanges();
            return RedirectToAction(nameof(TypeList));
        }

        #endregion 

        #region Subscribers

        [Authorize(Roles = Roles.Master)]
        public IActionResult SubscriberList()
        {
            return View(db.Subscribers.ToList());
        }

        [Authorize(Roles = Roles.Master)]
        public IActionResult Newletter()
        {
            return View();
        }



        [Authorize(Roles = Roles.Master)]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteSubscriber(int? id)
        {
            var subscriber = db.Subscribers.Find(id);
            if (subscriber == null)
            {
                return NotFound();
            }
            db.Subscribers.Remove(subscriber);
            db.SaveChanges();
            return RedirectToAction(nameof(SubscriberList));
        }

        #endregion

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

    }
}