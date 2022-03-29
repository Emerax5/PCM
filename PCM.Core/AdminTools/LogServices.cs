using PCM.Data.CRUDs;
using PCM.DTO.DTOModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

        public void OfflineLog(string Message, string ErrorMessage) 
        {

            //string path = Path.Combine(Environment.CurrentDirectory, "OfflineLog");
            string path = Path.Combine(Environment.CurrentDirectory, "LogFile.txt");

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine(string.Format("{2}| {0} {1}",Message,ErrorMessage,DateTime.Now.ToString()));
                tw.Close();
            }
            else if (File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(string.Format("{2}| {0} {1}", Message, ErrorMessage, DateTime.Now.ToString()));
                    tw.Close();
                }
            }
        }
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
