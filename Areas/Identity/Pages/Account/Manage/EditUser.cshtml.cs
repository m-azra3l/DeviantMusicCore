using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DeviantMusicCore.Models;
using DeviantMusicCore.Logic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace DeviantMusicCore.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = Roles.Master)]
    public partial class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditUserModel(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Display(Name = "Stage Name")]
            public string StageName { get; set; }

            [Display(Name = "Designation")]
            public string Designation { get; set; }

            [Display(Name = "Artiste")]
            public bool IsArtiste { get; set; }

            [Display(Name = "Team Member")]
            public bool IsTeam { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var udesignation = user.Designation;
            var ustagename = user.StageName;
            var uisteam = user.IsTeam;
            var uisartiste = user.IsArtiste;

            Username = userName;

            Input = new InputModel
            {
                UserName = userName,
                Designation = udesignation,
                StageName = ustagename,
                IsTeam = uisteam,
                IsArtiste = uisartiste
            };
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var udesignation = user.Designation;
            var ustagename = user.StageName;
            var uisteam = user.IsTeam;
            var uisartiste = user.IsArtiste;
            if (Input.Designation != udesignation)
            {

                user.Designation = Input.Designation;
                await _userManager.UpdateAsync(user);
            }
            if (Input.StageName != ustagename)
            {

                user.StageName = Input.StageName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.IsArtiste != uisartiste)
            {

                user.IsArtiste = Input.IsArtiste;
                await _userManager.UpdateAsync(user);
            }
            if (Input.IsTeam != uisteam)
            {

                user.IsTeam = Input.IsTeam;
                await _userManager.UpdateAsync(user);
            }
            StatusMessage = "User has been updated";
            return RedirectToAction("UsersList","Admin");
        }
    }
}
