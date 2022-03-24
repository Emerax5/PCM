using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class ARSCRUD
    {
        private IMongoDatabase db;

        public ARSCRUD(string database)
        {


            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertARS<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public List<T> LoadAllARS<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();

        }
        public T LoadARSByName<T>(string table, string Name)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Name", Name);


            return collection.Find(filter).First();

        }

        public void DeleteARSById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            collection.DeleteOne(filter);

        }

    }
}
