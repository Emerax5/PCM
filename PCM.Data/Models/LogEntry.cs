using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class LogEntry
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public Type Type { get; set; }
        public DateTime Time { get; set; }
    }
    public enum Type
    {
        info,
        error
    }
}
