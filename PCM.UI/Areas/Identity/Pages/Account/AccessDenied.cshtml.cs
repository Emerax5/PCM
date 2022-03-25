using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;

namespace PCM.UI.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        LogServices logServices = new LogServices();

        public void OnGet()
        {
            logServices.Log(string.Format("Access denied to user {0}", User.Identity.Name));

        }
    }
}

