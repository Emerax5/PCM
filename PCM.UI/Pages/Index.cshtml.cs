using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PCM.Core.AdminTools;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        UserServices userServices = new UserServices();
        LogServices logServices = new LogServices();


        public int UserCount { get; set; }
        public User user { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            try
            {
                UserCount = userServices.UserCount();

                if (User.Identity.Name != null)
                {

                    user = userServices.GetUserByUserName(User.Identity.Name);

                }

                logServices.Log(string.Format("ip {0} accessed the index page. {1}", logServices.GetLocalIPAddress(), User.Identity.Name));


                return Page();

            }
            catch (Exception e)
            {
                string error = e.Message.ToString();

                logServices.OfflineLog("Connection error to the mongodb data base while looking for the User count. System Error:",error);

                return RedirectToPage("./ConnectionError");
            }

          


        }
    }
}
