using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Patient
    {
        public ObjectId Id { get; set; }

        [Display(Name ="Nombre")]
        public string Name { get; set; }

        [Display(Name = "Telefono")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; }
        public Address Address { get; set; }

        [Display(Name = "Notas generales")]
        public string Alergies { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "ID")]
        public string PersonalID { get; set; }

        [Display(Name = "Sexo")]
        public Sex sex { get; set; }

        [Display(Name = "Tipo de sangre")]
        public BloodType BloodType { get; set; }

        [Display(Name = "Estado civil")]
        public CivilStatus civilStatus { get; set; }

        public DateTime InsertionDate { get; set; }

        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Display(Name = "Nacionalidad")]
        public string Nationality { get; set; }

        [Display(Name = "Creador del archivo")]
        public string CreatedBy { get; set; }
        public byte[] ProfilePic { get; set; }

        public string LastEditedBy { get; set; }
        public DateTime LastEditedTime { get; set; }
        public DateTime UpsertAlergieDate { get; set; }
        public string AlegieEditedBy { get; set; }
        public string Insurance { get; set; }
        public string InsuranceId { get; set; }

    }

    public static class EnumName
    {

        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }

    }

    public enum Sex
    {
        [Display(Name = "-No definido-")]
        Notdefined,

        [Display(Name = "Masculino")]
        M,

        [Display(Name = "Femenino")]
        F
    }
    public enum BloodType
    {
        [Display(Name = "-No definido-")]
        Notdefined,

        [Display(Name = "O negativo")]
        Oneg,

        [Display(Name = "O positivo")]
        Opos,

        [Display(Name = "A negativo")]
        Aneg,

        [Display(Name = "A positivo")]
        Apos,

        [Display(Name = "B negativo")]
        Bneg,

        [Display(Name = "B positivo")]
        Bpos,

        [Display(Name = "AB negativo")]
        ABneg,

        [Display(Name = "AB positivo")]
        ABPos
    }

    public enum CivilStatus
    {
        
        [Display(Name = "-No definido-")]
        Notdefined,

        [Display(Name = "Solter@")]
        Single,

        [Display(Name = "Casad@")]
        Married,

        [Display(Name = "Divorcia@")]
        Divorced
    }
    public enum Insurance
    {

        [Display(Name = "Privado")]
        Private,

        [Display(Name = "ARS BMI")]
        ARSBMI,

        [Display(Name = "Primera ARS Humano")]
        PrimeraARSHumano,

        [Display(Name = "ARS Palic Salud")]
        ARSPalicSalud,

        [Display(Name = "ARS La Colonial")]
        ARSLaColonial,

        [Display(Name = "ARS Monumental")]
        ARSMonumental,

        [Display(Name = "ARS Renacer")]
        ARSRenacer,

        [Display(Name = "ARS Simag")]
        ARSSimag,

        [Display(Name = "ARS Universal")]
        ARSUniversal,

        [Display(Name = "ARS Yunén")]
        ARSYunén,

        [Display(Name = "Seguro Nacional de Salud (SENASA)")]
        SeguroNacionaldeSaludSENASA,
        
        [Display(Name = "ARS Grupo Médico Asociado (ARS GMA)")]
        ARSGrupoMédicoAsociadoARSGMA,

        [Display(Name = "ARS Futuro")]
        ARSFuturo,

        [Display(Name = "Otro")]
        other
    }
}
