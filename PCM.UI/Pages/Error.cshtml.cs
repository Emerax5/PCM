using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public class ErrorModel : PageModel
    {

        LogServices logServices = new LogServices();

        public string ErrorInfo { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            try
            {

                logServices.Log(string.Format("General Error IP: {0} |", logServices.GetLocalIPAddress()),Activity.Current.OperationName);

            }
            catch (Exception e)
            {

                logServices.OfflineLog(string.Format("General Error IP: {0}", logServices.GetLocalIPAddress()),e.Message);

            }


        }
    }
}
