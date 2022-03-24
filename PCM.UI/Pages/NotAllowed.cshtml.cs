using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PCM.UI.Pages.Shared
{
    public class NotAllowedModel : PageModel
    {
        public IActionResult OnGet()
        {


            Thread.Sleep(10000);


            return RedirectToPage("./Index");

        }
    }
}
