using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class User
    {
        public ObjectId Id { get; set; }
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Pregunta de seguridad")]
        public string SecurityQuestion { get; set; }
        [Display(Name = "Respuesta de seguridad")]
        public string SecurityAnswer { get; set; }
        [Display(Name = "Rol")]
        public Role Role { get; set; }

        public class Identity
        {
        }
    }
    public enum Role
    {
        [Display(Name = "Secret.")]
        Secretary,

        [Display(Name = "Doc.")]
        Doctor,

        [Display(Name = "Admin.")]
        Admin

    }
}
