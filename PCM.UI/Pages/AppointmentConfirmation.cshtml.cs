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
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class AppointmentConfirmationModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();

        [BindProperty]
        public Patient patient { get; set; }
        [BindProperty]
        public Appointment appointment { get; set; }

        public void OnGet(string Id)
        {
            appointment = appointmentServices.GetAppointmentsById(ObjectId.Parse(Id));

            patient = patientServices.GetPatienByID(ObjectId.Parse(appointment.PatientId));
        }
     
    }
}
