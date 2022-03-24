using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class ARS
    {

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
