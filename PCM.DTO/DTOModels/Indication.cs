using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Indication
    {
        public ObjectId Id { get; set; }
        public string Medicine { get; set; }
        public string Details { get; set; }
        public string PrescriptionId { get; set; }
    }
}
