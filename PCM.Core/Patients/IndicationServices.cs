using MongoDB.Bson;
using PCM.Data.CRUDs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Core.Patients
{
    public class IndicationServices
    {
        string Collection = "Indications";
        string appDataBase = "AppDataBase";

        public void AddIndication(DTO.DTOModels.Indication NewIndication)
        {

            IndicationCRUD db = new IndicationCRUD(appDataBase);
            db.InsertIndication(Collection, NewIndication);

        }
  
        public void RemoveIndicationById(ObjectId Id)
        {

            var db = new IndicationCRUD(appDataBase);
            db.DeleteIndicationById<DTO.DTOModels.Indication>(Collection, Id);

        }
        public List<DTO.DTOModels.Indication> GetIndicationsByPrescriptionId(string PrescriptionId)
        {


            IndicationCRUD db = new IndicationCRUD(appDataBase);
            return db.GetAllIndicationsByPrescriptionId<DTO.DTOModels.Indication>(Collection, PrescriptionId);

        }
        public DTO.DTOModels.Indication GetIndicationById(ObjectId Id)
        {


            IndicationCRUD db = new IndicationCRUD(appDataBase);
            return db.LoadIndicationById<DTO.DTOModels.Indication>(Collection, Id);

        }
    }
}
