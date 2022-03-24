using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Indication
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Medicine { get; set; }
        public string Details { get; set; }
        public string PrescriptionId { get; set; }
    }

}
