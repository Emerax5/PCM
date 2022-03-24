using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PCM.Core.CommunSevices;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        UserServices _userServices = new UserServices();
        User dbUser = new User();
        ErrorMesseges Error = new ErrorMesseges();
        [BindProperty]
        public Role UserRole { get; set; }

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string User { get; set; }

            public string securityAnswer { get; set; }

            public string securityQuestion { get; set; }

            [StringLength(100, ErrorMessage = "La {0} debe ser minimo {2} caracteres", MinimumLength = 6)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La clave de confirmación no coincide.")]
            public string ConfirmPassword { get; set; }

           
        }

        public void OnGet(string UserFP)
        {
            if (User.Identity.IsAuthenticated)
            {
                UserRole = _userServices.GetRoleByUserName(User.Identity.Name);
            }
            dbUser = _userServices.GetUserByUserName(UserFP);
            Input.User = dbUser.UserName;
            string question = "";
            question = dbUser.SecurityQuestion;
            Input.securityQuestion = question;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User dbuser = _userServices.GetUserByUserName(Input.User);
            if (User.Identity.IsAuthenticated)
            {
                UserRole = _userServices.GetRoleByUserName(User.Identity.Name);
            }


            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserRole != Role.Admin)
            {
                if (!(dbuser.SecurityAnswer == Input.securityAnswer.Trim()))
                {

                    ModelState.AddModelError(string.Empty, Error.ConfirmationAnswerNotMatch);

                    return Page();
                }
            }
          

            var user = await _userManager.FindByNameAsync(Input.User);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, Input.Password.Trim());
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
