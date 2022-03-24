using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Payment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string PatientName { get; set; }
        public string ReceiptNumber { get; set; }
        public string AppointmentId { get; set; }
        public string Description { get; set; }
        public string PatientId { get; set; }
        public DateTime EmitedTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string EmitedBy { get; set; }
        public decimal InsuranceCoverage { get; set; }
        public decimal AppointmentPrice { get; set; }
        public decimal TotalPayment { get; set; }
        public Address PatientAddress { get; set; }
        public string Insurance { get; set; }
        public string PatientPhone { get; set; }
        public string InsuranceCardnet { get; set; }

    }
}
