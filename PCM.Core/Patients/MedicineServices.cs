using MongoDB.Bson;
using PCM.Data.CRUDs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCM.Core.Patients
{
    public class MedicineServices
    {
        string Collection = "Medicines";
        string appDataBase = "AppDataBase";

        public void AddMedicine(DTO.DTOModels.Medicine NewMedicine)
        {

            MedicineCRUD db = new MedicineCRUD(appDataBase);
            db.InsertMedicine(Collection, NewMedicine);

        }
        public List<DTO.DTOModels.Medicine> GetAllMedicines()
        {

            var db = new MedicineCRUD(appDataBase);
            return db.LoadAllMedicine<DTO.DTOModels.Medicine>(Collection).OrderByDescending(x => x.ComercialName).ToList();

        }
        public void RemoveMedicineById(ObjectId Id)
        {

            var db = new MedicineCRUD(appDataBase);
            db.DeleteMedicineById<DTO.DTOModels.ARS>(Collection, Id);

        }
    }
}
