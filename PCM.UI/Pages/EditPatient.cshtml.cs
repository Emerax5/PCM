using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCM.Core.Patients;
using PCM.DTO.DTOModels;
using ServiceStack;
using PCM.Core.CommunSevices;
using static PCM.Core.CommunSevices.CommunServices;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{
    [Authorize]
    public class EditPatientModel : PageModel
    {

        PatientServices patientServices = new PatientServices();
        ARSServices aRSServices = new ARSServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public Patient patient { get; set; }

        [BindProperty]
        public Address Address { get; set; }

        public List<ARS> AllARS { get; set; }

        public class InputModel
        {


            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public Address Address { get; set; }
            public string GeneralNotes { get; set; }
            public DateTime BirthDay { get; set; }
            public string LastName { get; set; }
            public string PersonalID { get; set; }
            public Sex sex { get; set; }
            public BloodType BloodType { get; set; }
            public CivilStatus civilStatus { get; set; }
            public string Religion { get; set; }
            public string Nationality { get; set; }

            [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile file { get; set; }
            public string Insurance { get; set; }
            public string InsuranceId { get; set; }
        }

        public void OnGet(string ID)

        {
            AllARS = aRSServices.GetAllARS();

            ObjectId patientId = ObjectId.Parse(ID);

            patient = patientServices.GetPatienByID(patientId);

            logServices.Log(string.Format("User {0} accessed edit page for patien Id: {1}", User.Identity.Name,ID));


        }

        public IActionResult OnPost(string id)
        {

            AllARS = aRSServices.GetAllARS();

            ObjectId patienId = ObjectId.Parse(id);

            patient = patientServices.GetPatienByID(patienId);

            var dbPatient = new Patient();
            dbPatient.Address = new Address();
            var todayDate = DateTime.Now;

            byte[] fileBytes = null;

            if (!(Input.file == null))
            {

                Image image = Image.FromStream(Input.file.OpenReadStream(), true, true);
                int imageWidth = image.Width;
                int imageHeight = image.Height;
                Bitmap newImage;
                if (imageWidth > 1500 || imageHeight > 1500)
                {
                    newImage = new Bitmap(imageWidth / 6, imageHeight / 6);
                    using (var g = Graphics.FromImage(newImage))
                    {
                        g.DrawImage(image, 0, 0, imageWidth / 6, imageHeight / 6);
                    }
                }
                else
                {
                    newImage = new Bitmap(imageWidth, imageHeight);
                    using (var g = Graphics.FromImage(newImage))
                    {
                        g.DrawImage(image, 0, 0, imageWidth, imageHeight);
                    }
                }

                fileBytes = ImageToByte2(newImage);
            }




            //Edit personal data
            if (Input.Name == null || Input.Name.Trim().Count() == 0) dbPatient.Name = patient.Name; else dbPatient.Name = Input.Name;
            if (Input.LastName == null || Input.LastName.Trim().Count() == 0) dbPatient.LastName = patient.LastName; else dbPatient.LastName = Input.LastName;
            if (Input.PersonalID == null || Input.PersonalID.Trim().Count() == 0) dbPatient.PersonalID = patient.PersonalID; else dbPatient.PersonalID = Input.PersonalID;
            if (Input.Nationality == null || Input.Nationality.Trim().Count() == 0) dbPatient.Nationality = patient.Nationality; else dbPatient.Nationality = Input.Nationality;
            if (Input.InsuranceId == null || Input.InsuranceId.Trim().Count() == 0) dbPatient.InsuranceId = patient.InsuranceId; else dbPatient.InsuranceId = Input.InsuranceId;
            if (Input.Insurance == null || Input.Insurance.Trim().Count() == 0) dbPatient.Insurance = patient.Insurance; else dbPatient.Insurance = Input.Insurance;

            dbPatient.BloodType = Input.BloodType;
            if (Input.BirthDay == default) dbPatient.BirthDay = patient.BirthDay; else dbPatient.BirthDay = Input.BirthDay;
            dbPatient.Religion = Input.Religion;
            dbPatient.sex = Input.sex;
            dbPatient.civilStatus = Input.civilStatus;
            dbPatient.InsuranceId = Input.InsuranceId;

            //save the addresss
            if (Input.Address.Address1 == null || Input.Address.Address1.Trim().Count() == 0) dbPatient.Address.Address1 = patient.Address.Address1; else dbPatient.Address.Address1 = Input.Address.Address1;
            if (Input.Address.Address2 == null || Input.Address.Address2.Trim().Count() == 0) dbPatient.Address.Address2 = patient.Address.Address2; else dbPatient.Address.Address2 = Input.Address.Address2;
            if (Input.Address.City == null || Input.Address.City.Trim().Count() == 0) dbPatient.Address.City = patient.Address.City; else dbPatient.Address.City = Input.Address.City;
            dbPatient.Address.Povidence = Input.Address.Povidence;
            if (Input.PhoneNumber == null || Input.PhoneNumber.Trim().Count() == 0) dbPatient.PhoneNumber = patient.PhoneNumber; else dbPatient.PhoneNumber = Input.PhoneNumber;

            //edit the Photo
            if (Input.file == null) dbPatient.ProfilePic = patient.ProfilePic; else dbPatient.ProfilePic = fileBytes;
            //Set permanet fields
            dbPatient.CreatedBy = patient.CreatedBy;
            dbPatient.InsertionDate = patient.InsertionDate;
            dbPatient.Alergies = patient.Alergies;
            dbPatient.Id = patient.Id;
            dbPatient.AlegieEditedBy = patient.AlegieEditedBy;
            dbPatient.UpsertAlergieDate = patient.UpsertAlergieDate;

            dbPatient.FullName = string.Format("{0} {1}", dbPatient.Name, dbPatient.LastName);

            dbPatient.LastEditedBy = User.Identity.Name;
            dbPatient.LastEditedTime = todayDate;

            patientServices.UpsertPatient(patient.Id, dbPatient);

            logServices.Log(string.Format("User {0} updated patient id: {1}", User.Identity.Name, id));

            return RedirectToPage("./PatientProfile", new { ID = id });

        }
        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }



    }
}
