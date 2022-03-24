using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Patient
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string Alergies { get; set; }
        public DateTime BirthDay { get; set; }
        public string LastName { get; set; }
        public string PersonalID { get; set; }
        public Sex sex { get; set; }
        public BloodType BloodType { get; set; }
        public CivilStatus civilStatus { get; set; }
        public DateTime InsertionDate { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string CreatedBy { get; set; }
        public byte[] ProfilePic { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedTime { get; set; }
        public DateTime UpsertAlergieDate { get; set; }
        public string AlegieEditedBy { get; set; }
        public string Insurance { get; set; }
        public string InsuranceId { get; set; }

    }

    public enum Sex
    {
        Notdefined,
        M,
        F
    }
    public enum BloodType
    {
        Notdefined,
        Oneg,
        Opos,
        Aneg,
        Apos,
        Bneg,
        Bpos,
        ABneg,
        ABPos
    }

    public enum CivilStatus
    {
        Notdefined,
        Single,
        Married,
        Divorced
    }

    public enum Insurance
    {


        Private,
        ARSBMI,
        PrimeraARSHumano,
        ARSPalicSalud,
        ARSLaColonial,
        ARSMonumental,
        ARSRenacer,
        ARSSimag,
        ARSUniversal,
        ARSYunén,
        SeguroNacionaldeSaludSENASA,
        ARSGrupoMédicoAsociadoARSGMA,
        ARSFuturo,
        other
    }
}
