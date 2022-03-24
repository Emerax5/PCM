using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using PCM.Data.CRUDs;

namespace PCM.Data.CRUDs
{
    public class AppointmentCRUD
    {

        private IMongoDatabase db;

        public AppointmentCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertAppointment<T>(string Table, T appointment)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(appointment);

        }
        public List<T> LoadAppointments<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();

        }
        public T LoadAppointmentById<T>(string table, ObjectId Id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", Id);


            return collection.Find(filter).First();
        }
        public List<T> LoadAppointmentsByPatientId<T>(string table, string PatientId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PatientId", PatientId);


            return collection.Find(filter).ToList();
        }
        public void DeleteAppointmentById<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);


            collection.DeleteOne(filter);
        }
        public List<T> LoadAppointmentsByDate<T>(string table, DateTime day)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("AppointmentDateStart", day);


            return collection.Find(filter).ToList();
        }
        public void UpsertNotesID<T>(string table, ObjectId id, string Note, DateTime NoteUpserDate, string User)
        {
            var collection = db.GetCollection<BsonDocument>(table);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var update = Builders<BsonDocument>.Update.Set("AppointmentNotes", Note);

            var UpdatedBy= Builders<BsonDocument>.Update.Set("EditedBy", User);

            var UpdateDate= Builders<BsonDocument>.Update.Set("LastEditedTime", NoteUpserDate);


            collection.UpdateOne(filter, update);
            collection.UpdateOne(filter, UpdateDate);
            collection.UpdateOne(filter, UpdatedBy);
        }

        public void Reschedule<T>(string table, ObjectId id, string Hour, DateTime Date, DateTime UpdateDate, string User, int Status)
        {
            var collection = db.GetCollection<BsonDocument>(table);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var hour = Builders<BsonDocument>.Update.Set("AppointmentHour", Hour);

            var date = Builders<BsonDocument>.Update.Set("AppointmentDateStart", Date);

            var user = Builders<BsonDocument>.Update.Set("EditedBy", User);

            var updatedate = Builders<BsonDocument>.Update.Set("LastEditedTime", UpdateDate);

            var status = Builders<BsonDocument>.Update.Set("Status", Status);



            collection.UpdateOne(filter, hour);
            collection.UpdateOne(filter, date);
            collection.UpdateOne(filter, user);
            collection.UpdateOne(filter, updatedate);
            collection.UpdateOne(filter, status);
        }

        public void ChangeStatus<T>(string table, ObjectId id, int Status, string Hour)
        {

            var collection = db.GetCollection<BsonDocument>(table);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);      

            var status = Builders<BsonDocument>.Update.Set("Status", Status);

            var hour = Builders<BsonDocument>.Update.Set("AppointmentHour", Hour);


            collection.UpdateOne(filter, status);
            collection.UpdateOne(filter, hour);
        }
    }
}
