using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{
    public class ConnectionErrorModel : PageModel
    {

        LogServices logServices = new LogServices();

        public void OnGet()
        {
            
        }
    }
}
