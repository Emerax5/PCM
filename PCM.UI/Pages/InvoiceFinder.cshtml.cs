using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.AdminTools;
using PCM.Core.Patients;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    public class InvoiceFinderModel : PageModel
    {
        PaymentServices paymentServices = new PaymentServices();
        LogServices logServices = new LogServices();

        [BindProperty]
        public string SearchInput { get; set; }
        public List<Payment> Result { get; set; }

        public void OnGet()
        {
            Result = new List<Payment>();
            logServices.Log(string.Format("User {0} accessed the invoice finder", User.Identity.Name));


        }
        public IActionResult OnPost() 
        {
            Result = new List<Payment>();

            Result = paymentServices.GetPaymentsByInvoiceNumber(SearchInput);

            if (Result.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No hay resultados");
                logServices.Log(string.Format("User {0} searched invoice# {1} with no result", User.Identity.Name,SearchInput));
                return Page();

            }

            logServices.Log(string.Format("User {0} searched invoice number# {1}", User.Identity.Name,SearchInput));


            return null;
        
        }
    }
}
