using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.Patients;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    [Authorize]
    public class InvoiceModel : PageModel
    {
        EntityServices entityServices = new EntityServices();
        PaymentServices paymentServices = new PaymentServices();
        LogServices logServices = new LogServices();

        CultureInfo es = new CultureInfo("es-ES");

        public Entity CurrentEntity { get; set; }
        public int EntityCount { get; set; }
        public Payment Payment { get; set; }
        public bool PayStatus { get; set; }
        public string PrintDate { get; set; }
        public string PaymentDate { get; set; }

        public string img { get; set; }

        public async Task OnGetAsync(string Id)
        {

            PrintDate = DateTime.Today.ToString("D", es);

            PayStatus = paymentServices.IsAppointmentPaid(Id);

            if (PayStatus == true)
            {
                Payment = paymentServices.GetPaymentByAppointmentId(Id);

                PaymentDate = Payment.EmitedTime.ToString("D", es);

                EntityCount = entityServices.CheckForEntity();

                if (EntityCount > 0)
                {
                    CurrentEntity = entityServices.FindEntityByIdentifier();
                    img = await getImg();
                }
            }

            logServices.Log(string.Format("User {0} accessd invoice id: {1}", User.Identity.Name,Payment.Id));


        }
        public async Task<string> getImg()
        {
            string photo = await Task.Run(() => Convert.ToBase64String(CurrentEntity.Logo));

            return photo;
        }
    }
}
