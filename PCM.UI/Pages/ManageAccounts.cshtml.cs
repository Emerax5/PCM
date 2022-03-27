using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.CommunSevices;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class ManageAccountsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        UserServices userServices = new UserServices();
        User dbUser = new User();
        ErrorMesseges Error = new ErrorMesseges();
        LogServices logServices = new LogServices();

        public List<User> Users { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public User ModUser { get; set; }

        public class InputModel
        {

            [StringLength(100, ErrorMessage = "La {0} debe ser al menos {2}", MinimumLength = 6)]
            public string Password { get; set; }

            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña de confirmacion no coinciden.")]
            public string ConfirmPassword { get; set; }        

        }
        public IActionResult OnGet(string Name)
        {

            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin)
            {

                Users = userServices.GetUsers<User>();

                if (Name != null)
                {
                    OnPostSelect(Name);
                }

                logServices.Log(string.Format("User {0} accessed the manage account page", User.Identity.Name));

                return Page();
               
            }
            else
            {

                logServices.Log(string.Format("User {0} attempted to manage user accounts", User.Identity.Name));
                return RedirectToPage("./NotAllowed");

            }            

        }
        public async Task<IActionResult> OnPostAsyncReset(string userName)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByNameAsync(userName);
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, Input.Password.Trim());
            if (result.Succeeded)
            {
                logServices.Log(string.Format("User {0} changed {1} password", User.Identity.Name,userName));
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();

        }

        public void OnPostSelect(string Name)
        {
            logServices.Log(string.Format("User {0} selected account:{1} at the manage accounts page", User.Identity.Name, Name));
            ModUser = userServices.GetUserByUserName(Name);   
        
        }
    }
}
