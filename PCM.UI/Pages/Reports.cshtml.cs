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
    public class ReportsModel : PageModel
    {
        EntityServices entityServices = new EntityServices();
        PaymentServices paymentServices = new PaymentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();
        ARSServices ARSServices = new ARSServices();

        [BindProperty]
        public DateTime Date1 { get; set; } = DateTime.Today;
        [BindProperty]
        public DateTime Date2 { get; set; } = DateTime.Today;
        [BindProperty]
        public string InsuranceInput { get; set; }
        public List<ARS> AllARS { get; set; }
        public List<ReportEntry> Result { get; set; }
        public Entity CurrentEntity { get; set; }
        public int EntityCount { get; set; }
        public string img { get; set; }



        public class ReportEntry
        {
            public string PatientName { get; set; }
            public string Insurance { get; set; }
            public string InsuranceNumber { get; set; }
            public string PaymentId { get; set; }
            public decimal PaymentAmount { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTime DateTime { get; set; }
            public decimal InsuranceCoverage { get; set; }
            public decimal ApmtPrice { get; set; }

        }


        public void OnGet()
        {
            Result = new List<ReportEntry>();

            AllARS = ARSServices.GetAllARS();

            logServices.Log(string.Format("User {0} accessed the report page",User.Identity.Name));

        }

        public async Task<IActionResult> OnPostAsync() 
        {

            AllARS = ARSServices.GetAllARS();

            Result = new List<ReportEntry>();

            if (ModelState.IsValid)
            {
                if (Date1 > Date2 || Date1 > DateTime.Today || Date2 > DateTime.Today)
                {

                    ModelState.AddModelError(string.Empty,"Fecha invalida, verifique que fecha inicial no sea mayor que la final o que no sea fecha futura.");
                    logServices.Log(string.Format("User {0} searched invalid date", User.Identity.Name));

                    return Page();

                }

                var dbPayments = paymentServices.LoadReport(Date1,Date2,InsuranceInput);

                if (dbPayments.Count <= 0)
                {
                    ModelState.AddModelError(string.Empty, "No hay reportes para este rango fechas. ");

                    logServices.Log(string.Format("User {0} searched for date range {1} & {2} with no result", User.Identity.Name,Date1.ToShortDateString(),Date2.ToShortDateString()));

                    return Page();
                }
                //Entity information 

                EntityCount = entityServices.CheckForEntity();

                if (EntityCount > 0)
                {
                    CurrentEntity = entityServices.FindEntityByIdentifier();
                    img = await getImg();
                }

                //End entity information

                foreach (var dbPayment in dbPayments)
                {

                    ReportEntry entry = new ReportEntry();
                    Patient patient = patientServices.GetPatienByID(ObjectId.Parse(dbPayment.PatientId));

                    entry.PatientName = patient.FullName;
                    entry.Insurance = dbPayment.Insurance;
                    entry.PaymentId = dbPayment.Id.ToString();
                    entry.PaymentAmount = dbPayment.TotalPayment;
                    entry.InvoiceNumber = dbPayment.ReceiptNumber;
                    entry.DateTime = dbPayment.EmitedTime;
                    entry.InsuranceCoverage = dbPayment.InsuranceCoverage;
                    entry.ApmtPrice = dbPayment.AppointmentPrice;
                    entry.InsuranceNumber = dbPayment.InsuranceCardnet;

                    Result.Add(entry);

                }

                logServices.Log(string.Format("User {0} searched for date range {1} & {2}", User.Identity.Name, Date1.ToShortDateString(), Date2.ToShortDateString()));

            }

            return null;        
        
        }
        public async Task<string> getImg()
        {
            string photo = await Task.Run(() => Convert.ToBase64String(CurrentEntity.Logo));

            return photo;
        }
    }
}
