using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class ARS
    {

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
