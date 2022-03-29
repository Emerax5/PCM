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

            Entry.Date = DateTime.Today.ToLocalTime();

            Entry.ExactTime = DateTime.Now;

            LogCRUD db = new LogCRUD(appDataBase);

            db.InsertLog(Collection, Entry);
        }
        public void Log(string Message, string ErrorMessage)
        {
            var Entry = new LogEntry();

            Entry.Date = DateTime.Today.ToLocalTime();

            Entry.ExactTime = DateTime.Now;

            Entry.Message = Message;

            Entry.ErrorMessage = ErrorMessage;

            Entry.Type = DTO.DTOModels.LogType.error;

            LogCRUD db = new LogCRUD(appDataBase);

            db.InsertLog(Collection, Entry);
        }
        public List<LogEntry> GetLogsByDate(DateTime day)
        {


            LogCRUD db = new LogCRUD(appDataBase);


            return db.LoadLogsByDate<LogEntry>(Collection, day);

        }
    }
}
