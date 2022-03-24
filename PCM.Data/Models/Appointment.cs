using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class Appointment
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime AppointmentDateStart { get; set; }
        public string AppointmentHour { get; set; }
        public DateTime AppointmentTimeEnds { get; set; }
        public string AppointmentNotes { get; set; }
        public string PatientId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime TimeCreated { get; set; }
        public string EditedBy { get; set; }
        public DateTime LastEditedTime { get; set; }
        public Status Status { get; set; }    
        public AppoinmentReason AppoinmentReason { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

    public enum Status
    {
        Scheduled,
        Confirmed,
        Completed,
        Cancelled,
        Reschedule,
        NoShow

    }
    public enum AppoinmentReason
    {
        Consultation,
        Followup,
        Procedure

    }
    public enum PaymentStatus 
    {

        Unpaid,    
        Paid
    
    }

}
