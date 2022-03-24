using MongoDB.Bson;
using PCM.Data.CRUDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCM.Core.Patients
{
    public class ARSServices
    {
        string Collection = "ARSs";
        string appDataBase = "AppDataBase";

        public void AddARS(DTO.DTOModels.ARS NewARS)
        {

            ARSCRUD db = new ARSCRUD(appDataBase);
            db.InsertARS(Collection, NewARS);

        }
        public List<DTO.DTOModels.ARS> GetAllARS()
        {

            var db = new ARSCRUD(appDataBase);

            return db.LoadAllARS<DTO.DTOModels.ARS>(Collection).OrderByDescending(x => x.Name).ToList();

        }

        public void RemoveARSById(ObjectId Id)
        {

            var db = new ARSCRUD(appDataBase);
            db.DeleteARSById<DTO.DTOModels.ARS>(Collection,Id);

        }
    }
}
