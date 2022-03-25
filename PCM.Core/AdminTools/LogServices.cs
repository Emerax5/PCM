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
        public void Log(string Message)
        {
            var Entry = new LogEntry();

            Entry.Message = Message;

            Entry.Time = DateTime.Now;

            LogCRUD db = new LogCRUD(appDataBase);

            db.InsertLog(Collection, Entry);
        }
        public void Log(string Message, string ErrorMessage)
        {
            var Entry = new LogEntry();

            Entry.Time = DateTime.Now;

            Entry.Message = Message;

            Entry.ErrorMessage = ErrorMessage;

            Entry.Type = DTO.DTOModels.LogType.error;

            LogCRUD db = new LogCRUD(appDataBase);

            db.InsertLog(Collection, Entry);
        }
    }
}
