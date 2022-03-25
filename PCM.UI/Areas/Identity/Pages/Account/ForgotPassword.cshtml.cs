using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PCM.Core.CommunSevices;
using PCM.Core.AdminTools;

namespace PCM.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]

    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        ErrorMesseges error = new ErrorMesseges();
        LogServices logServices = new LogServices();


        public ForgotPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Campor requerido")]
            public string User { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.User.Trim());

                if (!(user == null))
                {
                    logServices.Log(string.Format("User {0} atempting to change password",Input.User.Trim()));
                    return RedirectToPage("./ResetPassword", new {UserFP = Input.User.Trim() });
                }
                
            }
            logServices.Log("User not found to change password, User:" + Input.User.Trim());
            ModelState.TryAddModelError(String.Empty, error.UserNotFound);

            return Page();
        }
    }
}
