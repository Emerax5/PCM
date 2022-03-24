using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }        
        public string UserName { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public Role Role { get; set; }

        
    }

    public enum Role 
    {
        Secretary, 
        Doctor, 
        Admin   
    
    }
}
