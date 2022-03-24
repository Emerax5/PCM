using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class PrescriptionModel : PageModel
    {
        EntityServices entityServices = new EntityServices();
        PrescriptionServices prescriptionServices = new PrescriptionServices();
        IndicationServices indicationServices = new IndicationServices();
        PatientServices patientServices = new PatientServices();
        CultureInfo es = new CultureInfo("es-ES");

        public Entity CurrentEntity { get; set; }
        public Prescription CurrentPrescription { get; set; }
        public List<Indication> Indications { get; set; }
        public int EntityCount { get; set; }
        public Patient patient { get; set; }
        public string PrescriptionDate { get; set; }
        public string PrintDate { get; set; }
        public string img { get; set; }
        public int Age { get; set; }

        public async Task OnGetAsync(string pID)
        {
            EntityCount = entityServices.CheckForEntity();

            CurrentPrescription = prescriptionServices.GetPrescriptionById(ObjectId.Parse(pID));        

            Indications = indicationServices.GetIndicationsByPrescriptionId(pID);

            patient = patientServices.GetPatienByID(ObjectId.Parse(CurrentPrescription.PatientId));

            PrescriptionDate = CurrentPrescription.AddedDate.ToLocalTime().ToString("D", es);

            PrintDate = DateTime.Today.ToLocalTime().ToString("D", es);

            if (EntityCount > 0)
            {
                CurrentEntity = entityServices.FindEntityByIdentifier();
                img = await getImg();
            }


            DateTime zeroTime = new DateTime(1, 1, 1);

            DateTime a = patient.BirthDay;

            DateTime b = DateTime.Today.ToLocalTime();

            TimeSpan span = b - a;

            Age = (zeroTime + span).Year - 1;

        }
        public async Task<string> getImg()
        {
            string photo = await Task.Run(() => Convert.ToBase64String(CurrentEntity.Logo));

            return photo;
        }
    }
}
