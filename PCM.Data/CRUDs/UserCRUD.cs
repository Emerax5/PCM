using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class UserCRUD
    {
        private IMongoDatabase db;

        public UserCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertUser<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public List<T> LoadUsers<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();

        }
        public T LoadUserByUserName<T>(string table, string UserName)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("UserName", UserName);


            return collection.Find(filter).First();

        }

        public void DeleteByUserName<T>(string table, string UserName)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("UserName", UserName);


             collection.DeleteOne(filter);

        }

        public int GetUserCount<T>(string table) 
        {
            var collection = db.GetCollection<T>(table);

            return (int)collection.Find(new BsonDocument()).CountDocuments();

        }
    }
}
