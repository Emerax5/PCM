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
    public class MedicalHistoryModel : PageModel
    {
        PatientServices patientServices = new PatientServices();
        UserServices userServices = new UserServices();
        MedicalEntryServices medicalEntryServices = new MedicalEntryServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public Patient Patient { get; set; }
        [BindProperty]       
        public InputModel Input { get; set; }
        [BindProperty]
        public List<HistoryEntry> PatientHistory { get; set; }
        [BindProperty]
        public List<DateTime> EntryDates { get; set; }


        public class InputModel
        {

            public string MedicalEntry { get; set; }
            public MedicalEntryType Type { get; set; }

        }

        public IActionResult OnGet(string Id)
        {

            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin || userServices.GetRoleByUserName(User.Identity.Name) == Role.Doctor)
            {
                Patient = patientServices.GetPatienByID(ObjectId.Parse(Id));

                PatientHistory = medicalEntryServices.GetEntriesByPatientId(Id).OrderByDescending(d => d.EntryDate).ToList();

                List<DateTime> Dates = new List<DateTime>();

                foreach (var Entry in PatientHistory)
                {
                    if (!Dates.Contains(Entry.EntryDate))
                    {
                        Dates.Add(Entry.EntryDate);
                    }
                }

                EntryDates = Dates;

                return Page();

            }
            else
            {
                return RedirectToPage("./NotAllowed");

            }


        }
        public IActionResult OnPostAdd(string Id) 
        {
            var newEntry = new HistoryEntry();

            newEntry.CreatedBy = User.Identity.Name;
            newEntry.PatientId = Id;
            newEntry.EntryDate = DateTime.Today;
            newEntry.EntryText = Input.MedicalEntry;
            newEntry.EntryType = Input.Type;

            medicalEntryServices.AddEntry(newEntry);

            return RedirectToPage("./MedicalHistory", new { ID = Id });
        }

        public IActionResult OnPostDeleteEntry(string EntryId, string Id) 
        {
            ObjectId entryId = ObjectId.Parse(EntryId);
            medicalEntryServices.DeleteMedicaEntryById(entryId);

            return RedirectToPage("./MedicalHistory", new { ID = Id });
        }
    }
}
