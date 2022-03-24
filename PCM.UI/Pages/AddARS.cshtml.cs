using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
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
                return Page();
            }
            else
            {
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

                ARSServices.AddARS(ars);
            }



            return RedirectToPage("./AddARS");

        }


        public IActionResult OnPostDelete(string Id)
        {           

            ARSServices.RemoveARSById(ObjectId.Parse(Id));            

            return RedirectToPage("./AddARS");
        }
    }
}
