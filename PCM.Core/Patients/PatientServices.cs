using MongoDB.Bson;
using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Core.Patients
{
    public class PatientServices
    {
        string Collection = "Patients";
        string appDataBase = "AppDataBase";
        public void AddPatient(Patient NewPatient)
        {



           
             var dbPatient = new Data.Models.Patient();
             dbPatient.Address = new Data.Models.Address();

             dbPatient.Name = NewPatient.Name;
             dbPatient.FullName = NewPatient.FullName;
             dbPatient.PhoneNumber = NewPatient.PhoneNumber;
             dbPatient.Alergies = NewPatient.Alergies;
             dbPatient.BirthDay = NewPatient.BirthDay;
             dbPatient.LastName = NewPatient.LastName;
             dbPatient.PersonalID = NewPatient.PersonalID;
             dbPatient.sex = (Data.Models.Sex)NewPatient.sex;
             dbPatient.BloodType = (Data.Models.BloodType)NewPatient.BloodType;
             dbPatient.civilStatus = (Data.Models.CivilStatus)NewPatient.civilStatus;
             dbPatient.Insurance = NewPatient.Insurance;
             dbPatient.InsertionDate = NewPatient.InsertionDate;
             dbPatient.Religion = NewPatient.Religion;
             dbPatient.CreatedBy = NewPatient.CreatedBy;
             dbPatient.Nationality = NewPatient.Nationality;
             dbPatient.ProfilePic = NewPatient.ProfilePic;
             dbPatient.InsuranceId = NewPatient.InsuranceId;
           
             dbPatient.Address = new Data.Models.Address();
           
             dbPatient.Address.Address1 = NewPatient.Address.Address1;
             dbPatient.Address.Address2 = NewPatient.Address.Address2;
             dbPatient.Address.City = NewPatient.Address.City;
             dbPatient.Address.Povidence = (Data.Models.Providence)NewPatient.Address.Povidence;
           
           
             PatientCRUD db = new PatientCRUD(appDataBase);
             db.InsertPatient(Collection, dbPatient);

          

        }

        public List<DTO.DTOModels.Patient> GetPatientsByFullName(string fullName)
        {
            var db = new PatientCRUD(appDataBase);

            var patientList = new List<DTO.DTOModels.Patient>();

            var dbListPatients = db.LoadPatientsByFullName<Patient>(Collection, fullName);

            foreach (var Patient in dbListPatients)
            {
                patientList.Add(new DTO.DTOModels.Patient { 
                    Name = Patient.Name, 
                    PhoneNumber = Patient.PhoneNumber, 
                    LastName = Patient.LastName, 
                    FullName = Patient.FullName, 
                    Id = Patient.Id, 
                    PersonalID = Patient.PersonalID,
                });
            }

            return patientList;
        }

        public Patient GetPatienByID(ObjectId id)
        {
            var db = new PatientCRUD(appDataBase);

            Patient dbPatient = db.LoadPatientByID<Patient>(Collection, id);

            return dbPatient;
        }

        public void ErasePatienByID(ObjectId id)
        {
            var db = new PatientCRUD(appDataBase);

            db.DeletePatientByID<Patient>(Collection, id);

        }

        public List<DTO.DTOModels.Patient> GetPatientsByPersonalID(string PersonalID)
        {
            var db = new PatientCRUD(appDataBase);


            var dbListPatientsByPersonalID = db.LoadPatientByPersonalID<Patient>(Collection, PersonalID);


            return dbListPatientsByPersonalID;
        }

        public List<DTO.DTOModels.Patient> GetAllPatients()
        {
            var db = new PatientCRUD(appDataBase);


            var dbListPatients = db.LoadAllPatients<Patient>(Collection);


            return dbListPatients;
        }

        public void UpsertPatient(ObjectId id, Patient NewPatient)
        {          

           PatientCRUD db = new PatientCRUD(appDataBase);
           db.UpsertPatientByID(Collection, id, NewPatient);

        }

        public void UpsertNotes(ObjectId id, string Notes, string user)
        {
            DateTime noteUpdatedate = DateTime.Now; ;

            PatientCRUD db = new PatientCRUD(appDataBase);
            db.UpsertNotesID<Patient>(Collection, id, Notes, noteUpdatedate, user);

        }
        public List<DTO.DTOModels.Patient> GetPatientsWithPaging(int pageSize, int CurrentPage)
        {
            int SkipPages = pageSize * (CurrentPage - 1);

            var db = new PatientCRUD(appDataBase);

            var dbListPatients = db.LoadAllPatientsWithPaging<Patient>(Collection, pageSize, SkipPages);

            var dtoPatients = new List<DTO.DTOModels.Patient>();

            foreach (var patient in dbListPatients)
            {
                dtoPatients.Add(new DTO.DTOModels.Patient { Id =
                    patient.Id,
                    Name = patient.Name,
                    PersonalID = patient.PersonalID,
                    PhoneNumber = patient.PhoneNumber,
                    LastName = patient.LastName, 
                    FullName = patient.FullName});

            }

            return dtoPatients;
        }

        public int GetPatientPagesCount(int pageSize)
        {
            var db = new PatientCRUD(appDataBase);

            var patientCount = db.CountPatients(Collection);

            decimal pages = (decimal)patientCount / (decimal)pageSize;

            return Convert.ToInt32(Math.Ceiling(pages)); ;
        }

        public int GetAllPatientsCount()
        {
            var db = new PatientCRUD(appDataBase);

            return db.CountPatients(Collection);

        }

        public void SavePatientDocument(byte[] file, string patientId, string FileName)
        {
            var db = new PatientCRUD(appDataBase);

            db.InsertPatientFile(file, patientId, FileName);
        }

        public List<PatientDocumentInfo> GetBasicDocumentInfoList(string patienId)
        {
            var db = new PatientCRUD(appDataBase);

            var DocList = new List<PatientDocumentInfo>();

            var dbList = db.GetBasicDocumentInfoList<PatientDocumentInfo>(patienId);

            foreach (var Doc in dbList)
            {
                DocList.Add(new PatientDocumentInfo {
                    DocumentName = Doc.DocumentName,
                    DocumentId = Doc.DocumentId
                });

            }

            return DocList;
        }

        public byte[] GetPatientFileById(ObjectId id)
        {
            var db = new PatientCRUD(appDataBase);

            return db.GetPatientFileByDocumentId(id);

        }

        public void DeletePatientFileById(ObjectId id)
        {
            var db = new PatientCRUD(appDataBase);

            db.DeletePatientFileByDocumentId(id);

        }
    }

}
