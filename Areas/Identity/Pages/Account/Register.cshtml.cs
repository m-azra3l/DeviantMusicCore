using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using DeviantMusicCore.Logic;
using DeviantMusicCore.Models;
using DeviantMusicCore.Data;

namespace DeviantMusicCore.Areas.Identity.Pages.Account
{
    [Authorize(Roles = Roles.Master)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "Designation")]
            public string Designation { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [BindNever]
            [Display(Name = "Avatar Image")]
            public byte[] UserImage { get; set; }


            [Display(Name = "Admin")]
            public bool IsAdmin { get; set; }

            [Display(Name = "Super Admin")]
            public bool IsSuperAdmin { get; set; }

            [Display(Name = "Master")]
            public bool IsMaster { get; set; }

            [Display(Name = "Member")]
            public bool IsMember { get; set; }

            [Display(Name = "Artiste")]
            public bool IsArtiste { get; set; }

            [Display(Name = "Team Member")]
            public bool IsTeam { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
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
                        ModelState.AddModelError("","The picture file is too large.");
                        return Page();
                    }
                }
                var userNameExists = await _userManager.FindByNameAsync(Input.UserName);
                if (userNameExists != null)
                {
                    ModelState.AddModelError("","Username already taken. Select a different username.");
                    return RedirectToPage();
                }
                var emailExists = await _userManager.FindByNameAsync(Input.Email);
                if (emailExists != null)
                {
                    ModelState.AddModelError("","You already registered with this email.");
                    return RedirectToPage();
                }
                var user = new ApplicationUser
                { 
                    UserName = Input.UserName,
                    Email = Input.Email,
                    Name = Input.Name,
                    IsArtiste = Input.IsArtiste,
                    IsTeam = Input.IsTeam,
                    Designation = Input.Designation 
                };
                user.EmailConfirmed = true;
                user.UserImage = imageData;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //Create roles if not exisits
                    if(!await _roleManager.RoleExistsAsync(Roles.Master))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Master));
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.SuperAdmin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin));
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(Roles.Member))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Member));
                    }

                    //Assign user to a role as per the check box selection
                
                    if(Input.IsMaster)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Master);
                    }
                    else if(Input.IsSuperAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.SuperAdmin);
                    }
                    else if(Input.IsAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Admin);
                    }
                    else if(Input.IsMember)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Member);
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("UsersList","Admin");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
