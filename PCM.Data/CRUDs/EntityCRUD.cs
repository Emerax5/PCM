using MongoDB.Bson;
using MongoDB.Driver;
using PCM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class EntityCRUD
    {

        private IMongoDatabase db;

        public EntityCRUD(string database)
        {


            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertEntity<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
    
        public T LoadEntityById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            return collection.Find(filter).First();

        }

        public void DeleteEntityById<T>(string table, ObjectId Id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            collection.DeleteOne(filter);

        }
        public int CheckForEntity(string table)
        {
            var collection = db.GetCollection<Entity>(table);

            return (int)collection.Find(new BsonDocument()).CountDocuments();

        }
        public T LoadEntityByIdentifier<T>(string table)
        {


            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Identifier", "Entity");

            return collection.Find(filter).First();

        }

    }
}
