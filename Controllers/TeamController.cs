using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DeviantMusicCore.Models;
using DeviantMusicCore.ViewModels;
using DeviantMusicCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviantMusicCore.Controllers
{
    public class TeamController : Controller
    {
        private readonly DeviantContext db;
        private readonly UserManager<ApplicationUser> userManager;
        
        public TeamController(DeviantContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 9;
            var users = userManager.Users.Where(user => user.IsTeam.Equals(true))
                                            .OrderByDescending(user => user.Id);

            return View(await PaginatedList<ApplicationUser>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Member(string id)
        {
            ApplicationUser user = await userManager.Users.Include(u => u.BlogItems)
                                                            .Include(u => u.Softwares)
                                                            .AsSplitQuery().OrderByDescending(u => u.Id)
                                                            .FirstOrDefaultAsync(u => u.UserName == id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }
            return View(user);
        }
    }
}