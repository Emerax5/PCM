using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PCM.UI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel1 : PageModel
    {
        RoleManager<IdentityRole> roleManager;

        public IndexModel1(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public List<IdentityRole> roles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            roles = await roleManager.Roles.ToListAsync();

            return Page();
        }
    }
}