using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using PCM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PCM.Data.CRUDs
{
    public class PatientCRUD
    {
        private IMongoDatabase db;

        public PatientCRUD(string database)
        {

            var clientSettings = new ClientSettings();

            var client = clientSettings.ClientSetting(database);

            db = client.GetDatabase(database);

        }

        public void InsertPatient<T>(string Table, T record)
        {

            var collection = db.GetCollection<T>(Table);
            collection.InsertOne(record);

        }
        public List<T> LoadPatients<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();

        }
        public List<Patient> LoadPatientsByFullName<T>(string table, string fullName)
        {
            var collection = db.GetCollection<Patient>(table);        

            var regexFilter = Regex.Escape(fullName);
            var bsonRegex = new BsonRegularExpression(regexFilter, "i");
            var filter = Builders<Patient>.Filter.Regex("FullName", bsonRegex);
            return collection.Find(filter).SortBy(x => x.FullName).ToList();

        }

        public List<Patient> LoadPatientsByLastName<T>(string table, string lastName)
        {
            var collection = db.GetCollection<Patient>(table);
            var filter = Builders<Patient>.Filter.Eq("LastName", lastName);

            return collection.Find(filter).SortBy(x => x.LastName).ToList();

        }
        public List<T> LoadPatientsPhoneNumber<T>(string table, string PhoneNumber)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PhoneNumber", PhoneNumber);

            return collection.Find(filter).ToList();

        }

        public T LoadPatientByID<T>(string table, ObjectId id)
        {
           

            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
          
          

        }

        public void DeletePatientByID<T>(string table, ObjectId id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            collection.FindOneAndDelete(filter);

        }
        
        public List<T> LoadPatientByPersonalID<T>(string table, string personalId)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("PersonalID", personalId);

            return collection.Find(filter).ToList();

        }
        public List<T> LoadAllPatients<T>(string table)
        {
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();

        }

        public void UpsertPatientByID<T>(string table, ObjectId id, T patient)
        {

            var collection = db.GetCollection<T>(table);
            var result = collection.ReplaceOne(new BsonDocument("_id", id), patient, 
            
                new ReplaceOptions { IsUpsert = true });

        }

        public void UpsertNotesID<T>(string table, ObjectId id, string Note, DateTime NoteUpserDate, string User)
        {
            var collection = db.GetCollection<BsonDocument>(table);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            var update = Builders<BsonDocument>.Update.Set("Alergies", Note);

            var UpdateDate = Builders<BsonDocument>.Update.Set("UpsertAlergieDate", NoteUpserDate);

            var UpdatedBy = Builders<BsonDocument>.Update.Set("AlegieEditedBy", User);


            collection.UpdateOne(filter, update);
            collection.UpdateOne(filter, UpdateDate);
            collection.UpdateOne(filter, UpdatedBy);

        }

        public List<Patient> LoadAllPatientsWithPaging<T>(string table, int PageSize, int SkipPages)
        {
            var collection = db.GetCollection<Patient>(table);

            return collection.Find(new BsonDocument()).SortBy(c => c.Name)
                .Limit(PageSize)
                .Skip(SkipPages)
                .ToList()
                ;

        }

        public int CountPatients(string table)
        {
            var collection = db.GetCollection<Patient>(table);

            return (int)collection.Find(new BsonDocument()).CountDocuments();

        }
        public void InsertPatientFile(byte[] file, string PatienId, string fileName)
        {

            var bucket = new GridFSBucket(db);

            var options = new GridFSUploadOptions
            {
               Metadata = new BsonDocument
               {
                 { "PatientID", PatienId },
               }
            };

            bucket.UploadFromBytes(fileName,file,options);

        }

        public List<PatientDocumentInfo> GetBasicDocumentInfoList<T>(string PatienId)
        {
            var DocList = new List<PatientDocumentInfo>();

            var bucket = new GridFSBucket(db);

            var filter = Builders<GridFSFileInfo>.Filter.Eq("Metadata.PatientID", PatienId);

            var list = bucket.Find(filter).ToList();

            foreach (var doc in list)
            {
                DocList.Add(new PatientDocumentInfo{ 
                DocumentName = doc.Filename,
                DocumentId = doc.Id                
                });

            }

            return DocList;

        }

        public byte[] GetPatientFileByDocumentId(ObjectId id) 
        {

            var bucket = new GridFSBucket(db);

            return bucket.DownloadAsBytes(id); ;        
        }

        public void DeletePatientFileByDocumentId(ObjectId id)
        {

            var bucket = new GridFSBucket(db);

             bucket.Delete(id);
        }
      
    }
}
