using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class PatientDocumentInfo
    {
        public string DocumentName { get; set; }
        public ObjectId DocumentId { get; set; }
    }
}
