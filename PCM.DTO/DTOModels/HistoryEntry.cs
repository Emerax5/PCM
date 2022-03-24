using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class HistoryEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EntryText { get; set; }
        public string PatientId { get; set; }
        public MedicalEntryType EntryType { get; set; }
        public DateTime EntryDate { get; set; }
        public string CreatedBy { get; set; }

    }

    public enum MedicalEntryType
    {
        [Display(Name = "Otro")]
        Other,
        
        [Display(Name = "Signos y síntomas")]
        Symptoms,

        [Display(Name = "Antecedente patológico")]
        Pathologicalhistory,

        [Display(Name = "Imágenes / Laboratorio")]
        laboratory,
     
        [Display(Name = "Habitos toxicos")]
        ToxicHabits,

        [Display(Name = "Diagnostico")]
        Diagnosis,

    }
}
