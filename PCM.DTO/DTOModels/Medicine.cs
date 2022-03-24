﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Medicine
    {
        public ObjectId Id { get; set; }
        public string ComercialName { get; set; }
        public string GenericName { get; set; }
        public string lab { get; set; }
        public string Description { get; set; }
        public string SideEffects { get; set; }
        public string AddedBy { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
