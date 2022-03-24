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

namespace PCM.UI.Pages
{
    [Authorize]
    public class AllPrescriptionsModel : PageModel
    {
        PatientServices patientServices = new PatientServices();
        PrescriptionServices prescriptionServices = new PrescriptionServices();
        IndicationServices indicationServices = new IndicationServices();
        UserServices userServices = new UserServices();

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

            return RedirectToPage("./AllPrescriptions", new { Id = patient.Id });
        }
    }
}
