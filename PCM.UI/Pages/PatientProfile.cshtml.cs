using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.Patients;
using PCM.DTO.DTOModels;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using static PCM.Core.CommunSevices.CommunServices;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using PCM.Core.Users;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{
    [Authorize]
    public class PatientProfileModel : PageModel
    {
        PatientServices patientServices = new PatientServices();
        AppointmentServices appointmentServices = new AppointmentServices();
        UserServices userServices = new UserServices();
        PrescriptionServices prescriptionServices = new PrescriptionServices();
        IndicationServices IndicationServices = new IndicationServices();
        LogServices logServices = new LogServices();


        public List<PatientDocumentInfo> DocInfoList { get; set; } = new List<PatientDocumentInfo>();
        public List<Appointment> ApmtList { get; set; } = new List<Appointment>();

        [BindProperty]
        public Patient patient { get; set; }

        [BindProperty]
        public Address Address { get; set; }

        [BindProperty]
        public int Age { get; set; }
        [BindProperty]
        public string AlegiesInProfile { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public string img { get; set; }

        public Role UserRole { get; set; }
        public class InputModel
        { 
        public IFormFile file { get; set; }
        
        }    


        public async Task OnGetAsync(string Id)
        {
         
            ObjectId patientId = ObjectId.Parse(Id);

            UserRole = userServices.GetRoleByUserName(User.Identity.Name);

            if (!(Id == null))
            {
                try
                {
                    patient = patientServices.GetPatienByID(patientId);

                }
                catch (Exception ex)
                {

                    throw ex;
                }


                AlegiesInProfile = patient.Alergies;

                DateTime zeroTime = new DateTime(1, 1, 1);

                DateTime a = patient.BirthDay;

                DateTime b = DateTime.Today.ToLocalTime();

                TimeSpan span = b - a;

                if (a.Date != DateTime.Today)
                {
                    Age = (zeroTime + span).Year - 1;

                }
                else
                {
                    Age = 0;
                }

                if (!(patient.Alergies == null))
                {
                    AlegiesInProfile = patient.Alergies;
                }

                DocInfoList = patientServices.GetBasicDocumentInfoList(Id);

                var apmtList = appointmentServices.GetAppointmentsByPatientId(Id);

                //change status to no show if needed
                foreach (var Apmt in apmtList)
                {
                    if ((Apmt.Status != Status.Cancelled && Apmt.Status != Status.Completed) && Apmt.AppointmentDateStart < DateTime.Today.ToLocalTime())
                    {
                        appointmentServices.ChangeStatus(Apmt.Id, (int)Status.NoShow, Apmt.AppointmentHour);
                    }

                }

                ApmtList = appointmentServices.GetAppointmentsByPatientId(Id);

                if (patient.ProfilePic != null)
                {

                    img = await getImg();

                }


            }

            logServices.Log(string.Format("User {0} accessed patient ID: {1}", User.Identity.Name,Id));

        }

        public IActionResult OnPostDelete(string id)
        {
            ObjectId patienId = ObjectId.Parse(id);

            if (!(patient == null))
            {
                patientServices.ErasePatienByID(patienId);

                //Delete all files for curent patient.
                DocInfoList = patientServices.GetBasicDocumentInfoList(id);

                if (DocInfoList.Count > 0)
                {
                    foreach (var Doc in DocInfoList)
                    {
                        patientServices.DeletePatientFileById(Doc.DocumentId);
                    }
                }

                //Delete all apointments for curent patient. 
                ApmtList = appointmentServices.GetAppointmentsByPatientId(id);

                if (ApmtList.Count > 0)
                {
                    foreach (var Apmt in ApmtList)
                    {
                        appointmentServices.DeleteAppointmentsById(Apmt.Id);
                    }
                }
                //Deletes all prescriptions and indications for current patient 
                var prescList = prescriptionServices.GetAllPrescriptionsByPatientId(id);
                foreach (var pres in prescList)
                {
                    var indicList = IndicationServices.GetIndicationsByPrescriptionId(pres.Id.ToString());

                    foreach (var indc in indicList)
                    {
                        IndicationServices.RemoveIndicationById(indc.Id);
                    }

                    prescriptionServices.RemovePrescriptionById(pres.Id);

                }


            }

            logServices.Log(string.Format("User {0} deleted patient", User.Identity.Name));

            return null;

        }

        public IActionResult OnPostUpdateNote(string id) 
        {
            ObjectId patienId = ObjectId.Parse(id);
            
            patientServices.UpsertNotes(patienId, AlegiesInProfile, User.Identity.Name);

            logServices.Log(string.Format("User {0} updated note for patien ID: {1}", User.Identity.Name,id));

            return RedirectToPage("./PatientProfile", new { ID = id });
        }

        public async Task<IActionResult> OnPostSaveFileAsync(string id)
        {

            if (ModelState.IsValid)
            {

                var tempPath = Path.GetTempPath() + Input.file.FileName;

                using (var streamtemp = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    await Input.file.CopyToAsync(streamtemp);
                }


                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(tempPath);


                patientServices.SavePatientDocument(fileBytes, id,Input.file.FileName);

                logServices.Log(string.Format("User {0} added file for patient ID: {1}", User.Identity.Name,id));

                return RedirectToPage("./PatientProfile", new { ID = id });
            }
            else
            {
                logServices.Log(string.Format("User {0} attempt to add file failed for patient ID: {1}", User.Identity.Name,id));

                ModelState.AddModelError(string.Empty, "Verifique e intente de nuevo.");
                return Page();

            }

        }

        public IActionResult OnPostShowFile(string DocId, string FileName)
        {
            ObjectId DocumentoId = ObjectId.Parse(DocId);

            byte[] bytes = patientServices.GetPatientFileById(DocumentoId);

            new FileExtensionContentTypeProvider().TryGetContentType(FileName, out string contentType);

            if (contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                logServices.Log(string.Format("User {0} accessed document ID: {1}", User.Identity.Name, DocId));
                return File(bytes, contentType, FileName);

            }
            else
            {
                logServices.Log(string.Format("User {0} accessed document ID: {1}", User.Identity.Name, DocId));
                return File(bytes, contentType);
            }


        }

        public IActionResult OnPostDeleteFile(string DocId,string id) 
        {
            ObjectId DocumentoId = ObjectId.Parse(DocId);

            patientServices.DeletePatientFileById(DocumentoId);

            logServices.Log(string.Format("User {0} deleted file ID: {1} for patient ID: {2}", User.Identity.Name, DocId,id));

            return RedirectToPage("./PatientProfile", new { ID = id });

        }

        public IActionResult OnPostDeleteAppointment(string ApmtId, string Id)
        {
            ObjectId id = ObjectId.Parse(ApmtId);

            appointmentServices.DeleteAppointmentsById(id);

            logServices.Log(string.Format("User {0} deleted appointment ID: {1} for patient ID: {2}", User.Identity.Name, ApmtId,Id));

            return RedirectToPage("./PatientProfile", new { ID = Id });

        }

        public async Task<string> getImg()         
        {
           string photo = await Task.Run(() => Convert.ToBase64String(patient.ProfilePic));

            return photo;
        }
    }
}
