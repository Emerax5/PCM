using MongoDB.Bson;
using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCM.Core.Patients
{
    public class AppointmentServices
    {
        string Collection = "Appointments";
        string appDataBase = "AppDataBase";


        public void AddAppointment(Appointment NewAppointment)
        {
            
            AppointmentCRUD db = new AppointmentCRUD(appDataBase);
            db.InsertAppointment(Collection, NewAppointment);

        }

        public List<Appointment> GetAppointmentsByDate(DateTime day)
        {


            AppointmentCRUD db = new AppointmentCRUD(appDataBase);


            return db.LoadAppointmentsByDate<Appointment>(Collection, day);

        }

        public List<Appointment> GetAppointmentsByPatientId(string PatientId)
        {


            AppointmentCRUD db = new AppointmentCRUD(appDataBase);


            return db.LoadAppointmentsByPatientId<Appointment>(Collection, PatientId).OrderByDescending(x => x.AppointmentDateStart).ToList();

        }

        public void DeleteAppointmentsById(ObjectId Id)
        {


            AppointmentCRUD db = new AppointmentCRUD(appDataBase);


            db.DeleteAppointmentById<Appointment>(Collection, Id);
        }

        public Appointment GetAppointmentsById(ObjectId Id)
        {

            AppointmentCRUD db = new AppointmentCRUD(appDataBase);

            Appointment apmt = db.LoadAppointmentById<Appointment>(Collection, Id);

            return apmt;
        }

        public void UpsertNote(ObjectId Id, string Note, string User)
        {

            AppointmentCRUD db = new AppointmentCRUD(appDataBase);

            db.UpsertNotesID<Appointment>(Collection, Id, Note, DateTime.Now, User);

        }

        public void Reschedule(ObjectId Id, string hour, DateTime Date, string User)
        {

            int status;
            if (Date >= DateTime.Today.ToLocalTime())
            {
                status = (int)Status.Reschedule;
            }
            else
            {
                status = (int)Status.Completed;
            }
            AppointmentCRUD db = new AppointmentCRUD(appDataBase);

            db.Reschedule<Appointment>(Collection, Id, hour, Date, DateTime.Now, User, status);

        }

        public void ChangeStatus(ObjectId Id, int Status, string hour)
        {

            AppointmentCRUD db = new AppointmentCRUD(appDataBase);

            db.ChangeStatus<Appointment>(Collection, Id, Status, hour);

        }

        public string[] ValidHours = {
            "09:30.AM",
            "09:45.AM",
            "10:00.AM",
            "10:15.AM",
            "10:30.AM",
            "10:45.AM",
            "11:00.AM",
            "11:15.AM",
            "11:30.AM",
            "11:45.AM",
            "12:00.PM",
            "12:15.PM",
            "12:30.PM",
            "12:45.PM",
            "01:00.PM",
            "01:15.PM",
            "01:30.PM",
            "01:45.PM",
            "02:00.PM",
            "02:15.PM",
            "02:30.PM",
            "02:45.PM"};
    }
}
