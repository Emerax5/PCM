using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using PCM.Core.Patients;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class PrescribeModel : PageModel
    {
        PrescriptionServices prescriptionServices = new PrescriptionServices();
        PatientServices patientServices = new PatientServices();
        MedicineServices medicineServices = new MedicineServices();
        IndicationServices indicationServices = new IndicationServices();
        public Prescription CurrentPrescription { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<Medicine> medicines { get; set; }
        public List<Indication> Indications { get; set; }

        public class InputModel
        {

            public string Medication { get; set; }
            public string Indication { get; set; }



        }

        public void OnGet(string Id, string pID)
        {
            medicines = medicineServices.GetAllMedicines();

            if (Id != null)
            {
                var newprescription = new Prescription();

                newprescription.AddedBy = User.Identity.Name;
                newprescription.AddedDate = DateTime.Now;
                newprescription.PatientId = Id;
                newprescription.Id = ObjectId.GenerateNewId();

                Indications = indicationServices.GetIndicationsByPrescriptionId(newprescription.Id.ToString());

                CurrentPrescription = newprescription;

                prescriptionServices.AddPrescription(newprescription);
            }
            if (pID != null)
            {
                CurrentPrescription = prescriptionServices.GetPrescriptionById(ObjectId.Parse(pID));

                Indications = indicationServices.GetIndicationsByPrescriptionId(pID);
            }

        }

        public IActionResult OnPostDelete(string pID) 
        {

            CurrentPrescription = prescriptionServices.GetPrescriptionById(ObjectId.Parse(pID));

            string PatientId = CurrentPrescription.PatientId;

            Indications = indicationServices.GetIndicationsByPrescriptionId(pID);

            foreach (var Ind in Indications)
            {
                indicationServices.RemoveIndicationById(Ind.Id);
            }

            prescriptionServices.RemovePrescriptionById(ObjectId.Parse(pID));

            return RedirectToPage("./MedicalHistory", new { Id = PatientId });
        }
        public IActionResult OnPostAdd(string pID) 
        {

            Indication newIndication = new Indication();

            newIndication.Medicine = Input.Medication;
            newIndication.PrescriptionId = pID;
            newIndication.Details = Input.Indication.Trim();

            indicationServices.AddIndication(newIndication);

            return RedirectToPage("./Prescribe", new { pID = pID });
        }
        public IActionResult OnPostDeleteIndication(string IndId) 
        {

            Indication curentInd = indicationServices.GetIndicationById(ObjectId.Parse(IndId));

            CurrentPrescription = prescriptionServices.GetPrescriptionById(ObjectId.Parse(curentInd.PrescriptionId));

            indicationServices.RemoveIndicationById(ObjectId.Parse(IndId));

            return RedirectToPage("./Prescribe", new { pID = CurrentPrescription.Id });
        }
    }
}
