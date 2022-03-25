using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages.Shared
{
    public class NotAllowedModel : PageModel
    {
        LogServices logServices = new LogServices();

        public IActionResult OnGet()
        {


            Thread.Sleep(10000);


            return RedirectToPage("./Index");

        }
    }
}
