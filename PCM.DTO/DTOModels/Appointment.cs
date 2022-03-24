using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace PCM.DTO.DTOModels
{
    public class Appointment
    {

        [BsonId]
        public ObjectId Id { get; set; }
        [Display(Name = "Fecha")]
        public DateTime AppointmentDateStart { get; set; }

        [Display(Name = "Hora")]
        public string AppointmentHour { get; set; }

        [Display(Name = "Fecha terminada")]
        public DateTime AppointmentTimeEnds { get; set; }

        [Display(Name = "Notas de cita")]
        public string AppointmentNotes { get; set; }

        [Display(Name = "Id interno Paciente")]
        public string PatientId { get; set; }

        [Display(Name = "Programado por")]
        public string CreatedBy { get; set; }

        [Display(Name = "Fecha de incercion")]
        public DateTime TimeCreated { get; set; }

        [Display(Name = "Editado por")]
        public string EditedBy { get; set; }

        [Display(Name = "Untima edicion")]
        public DateTime LastEditedTime { get; set; }

        [Display(Name = "Estatus")]
        public Status Status { get; set; }     
        public AppoinmentReason AppoinmentReason { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

    }

    public enum Status
    {

        [Display(Name = "Programada")]
        Scheduled,

        [Display(Name = "Confirmada")]
        Confirmed,

        [Display(Name = "Completada")]
        Completed,

        [Display(Name = "Cancelada")]
        Cancelled,

        [Display(Name = "Re-Programada")]
        Reschedule,      

        [Display(Name = "Perdida")]
        NoShow

    }

    public enum AppoinmentReason
    {
        [Display(Name = "Consuta")]
        Consultation,

        [Display(Name = "Seguimiento/Entrega")]
        Followup,

        [Display(Name = "Procedimiento")]
        Procedure
    }

    public enum PaymentStatus
    {

        Unpaid,
        Paid

    }



}
