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
    public class PaymentConfirmationModel : PageModel
    {

        PaymentServices paymentServices = new PaymentServices();

        public Payment payment { get; set; }

        public void OnGet(string AppointmentId)
        {
            if (paymentServices.IsAppointmentPaid(AppointmentId))
            {
                payment = paymentServices.GetPaymentByAppointmentId(AppointmentId);

            }

        }
        public IActionResult OnPostDelete(string Id)
        {
            payment = paymentServices.LoadPaymentById(ObjectId.Parse(Id));

            payment.EmitedBy = User.Identity.Name;

            payment.EmitedTime = DateTime.Now;

            paymentServices.SaveCancelledPayment(payment);

            paymentServices.DeletePaymentById(ObjectId.Parse(Id));

            return RedirectToPage("./Appointment", new { Id = payment.AppointmentId });
        }
    }
}
