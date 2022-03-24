using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.DTO.DTOModels;
using PCM.Core.Users;
using PCM.Core.Patients;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;

namespace PCM.UI.Pages.Shared
{
    [Authorize]
    public class EditEntryModel : PageModel
    {
        MedicalEntryServices medicalEntryServices = new MedicalEntryServices();
        UserServices userServices = new UserServices();

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public HistoryEntry Entry { get; set; }
        [BindProperty]
        public string MedicalEntry { get; set; }


        public class InputModel
        {

            public MedicalEntryType Type { get; set; } 
            public DateTime Date { get; set; }

        }

        public IActionResult OnGet(string ID)
        {
            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin || userServices.GetRoleByUserName(User.Identity.Name) == Role.Doctor)
            {
                if (ID != null)
                {

                    Entry = medicalEntryServices.GetEntryById(ObjectId.Parse(ID));
                    MedicalEntry = Entry.EntryText;

                }

                return Page();
            }
            else
            {

                return RedirectToPage("./NotAllowed");

            }

        }

        public IActionResult OnPostEdit(string Id) 
        {
            ObjectId entryId = ObjectId.Parse(Id);

            Entry = medicalEntryServices.GetEntryById(entryId);

            var dbEntry = new HistoryEntry();   



            //Edit Entry data
            if (MedicalEntry == null || MedicalEntry.Trim().Count() == 0) dbEntry.EntryText = Entry.EntryText; else dbEntry.EntryText = MedicalEntry;
            dbEntry.EntryType = Input.Type;
            if (Input.Date == default || Input.Date > DateTime.Today) dbEntry.EntryDate = Entry.EntryDate; else dbEntry.EntryDate = Input.Date;

            //Set permanet fields
            dbEntry.CreatedBy = Entry.CreatedBy;
            dbEntry.Id = Entry.Id;
            dbEntry.PatientId = Entry.PatientId;


   

            medicalEntryServices.UpsertEntry(Entry.Id, dbEntry);

            return RedirectToPage("./MedicalHistory", new { ID = Entry.PatientId });
        }
    }
}
