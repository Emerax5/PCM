using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using PCM.Core.AdminTools;
using PCM.Core.Users;
using PCM.DTO.DTOModels;

namespace PCM.UI.Pages
{
    public class ManageEntitiesModel : PageModel
    {
        UserServices userServices = new UserServices();
        EntityServices entityServices = new EntityServices();
        LogServices logServices = new LogServices();


        [BindProperty]
        public InputModel Input { get; set; }
        public int EntityCount { get; set; }
        public Entity CurrentEntity { get; set; }
        public string img { get; set; }

        public class InputModel
        {
            public string EntityName { get; set; }
            public string PhoneNumber { get; set; }
            public string DoctorName { get; set; }
            public IFormFile Logo { get; set; }
            public string Location { get; set; }
            public string Specialty { get; set; }
            public string Exequatur { get; set; }

        }
        public async Task<IActionResult> OnGetAsync()
        {

            if (userServices.GetRoleByUserName(User.Identity.Name) == Role.Admin)
            {
                EntityCount = entityServices.CheckForEntity();

                if (EntityCount > 0 )
                {
                    CurrentEntity = entityServices.FindEntityByIdentifier();
                    img = await getImg();
                }
                
               
                return Page();

            }
            else
            {
                return RedirectToPage("./NotAllowed");

            }
        }

        public IActionResult OnPost() 
        {


            if (ModelState.IsValid)
            {
                byte[] fileBytes = null;
                var newEntity = new Entity();

                if (!(Input.Logo == null))
                {

                    Image image = Image.FromStream(Input.Logo.OpenReadStream(), true, true);
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

                newEntity.AddedBy = User.Identity.Name;
                newEntity.AddedTime = DateTime.Now;
                newEntity.DoctorName = Input.DoctorName.Trim();
                newEntity.EntityName = Input.EntityName.Trim();
                newEntity.PhoneNumber = Input.PhoneNumber;
                newEntity.Location = Input.Location.Trim();
                newEntity.Logo = fileBytes;
                newEntity.Identifier = "Entity";
                newEntity.Exequatur = Input.Exequatur;
                newEntity.Specialty = Input.Specialty;

                entityServices.AddEntity(newEntity);
            }
            return RedirectToPage("./ManageEntities");
        }

        public IActionResult OnPostDelete(string Id) 
        {
            entityServices.DeleteEntityById(ObjectId.Parse(Id));

            return RedirectToPage("./ManageEntities");

        }

        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public async Task<string> getImg()
        {
            string photo = await Task.Run(() => Convert.ToBase64String(CurrentEntity.Logo));

            return photo;
        }
    }
}
