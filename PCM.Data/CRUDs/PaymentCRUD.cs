using MongoDB.Bson;
using MongoDB.Driver;
using PCM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PCM.Data.CRUDs
{
   public class PaymentCRUD
    {
        private IMongoDatabase db;

        public PaymentCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertPayment<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }

        public void DeletePaymentById<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            collection.FindOneAndDelete(filter);

        }

        public List<T> GetPaymentsByPatientId<T>(string table, string PatientId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PatientId", PatientId);


            return collection.Find(filter).ToList();

        }
        public T LoadPaymentById<T>(string table, ObjectId id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();

        }
        public T LoadPaymentByAppointmentId<T>(string table, string id)
        {

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("AppointmentId", id);

            return collection.Find(filter).First();

        }
        public void UpsertPaymentByID<T>(string table, ObjectId id, T Entry)
        {

            var collection = db.GetCollection<T>(table);
            collection.ReplaceOne(new BsonDocument("_id", id), Entry,

                new ReplaceOptions { IsUpsert = true });

        }
        public void UpsertInvoiceTracker<T>(string table, InvoiceNumberTracker InvoiceTracker )
        {

            
            var collection = db.GetCollection<BsonDocument>(table);

            if ((int)collection.Find(new BsonDocument()).CountDocuments() == 0)
            {

                collection.InsertOne(InvoiceTracker.ToBsonDocument());

            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Identifier", "Tracker");

                var UpdateDate = Builders<BsonDocument>.Update.Set("LastInvoiceDate", InvoiceTracker.LastInvoiceDate);

                var Update = Builders<BsonDocument>.Update.Set("InvoiceNumber", InvoiceTracker.InvoiceNumber);

                collection.UpdateOne(filter, Update);

                collection.UpdateOne(filter, UpdateDate);
            }
           

        }
        public T LoadInvoiceTracker<T>(string table)
        {


            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Identifier", "Tracker");

            return collection.Find(filter).First();

        }
        public int CheckForTracker(string table)
        {
            var collection = db.GetCollection<Payment>(table);

            return (int)collection.Find(new BsonDocument()).CountDocuments();

        }
        public bool IsAppointmentPaid(string table, string Id)         
        {

            var collection = db.GetCollection<Payment>(table);
            var filter = Builders<Payment>.Filter.Eq("AppointmentId", Id);
            


            if (collection.Find(filter).CountDocuments() <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public List<Payment> PaymentReport(DateTime MinDate, DateTime MaxDate, string Insurance,string table) 
        {

            var collection = db.GetCollection<Payment>(table);

            if (Insurance == "All" || Insurance == null)
            {
                var filter = Builders<Payment>.Filter.And(Builders<Payment>.Filter.Gte("EmitedDate", MinDate), Builders<Payment>.Filter.Lte("EmitedDate", MaxDate));

                return collection.Find(filter).ToList();

            }
            else
            {

                var filter = Builders<Payment>.Filter.And(
                    Builders<Payment>.Filter.Gte("EmitedDate", MinDate), 
                    Builders<Payment>.Filter.Lte("EmitedDate", MaxDate), 
                    Builders<Payment>.Filter.Eq("Insurance", Insurance));

                return collection.Find(filter).ToList();

            }

        
        }
        public List<Payment> LoadPaymentsByInvoiceNumber<T>(string table, string InvoiceNumber)
        {
            var collection = db.GetCollection<Payment>(table);

            var regexFilter = "^" + Regex.Escape(InvoiceNumber);
            var bsonRegex = new BsonRegularExpression(regexFilter);
            var filter = Builders<Payment>.Filter.Regex("ReceiptNumber", bsonRegex);
            return collection.Find(filter).SortBy(x => x.ReceiptNumber).ToList();

        }
    }
}
