using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DeviantMusicCore.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DeviantMusicCore.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
        public string Designation { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Stage Name (For Artiste)")]
            public string StageName { get; set; }

            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Social Media Url")]
            public string SocialUrl { get; set; }

            [Display(Name = "Bio")]
            public string Bio { get; set; }

            
            [Display(Name = "AVI (512kb max)")]
            public byte[] UserImage { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var ubio = user.Bio;
            var uname = user.Name;
            var udesig = user.Designation;
            var ustagename = user.StageName;
            var usocial = user.SocialUrl;
            var userImage = user.UserImage;

            Username = userName;
            
            Designation = udesig;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Bio = ubio,
                StageName = ustagename,
                UserImage = userImage,
                SocialUrl = usocial,
                Name = uname
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            var ubio = user.Bio;
            var uname = user.Name;
            var ustagename = user.StageName;
            var usocial = user.SocialUrl;
            if (Input.Name != uname)
            {

                user.Name = Input.Name;
                await _userManager.UpdateAsync(user);
            }
            if (Input.SocialUrl != usocial)
            {

                user.SocialUrl = Input.SocialUrl;
                await _userManager.UpdateAsync(user);
            }
            if (Input.StageName != ustagename)
            {

                user.StageName = Input.StageName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Bio != ubio)
            {
                user.Bio = Input.Bio;
                await _userManager.UpdateAsync(user);
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    if(dataStream.Length < 512000)
                    {
                        user.UserImage = dataStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("", "The picture file is too large");
                        return Page();
                    }
                }
                await _userManager.UpdateAsync(user);
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
