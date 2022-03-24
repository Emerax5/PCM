using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Entity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EntityName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string DoctorName { get; set; }
        public byte[] Logo{ get; set; }
        public DateTime AddedTime { get; set; }
        public string AddedBy { get; set; }
        public string Identifier { get; set; }
        public string Specialty { get; set; }
        public string Exequatur { get; set; }
    }
}
