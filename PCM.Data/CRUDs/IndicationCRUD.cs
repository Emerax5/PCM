using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class IndicationCRUD
    {
        private IMongoDatabase db;

        public IndicationCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertIndication<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public List<T> GetAllIndicationsByPrescriptionId<T>(string table, string PrescriptionId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PrescriptionId", PrescriptionId);


            return collection.Find(filter).ToList();

        }

        public void DeleteIndicationById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            collection.DeleteOne(filter);

        }
        public T LoadIndicationById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            return collection.Find(filter).First();

        }
    }
}
