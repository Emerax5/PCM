using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class HistoryEntry
    {
        [BsonId]        
        public ObjectId Id { get; set; }
        public string EntryText { get; set; }
        public string PatientId { get; set; }
        public MedicalEntryType EntryType { get; set; }
        public DateTime EntryDate{ get; set; }
        public string CreatedBy { get; set; }

    }

    public enum MedicalEntryType
    {

        Other,
        Symptoms,
        Pathologicalhistory,
        laboratory,
        ToxicHabits,
        Diagnosis        

    }
}
