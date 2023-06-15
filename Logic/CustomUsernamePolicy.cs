using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantMusicCore.Models;
using Microsoft.AspNetCore.Identity;

namespace DeviantMusicCore.Logic
{
    public class CustomUsernamePolicy : UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (user.UserName == "google")
            {
                errors.Add(new IdentityError
                {
                    Description = "Google cannot be used as a user name"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
