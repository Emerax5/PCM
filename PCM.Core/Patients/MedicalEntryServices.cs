using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using PCM;
using PCM.Data.CRUDs;
using PCM.Data.Models;

namespace PCM.Core.Patients
{
    public class MedicalEntryServices
    {
        string Collection = "HistoryEntry";
        string appDataBase = "AppDataBase";
        public void AddEntry(DTO.DTOModels.HistoryEntry historyEntry) 
        {

            MedicalEntryCRUD db = new MedicalEntryCRUD(appDataBase);
            db.InsertEntry( Collection, historyEntry);                    
        
        }

        public List<DTO.DTOModels.HistoryEntry> GetEntriesByPatientId(string PatientId)
        {


            MedicalEntryCRUD db = new MedicalEntryCRUD(appDataBase);
            return db.GetEntriesByPatientId<DTO.DTOModels.HistoryEntry>(Collection, PatientId);

        }
        public void DeleteMedicaEntryById(ObjectId Id)         
        {

            MedicalEntryCRUD db = new MedicalEntryCRUD(appDataBase);

            db.DeleteEntryById<HistoryEntry>(Collection, Id);

        }

        public DTO.DTOModels.HistoryEntry GetEntryById(ObjectId Id) 
        {

            MedicalEntryCRUD db = new MedicalEntryCRUD(appDataBase);
            return db.LoadEntryById<DTO.DTOModels.HistoryEntry>(Collection, Id);

        }
        public void UpsertEntry(ObjectId id, DTO.DTOModels.HistoryEntry NewEntry)
        {

            MedicalEntryCRUD db = new MedicalEntryCRUD(appDataBase);
            db.UpsertEntryByID(Collection, id, NewEntry);

        }
    }
}
