using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PCM.UI.Pages
{
    public class OperationErrorModel : PageModel
    {
        public string ErrorTxt { get; set; }
        public void OnGet(string description)
        {
            ErrorTxt = description;
        }
    }
}
