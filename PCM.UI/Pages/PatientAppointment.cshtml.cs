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
    public class PatientAppointmentModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();

        [BindProperty]
        public List<Patient> patients { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public Appointment appointment { get; set; }
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
            public AppoinmentReason ApmtReason { get; set; }


        }
        public void OnGet(string Id)
        {
            ObjectId patientId = ObjectId.Parse(Id);

            patient = patientServices.GetPatienByID(patientId);

            AppointmentsForTheDay = appointmentServices.GetAppointmentsByDate(AppointmentDate);

            logServices.Log(string.Format("User {0} accessed appointment creation for patient id: {1}", User.Identity.Name,Id));

        }

        public void OnPost(string Id) 
        {

            ValidHours = appointmentServices.ValidHours;

            OnGet(Id);

        }
        public IActionResult OnPostSave(string Id, string Date, string Hour)
        {


            if (DateTime.Parse(Date) != new DateTime(1, 1, 1))
            {
                ObjectId patientId = ObjectId.Parse(Id);

                appointment = new Appointment();

                patient = patientServices.GetPatienByID(patientId);

                appointment.AppointmentHour = Hour;
                appointment.CreatedBy = User.Identity.Name;
                appointment.AppointmentDateStart = DateTime.Parse(Date);
                appointment.PatientId = Id;
                appointment.TimeCreated = DateTime.Today;
                appointment.AppoinmentReason = Input.ApmtReason;
                appointment.Id = ObjectId.GenerateNewId();

                appointmentServices.AddAppointment(appointment);

                logServices.Log(string.Format("User {0} added appointment for patient ID: {1}", User.Identity.Name,Id));

                return RedirectToPage("./AppointmentConfirmation", new { Id = appointment.Id });


            }


            return RedirectToPage("./AppointmentConfirmation");
        }
    }
}
