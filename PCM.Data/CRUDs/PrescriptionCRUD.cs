using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class PrescriptionCRUD
    {
        private IMongoDatabase db;

        public PrescriptionCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);


        }

        public void InsertPrescription<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public void DeletePrescriptionById<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            collection.FindOneAndDelete(filter);

        }
        public List<T> GetAllPrescriptionsByPatientId<T>(string table, string PatientId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PatientId", PatientId);


            return collection.Find(filter).ToList();

        }
        public T LoadPrescriptionById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            return collection.Find(filter).First();

        }

    }
}
