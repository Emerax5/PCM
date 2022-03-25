using System;
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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PCM.DTO.DTOModels;
using PCM.Core.Users;
using static Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal.ExternalLoginModel;
using PCM.Core.CommunSevices;
using PCM.Core.AdminTools;

namespace PCM.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        ErrorMesseges error = new ErrorMesseges();
        UserServices userServices = new UserServices();
        LogServices logServices = new LogServices();


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;           
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public int UserCount { get; set; }
        public User user { get; set; }

        public Role UserRole { get; set; }

        public class InputModel
        {
            [StringLength(100, ErrorMessage = "El {0} debe ser al menos {2} caracteres.", MinimumLength = 5)]
            [Display(Name = "Usuario")]
            [Required(ErrorMessage = "Campo requerido")]
            public string User { get; set; }

            [StringLength(100, ErrorMessage = "El {0} debe ser al menos {2} caracteres.", MinimumLength = 6)]
            [Display(Name = "Contraseña")]
            [Required(ErrorMessage = "Campo requerido")]
            public string Password { get; set; }

            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "La clave de confirmación no coincide.")]
            [Required(ErrorMessage = "Campo requerido")]
            public string ConfirmPassword { get; set; }
            [Required (ErrorMessage = "Campo requerido")]
            [Display(Name = "Pregunta de seguridad")]
            public string SecurityQuestion { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            [Display(Name = "Respuesta de seguridad")]
            public string SecurityAnswer { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            public Role Role { get; set; }

        }
        public void  OnGet()
        {
           
            UserCount = userServices.UserCount();           

            if (User.Identity.Name == null)
            {
                UserRole = Role.Secretary;
            }
            else
            {
                user = userServices.GetUserByUserName(User.Identity.Name);
                UserRole = user.Role;
            }

            if (User.Identity.Name != null)
            {
                user = userServices.GetUserByUserName(User.Identity.Name);

            }
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.User.Trim(), Email = Input.User.Trim(), EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Input.Password.Trim());               

                if (result.Succeeded)
                {                   
                    var dbUser = new User();
                    UserServices db = new UserServices();

                    dbUser.UserName = Input.User.ToString().Trim();
                    dbUser.SecurityQuestion = Input.SecurityQuestion.ToString().Trim();
                    dbUser.SecurityAnswer = Input.SecurityAnswer.ToString().Trim();
                    dbUser.Role = Input.Role;

                    db.AddUser(dbUser);

                    logServices.Log("User " +dbUser.UserName +" created, " + "Role: " + dbUser.Role.DisplayName());
                    return RedirectToPage("RegisterConfirmation", new { email = Input.User, returnUrl = returnUrl });                       
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    logServices.Log("Fail to create user" + error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
