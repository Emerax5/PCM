using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Prescription
    {
        public ObjectId Id { get; set; }
        public string PatientId { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
    }
}
