using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.Patients;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class FindPatiensModel : PageModel
    {
        PatientServices dbPatientList = new PatientServices();
        LogServices logServices = new LogServices();


        public IList<Patient> Patients { get; set; }

        [BindProperty]
        public Patient patientModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }
        [BindProperty]
        public int pageSize { get; set; } = 7;
        [BindProperty]
        public int Pages { get; set; }
        [BindProperty]
        public int CurrentPage { get; set; }
        [BindProperty]
        public int PrevPage { get; set; }
        [BindProperty]
        public int NextPage { get; set; }
        [BindProperty]
        public int ResultsCount { get; set; }

        public class InputModel
        {
            public string FindRequest { get; set; }         
         
        }

        public void OnGet(int pageNumber)
        {
            if (pageNumber != 0)
            {
                OnPostRequestAll(pageNumber);
            }

        }

        public IActionResult OnPostRequestName()
        {
            string name = "";

            name = Input.FindRequest;

            if (name == null) name = "*";

            Patients = dbPatientList.GetPatientsByFullName(name);                  

            ResultsCount = Patients.Count();

            logServices.Log(string.Format("User {0} searched patient name :{1} on the search patient page", User.Identity.Name,name));

            return Page();
        
        }

        public IActionResult OnPostRequestID()
        {
            Patients = dbPatientList.GetPatientsByPersonalID(Input.FindRequest);

            ResultsCount = Patients.Count();

            logServices.Log(string.Format("User {0} searched patient personal id :{1} on the search patient page", User.Identity.Name,Input.FindRequest));

            return Page();

        }

        public IActionResult OnPostRequestAll(int pageNumber)
        {
            

            Pages = dbPatientList.GetPatientPagesCount(pageSize);

            if (pageNumber == 0) pageNumber = 1;

            CurrentPage = pageNumber;

            Patients = dbPatientList.GetPatientsWithPaging(pageSize, CurrentPage);

            PrevPage = CurrentPage - 1;

            NextPage = CurrentPage + 1;
           
            ResultsCount = dbPatientList.GetAllPatientsCount();

            if (CurrentPage == 1) logServices.Log(string.Format("User {0} requested a full list of all patients at the search patient page", User.Identity.Name));

            return Page();

        }


    }
}
