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
    public class EditDateModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();

        [BindProperty]
        public List<Patient> patients { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public Appointment Appointment { get; set; }
        [BindProperty]
        public List<Appointment> AppointmentsForTheDay { get; set; }
        [BindProperty]
        public int ResultsCount { get; set; }
        [BindProperty]
        public Patient patientModel { get; set; }
        [BindProperty]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;
        [BindProperty]
        public Patient patient { get; set; }
        [BindProperty]
        public string[] ValidHours { get; set; } = new AppointmentServices().ValidHours;

        public class InputModel
        {

            public string PateintSerch { get; set; }

        }
        public void OnGet(string Id, string ApmtId)
        {
            ObjectId patientId = ObjectId.Parse(Id);

            ObjectId apomtId = ObjectId.Parse(ApmtId);

            Appointment = appointmentServices.GetAppointmentsById(apomtId);

            patient = patientServices.GetPatienByID(patientId);

            AppointmentsForTheDay = appointmentServices.GetAppointmentsByDate(AppointmentDate);          


        }

        public void OnPost(string Id, string ApmtId) 
        {
            ValidHours = appointmentServices.ValidHours;

            OnGet(Id, ApmtId);

        }

        public IActionResult OnPostEditDate(string ApmtId, string date, string hour)         
        {

            ObjectId id = ObjectId.Parse(ApmtId);

            appointmentServices.Reschedule(id, hour, DateTime.Parse(date), User.Identity.Name );

            return RedirectToPage("./Appointment", new { ID = ApmtId });
        }

    }
}
