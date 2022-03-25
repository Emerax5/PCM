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
    public class AppointmentModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PaymentServices paymentServices = new PaymentServices();
        PatientServices patientServices = new PatientServices();
        LogServices logServices = new LogServices();


        public Appointment appointment { get; set; }
        public Patient patient { get; set; }
        [BindProperty]
        public string NoteInProfile { get; set; }
        public bool PaymentStatus { get; set; }
        public void OnGet(string Id)
        {

            PaymentStatus = paymentServices.IsAppointmentPaid(Id);

            ObjectId apmtId = ObjectId.Parse(Id);

           var Apmt = appointmentServices.GetAppointmentsById(apmtId);

            if ((Apmt.Status != Status.Cancelled && Apmt.Status != Status.Completed) && Apmt.AppointmentDateStart < DateTime.Today.ToLocalTime())
            {
                appointmentServices.ChangeStatus(Apmt.Id, (int)Status.NoShow, Apmt.AppointmentHour);
            }

            ObjectId patientId = ObjectId.Parse(Apmt.PatientId);

            patient = patientServices.GetPatienByID(patientId);

            if (!(Apmt.AppointmentNotes == null))
            {
                NoteInProfile = Apmt.AppointmentNotes;
            }

            appointment = appointmentServices.GetAppointmentsById(apmtId);

            logServices.Log(string.Format("User {0} accessed appointment ID:{1} for patien Id {2}, ", User.Identity.Name, Id, patient.Id));


        }

        public IActionResult OnPostUpdateNote(string Id)
        {
            ObjectId ApmtId = ObjectId.Parse(Id);

            appointmentServices.UpsertNote(ApmtId, NoteInProfile, User.Identity.Name);

            logServices.Log(string.Format("User {0} updated notes on appointment ID: {1}", User.Identity.Name,Id));

            return RedirectToPage("./Appointment", new { ID = Id });
        }

        public IActionResult OnPostChangeStatus(string Id, Status Status) 
        {
            ObjectId ApmtId = ObjectId.Parse(Id);

            string hour;

            appointment = appointmentServices.GetAppointmentsById(ApmtId);

            if (Status == Status.Cancelled)
            {
                hour = "*" + appointment.AppointmentHour;
            }
            else
            {
                hour = appointment.AppointmentHour;
            }

            appointmentServices.ChangeStatus(ApmtId, (int)Status, hour);

            logServices.Log(string.Format("User {0} changed status for appointment ID: {1} to {2}", User.Identity.Name, Id, Status.DisplayName()));


            return RedirectToPage("./Appointment", new { ID = Id });

        }
    }
}
