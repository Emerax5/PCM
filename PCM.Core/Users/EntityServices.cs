using MongoDB.Bson;
using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Core.Users
{
    public class EntityServices
    {
        string Collection = "Entities";
        string appDataBase = "AppDataBase";

        public void AddEntity(Entity Entity)
        {            
            EntityCRUD db = new EntityCRUD(appDataBase);
            db.InsertEntity(Collection, Entity);
        }

        public Entity LoadEntityById(ObjectId Id) 
        {
            EntityCRUD db = new EntityCRUD(appDataBase);

            return db.LoadEntityById<DTO.DTOModels.Entity>(Collection,Id);

        }

        public int CheckForEntity() 
        {
            EntityCRUD db = new EntityCRUD(appDataBase);

            return db.CheckForEntity(Collection);
        }

        public void DeleteEntityById(ObjectId Id) 
        {
            EntityCRUD db = new EntityCRUD(appDataBase);
            db.DeleteEntityById<Entity>(Collection, Id);
        }
        public DTO.DTOModels.Entity FindEntityByIdentifier()
        {
            EntityCRUD db = new EntityCRUD(appDataBase);

            return db.LoadEntityByIdentifier<DTO.DTOModels.Entity>(Collection);

        }
    }
}
