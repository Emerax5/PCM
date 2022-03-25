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

        public void OnGet()
        {
            UserCount = userServices.UserCount();

            if (User.Identity.Name != null)
            {
                user = userServices.GetUserByUserName(User.Identity.Name);

            }


        }
    }
}
