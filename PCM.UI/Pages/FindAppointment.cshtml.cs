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
    public class FindAppointmentModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public DateTime Date { get; set; } = DateTime.Today;
        [BindProperty]
        public List<Appointment> AppointmentsForTheDay { get; set; }
        [BindProperty]
        public string[] ValidHours { get; set; } = new AppointmentServices().ValidHours;

        public List<Patient> Patients { get; set; }
        public void OnGet()
        {
            AppointmentsForTheDay = appointmentServices.GetAppointmentsByDate(Date);

            var apmtList = appointmentServices.GetAppointmentsByDate(Date);

            Patients = new List<Patient>();
        

            if (apmtList.Count > 0)
            {

                //change status to no show if needed
                foreach (var Apmt in apmtList)
                {
                    if ((Apmt.Status != Status.Cancelled && Apmt.Status != Status.Completed) && Apmt.AppointmentDateStart < DateTime.Today.ToLocalTime())
                    {
                        appointmentServices.ChangeStatus(Apmt.Id, (int)Status.NoShow, Apmt.AppointmentHour);
                    }

                }

                AppointmentsForTheDay = appointmentServices.GetAppointmentsByDate(Date);

                foreach (var Apmt in AppointmentsForTheDay)
                {
                    ObjectId id = ObjectId.Parse(Apmt.PatientId);
                    Patients.Add(patientServices.GetPatienByID(id));
                }

            }
            logServices.Log(string.Format("User {0} accessed appointment search for date: {1}",User.Identity.Name, Date.ToShortDateString()));
        }

        public void OnPost() 
        {

            ValidHours = appointmentServices.ValidHours;

            OnGet();

        }
    }
}
