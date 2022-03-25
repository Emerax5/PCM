using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Core.AdminTools
{
    public class LogServices
    {
        string Collection = "LogEntries";
        string appDataBase = "AppDataBase";
        public void AddLog(LogEntry Entry)
        {
            LogCRUD db = new LogCRUD(appDataBase);
            db.InsertLog(Collection, Entry);
        }

    }
}
