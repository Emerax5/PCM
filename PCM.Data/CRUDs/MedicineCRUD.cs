using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class MedicineCRUD
    {

        private IMongoDatabase db;

        public MedicineCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertMedicine<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public List<T> LoadAllMedicine<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();

        }
        public T LoadMedicineByName<T>(string table, string ComercialName)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("ComercialName", ComercialName);


            return collection.Find(filter).First();

        }

        public void DeleteMedicineById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            collection.DeleteOne(filter);

        }

    }
}
