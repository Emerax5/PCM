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
    public class AddMedicineModel : PageModel
    {

        MedicineServices medicineServices = new MedicineServices();
        UserServices userServices = new UserServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public InputModel Input { get; set; }

        public List<Medicine> AllMedicines { get; set; }
        public Role UserCurrentRole { get; set; }

        public class InputModel
        {
            public string ComercialName { get; set; }
            public string GenericName { get; set; }
            public string Lab { get; set; }
            public string SideEffects { get; set; }
            public string Description { get; set; }

        }

        public void OnGet()
        {

            UserCurrentRole = userServices.GetRoleByUserName(User.Identity.Name);

            AllMedicines = medicineServices.GetAllMedicines();

            logServices.Log(string.Format("User {0} accessed Medicine menu", User.Identity.Name));

        }

        public IActionResult OnPost()
        {
            Medicine medicine = new Medicine();

            if (AllMedicines == null)
            {
                AllMedicines = medicineServices.GetAllMedicines();
            }

            foreach (var Medicine in AllMedicines)
            {
                if (Medicine.ComercialName.Trim() == Input.ComercialName)
                {
                    ModelState.AddModelError(string.Empty, "Farmaco ya existe");

                    return Page();
                }

            }



            if (ModelState.IsValid && Input.ComercialName != null)
            {
                medicine.AddedBy = User.Identity.Name;
                medicine.TimeAdded = DateTime.Now;
                if (Input.ComercialName != null) medicine.ComercialName = Input.ComercialName.Trim();
                if (Input.GenericName != null) medicine.GenericName = Input.GenericName.Trim();
                if (Input.Lab != null) medicine.lab = Input.Lab.Trim();
                if (Input.SideEffects != null) medicine.SideEffects = Input.SideEffects.Trim();
                if (Input.Description != null) medicine.Description = Input.Description.Trim();

                medicineServices.AddMedicine(medicine);
                logServices.Log(string.Format("User {0} added medicine {1}", User.Identity.Name, medicine.ComercialName));

            }

            return RedirectToPage("./AddMedicine");

        }
        public IActionResult OnPostDelete(string Id)
        {

            medicineServices.RemoveMedicineById(ObjectId.Parse(Id));
            logServices.Log(string.Format("User {0} deleted medicine ID: {1}", User.Identity.Name, Id));

            return RedirectToPage("./AddMedicine");
        }
    }
}
