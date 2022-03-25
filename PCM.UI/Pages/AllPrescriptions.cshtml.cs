using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.DTO.DTOModels;
using PCM.Core.Patients;
using MongoDB.Bson;
using PCM.Core.Users;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{
    [Authorize]
    public class AllPrescriptionsModel : PageModel
    {
        PatientServices patientServices = new PatientServices();
        PrescriptionServices prescriptionServices = new PrescriptionServices();
        IndicationServices indicationServices = new IndicationServices();
        UserServices userServices = new UserServices();
        LogServices logServices = new LogServices();


        public Patient patient { get; set; }
        public List<Prescription> prescriptions { get; set; }
        public List<Indication> indications { get; set; }
        public Role UserRole { get; set; }
        public void OnGet(string Id)
        {
            patient = patientServices.GetPatienByID(ObjectId.Parse(Id));

            prescriptions = prescriptionServices.GetAllPrescriptionsByPatientId(Id);

            UserRole = userServices.GetRoleByUserName(User.Identity.Name);

            foreach (var pres in prescriptions)
            {
                indications = indicationServices.GetIndicationsByPrescriptionId(pres.Id.ToString());

                if (indications.Count <= 0)
                {
                    prescriptionServices.RemovePrescriptionById(pres.Id);
                }

            }

            prescriptions = prescriptionServices.GetAllPrescriptionsByPatientId(Id);

            logServices.Log(string.Format("User {0} accessed prescriptions for patien id {1}", User.Identity.Name, Id));

        }
        public IActionResult OnPostDelete(string PId)
        {
            var pres = prescriptionServices.GetPrescriptionById(ObjectId.Parse(PId));

            patient = patientServices.GetPatienByID(ObjectId.Parse(pres.PatientId));

            prescriptionServices.RemovePrescriptionById(ObjectId.Parse(PId));

            indications = indicationServices.GetIndicationsByPrescriptionId(PId);

            foreach (var ind in indications)
            {
                indicationServices.RemoveIndicationById(ind.Id);
            }
            logServices.Log(string.Format("User {0} deleted prescription for patien {1}, patien ID: {2}", User.Identity.Name,patient.FullName,patient.Id));


            return RedirectToPage("./AllPrescriptions", new { Id = patient.Id });
        }
    }
}
