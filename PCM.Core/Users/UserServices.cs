using System;
using System.Collections.Generic;
using System.Text;
using PCM;
using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;

namespace PCM.Core.Users
{
    public class UserServices
    {
        string Collection = "Users";
        string appDataBase = "AppDataBase";
        public void AddUser(User NewUser)
        {
          
            var dbUser = new Data.Models.User();

            dbUser.UserName = NewUser.UserName;
            dbUser.SecurityQuestion = NewUser.SecurityQuestion;
            dbUser.SecurityAnswer = NewUser.SecurityAnswer;
            dbUser.Role = (Data.Models.Role)NewUser.Role;

            UserCRUD db = new UserCRUD(appDataBase);
            db.InsertUser(Collection, dbUser);         

        }
        public List<User> GetUsers<T>()
        { 
            
            var users = new List<User>();
            UserCRUD db = new UserCRUD(appDataBase);
            var dbusers = db.LoadUsers<Data.Models.User>(Collection);
          
            foreach (var user in dbusers)
            {
                users.Add(new User()
                {
                    UserName = user.UserName,
                    Role = (Role)user.Role,
                    SecurityQuestion = user.SecurityQuestion,
                    SecurityAnswer = user.SecurityAnswer
                }); ;
          
            }
          
                return users;          
        }
        public User GetUserByUserName(string UserName)
        {
          
            var dtoUser = new User();

            UserCRUD db = new UserCRUD(appDataBase);
            var dbUser = db.LoadUserByUserName<Data.Models.User>(Collection,UserName);

            dtoUser.Id = dbUser.Id;
            dtoUser.UserName = dbUser.UserName;
            dtoUser.SecurityQuestion = dbUser.SecurityQuestion;
            dtoUser.SecurityAnswer = dbUser.SecurityAnswer;
            dtoUser.Role = (Role)dbUser.Role;

            return dtoUser;
          
        }
        public void EraseByUserName(string UserName)
        {            

            UserCRUD db = new UserCRUD(appDataBase);
            db.DeleteByUserName<User>(Collection, UserName);   
         
        }

        public Role GetRoleByUserName(string UserName) 
        {
            UserCRUD db = new UserCRUD(appDataBase);
            
            var User =  db.LoadUserByUserName<User>(Collection, UserName);
            
            return User.Role; 
        }

        public int UserCount() 
        {
            UserCRUD db = new UserCRUD(appDataBase);

            return db.GetUserCount<int>(Collection);
        }
    }

}
