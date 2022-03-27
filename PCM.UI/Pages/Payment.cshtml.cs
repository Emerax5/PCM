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
    public class PaymentModel : PageModel
    {
        AppointmentServices appointmentServices = new AppointmentServices();
        PatientServices patientServices = new PatientServices();
        PaymentServices paymentServices = new PaymentServices();
        LogServices logServices = new LogServices();


        public Appointment appointment { get; set; }

        public Patient patient { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            public string ConsultatioPrice { get; set; }
            public string InsurenceCover { get; set; }



        }

        public void OnGet(string Id)
        {

            appointment = appointmentServices.GetAppointmentsById(ObjectId.Parse(Id));

            patient = patientServices.GetPatienByID(ObjectId.Parse(appointment.PatientId));

            logServices.Log(string.Format("User {0} accessed payment page for appointment ID: {1}, patient ID: {2}", User.Identity.Name,Id,patient.Id));

        }

        public IActionResult OnPostPay(string Id) 
        {

            Payment newPayment = new Payment();

            appointment = appointmentServices.GetAppointmentsById(ObjectId.Parse(Id));

            patient = patientServices.GetPatienByID(ObjectId.Parse(appointment.PatientId));

            decimal apmtPrice = decimal.Parse(Input.ConsultatioPrice);
            decimal insCover = 0;

            if (patient.Insurance != "Privado")
            {
                insCover = decimal.Parse(Input.InsurenceCover);
            }


            newPayment.AppointmentDate = appointment.AppointmentDateStart;
            newPayment.AppointmentId = appointment.Id.ToString();
            newPayment.Description = appointment.AppoinmentReason.DisplayName();
            newPayment.AppointmentPrice = apmtPrice;
            newPayment.InsuranceCoverage = insCover;
            newPayment.PatientId = appointment.PatientId;
            newPayment.ReceiptNumber = paymentServices.GenerateInvoiceNumber();
            newPayment.TotalPayment = apmtPrice - insCover;
            newPayment.EmitedBy = User.Identity.Name;
            newPayment.EmitedTime = DateTime.Now;
            newPayment.PatientName = patient.FullName;
            newPayment.PatientAddress = patient.Address;
            newPayment.Insurance = patient.Insurance;
            newPayment.PatientPhone = patient.PhoneNumber;
            if (patient.Insurance != "Privado")
            {
                newPayment.InsuranceCardnet = patient.InsuranceId;
            }

            if (newPayment.TotalPayment > 0 && !paymentServices.IsAppointmentPaid(appointment.Id.ToString()))
            {
                paymentServices.AddPayment(newPayment);

            }

            logServices.Log(string.Format("User {0} processed payment for appointment ID: {1}, Patient ID: {2}  ", User.Identity.Name,Id,patient.Id));

            return RedirectToPage("./PaymentConfirmation", new { AppointmentId = appointment.Id });            

        }

    }
}
