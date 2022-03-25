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
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Threading;
using PCM.Core.AdminTools;

namespace PCM.UI.Pages
{

    [Authorize]
    public class InsertPatientModel : PageModel
    {

        PatientServices patientServices = new PatientServices();
        ARSServices aRSServices = new ARSServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public InputModel Input { get; set; }

        public List<ARS> AllARS { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "Campo requerido")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            public string PhoneNumber { get; set; }
            public Address Address { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            public DateTime? BirthDay { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Campo requerido")]
            public string PersonalID { get; set; }
            public Sex sex { get; set; }
            public string Insurance { get; set; }
            public BloodType BloodType { get; set; }
            public CivilStatus civilStatus { get; set; }
            public string Religion { get; set; }
            public string Nationality { get; set; }
            public string InsuranceId { get; set; }

            [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile file { get; set; }

        }

        public void OnGet()
        {
            AllARS = aRSServices.GetAllARS();
        }

        public IActionResult OnPost()
        {
            AllARS = aRSServices.GetAllARS();

            if (ModelState.IsValid)
            {
                var dbPatient = new Patient();
                
                dbPatient.Address = new Address();

                byte[] fileBytes = null;

                var todayDate = DateTime.Now;

                //verify if there is a profile pic submited and turns it into byte
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


                //save personal data
                dbPatient.Name = Input.Name.Trim();
                dbPatient.LastName = Input.LastName.Trim();
                dbPatient.PersonalID = Input.PersonalID;
                dbPatient.Nationality = Input.Nationality;
                dbPatient.BloodType = Input.BloodType;

                if (Input.BirthDay > DateTime.Today)
                {
                    dbPatient.BirthDay = DateTime.Today;
                }
                else
                {
                    dbPatient.BirthDay = (DateTime)Input.BirthDay;

                }

                dbPatient.InsertionDate = todayDate;
                dbPatient.Religion = Input.Religion;
                dbPatient.sex = Input.sex;
                dbPatient.Insurance = Input.Insurance;
                dbPatient.InsuranceId = Input.InsuranceId;

                dbPatient.civilStatus = Input.civilStatus;
                dbPatient.FullName = string.Format("{0} {1}",Input.Name, Input.LastName);
                //save the addresss
                dbPatient.Address.Address1 = Input.Address.Address1;
                dbPatient.Address.Address2 = Input.Address.Address2;
                dbPatient.Address.City = Input.Address.City;
                dbPatient.Address.Povidence = Input.Address.Povidence;
                dbPatient.PhoneNumber = Input.PhoneNumber;
                //saving the Photo
                dbPatient.ProfilePic = fileBytes;

                dbPatient.CreatedBy = User.Identity.Name;
                

                patientServices.AddPatient(dbPatient);

                return RedirectToPage("./InsertPatienConfirmation");
                
            }

            return Page();
        
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
