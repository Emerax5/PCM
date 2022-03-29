using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class LoggerModel : PageModel
    {
        LogServices logServices = new LogServices();
        UserServices userServices = new UserServices();

        [BindProperty]
        public DateTime Date { get; set; } = DateTime.Today;
        [BindProperty]
        public List<LogEntry> LogForTheDay { get; set; }

        public IActionResult OnGet()
        {

            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin) 
            {

                LogForTheDay = logServices.GetLogsByDate(Date);

                return Page();

            }
            else
            {
                logServices.Log(string.Format("User {0} attempted to access Logger", User.Identity.Name));

                return RedirectToPage("./NotAllowed");

            }


        }

        public void OnPost()
        {

            OnGet();

        }
    }
}
