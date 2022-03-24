using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class PatientDocumentInfo
    {
        public string DocumentName { get; set; }
        public ObjectId DocumentId { get; set; }
    }
}
