using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DeviantMusicCore.Models;
using Microsoft.AspNetCore.Hosting;

namespace DeviantMusicCore.Areas.Identity.Pages.Shared.Components.UsersList
{
    public class UsersListViewComponent : ViewComponent{
        private readonly UserManager<ApplicationUser> userManager;
        
        public UsersListViewComponent(UserManager<ApplicationUser> _userManager)
        {
            userManager = _userManager;
        }

        public IViewComponentResult Invoke()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IViewComponentResult> DeletesUser(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View();
            }
            IdentityResult result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{id}'.");
            }

            return View();
        }
    }
}