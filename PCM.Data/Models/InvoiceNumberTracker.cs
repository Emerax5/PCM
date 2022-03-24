using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class InvoiceNumberTracker
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime LastInvoiceDate { get; set; }

        public string Identifier = "Tracker";
    }
}
