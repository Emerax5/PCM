using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using PCM.Core.AdminTools;
using PCM.Core.Patients;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]

    public class AddARSModel : PageModel
    {

        ARSServices ARSServices = new ARSServices();
        UserServices userServices = new UserServices();
        LogServices logServices = new LogServices();

        [BindProperty]
        public InputModel Input { get; set; }

        public List<ARS> AllARS { get; set; }

        public class InputModel
        {
            public string ARSName { get; set; }

        }

        public IActionResult OnGet()
        {
            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin)
            {
                AllARS = ARSServices.GetAllARS();
                logServices.Log(string.Format("User {0} accessed the ARS menu", User.Identity.Name));

                return Page();
            }
            else
            {
                logServices.Log(string.Format("User {0} tryed to access the ARS menu",User.Identity.Name));
                return RedirectToPage("./NotAllowed");

            }
        }

        public IActionResult OnPost() 
        {
            ARS ars = new ARS();

            if (AllARS == null)
            {
                AllARS = ARSServices.GetAllARS();
            }

            foreach (var ARS in AllARS)
            {
                if (ARS.Name == Input.ARSName)
                {
                    ModelState.AddModelError(string.Empty, "ARS ya existe");

                    return Page();
                }

            }



            if (ModelState.IsValid && Input.ARSName != null)
            {
                ars.AddedBy = User.Identity.Name;
                ars.AddedDate = DateTime.Now;
                ars.Name = Input.ARSName.Trim();

                logServices.Log(string.Format("User {0} added ARS {1}", User.Identity.Name, ars.Name));


                ARSServices.AddARS(ars);
            }



            return RedirectToPage("./AddARS");

        }


        public IActionResult OnPostDelete(string Id)
        {               

            ARSServices.RemoveARSById(ObjectId.Parse(Id));
            logServices.Log(string.Format("User {0} deleted ARS ID: {1}", User.Identity.Name, Id));

            return RedirectToPage("./AddARS");
        }
    }
}
