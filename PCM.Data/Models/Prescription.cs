using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Prescription
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string PatientId { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }

    }
}
