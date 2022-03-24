using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class MedicalEntryCRUD
    {
        private IMongoDatabase db;

        public MedicalEntryCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertEntry<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }

        public void DeleteEntryById<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            collection.FindOneAndDelete(filter);

        }

        public List<T> GetEntriesByPatientId<T>(string table, string PatientId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PatientId", PatientId);


            return collection.Find(filter).ToList();

        }
        public T LoadEntryById<T>(string table, ObjectId id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();

        }
        public void UpsertEntryByID<T>(string table, ObjectId id, T Entry)
        {

            var collection = db.GetCollection<T>(table);
            collection.ReplaceOne(new BsonDocument("_id", id), Entry,

                new ReplaceOptions { IsUpsert = true });

        }
    }
}
