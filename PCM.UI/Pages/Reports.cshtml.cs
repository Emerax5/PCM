using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.Patients;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    public class ReportsModel : PageModel
    {
        EntityServices entityServices = new EntityServices();
        PaymentServices paymentServices = new PaymentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();
        ARSServices ARSServices = new ARSServices();

        public DateTime Date1 { get; set; } = DateTime.Today;
        public DateTime Date2 { get; set; } = DateTime.Today;
        public string Insurance { get; set; }
        public List<ARS> AllARS { get; set; }
        public List<ReportEntry> Result { get; set; }

        public class ReportEntry
        {
            public string PatientName { get; set; }
            public string Insurance { get; set; }
            public string PaymentId { get; set; }
            public decimal PaymentAmount { get; set; }
            public int InvoiceNumber { get; set; }
            public DateTime DateTime { get; set; }
            public decimal InsuranceCoverage { get; set; }
            public decimal ApmtPrice { get; set; }

        }


        public void OnGet()
        {

            AllARS = ARSServices.GetAllARS();

            logServices.Log(string.Format("User {0} accessed the report page",User.Identity.Name));

        }

        public void OnPost() 
        {
        
        
        
        
        }
    }
}
