using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using PCM.Data.CRUDs;

namespace PCM.Core.Patients
{
    public class PrescriptionServices
    {

        string Collection = "Prescriptions";
        string appDataBase = "AppDataBase";

        public void AddPrescription(DTO.DTOModels.Prescription NewPrescription)
        {

            PrescriptionCRUD db = new PrescriptionCRUD(appDataBase);
            db.InsertPrescription(Collection, NewPrescription);

        }
        public void RemovePrescriptionById(ObjectId Id)
        {

            var db = new PrescriptionCRUD(appDataBase);
            db.DeletePrescriptionById<DTO.DTOModels.Prescription>(Collection, Id);

        }
        public List<DTO.DTOModels.Prescription> GetAllPrescriptionsByPatientId(string PatientId)
        {


            PrescriptionCRUD db = new PrescriptionCRUD(appDataBase);
            return db.GetAllPrescriptionsByPatientId<DTO.DTOModels.Prescription>(Collection, PatientId);

        }
        public DTO.DTOModels.Prescription GetPrescriptionById(ObjectId Id)
        {


            PrescriptionCRUD db = new PrescriptionCRUD(appDataBase);
            return db.LoadPrescriptionById<DTO.DTOModels.Prescription>(Collection, Id);

        }
    }
}
