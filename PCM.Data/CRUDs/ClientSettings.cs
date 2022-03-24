using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCM.Data.CRUDs
{
    public class ClientSettings
    {
        public MongoClient ClientSetting(string database)
        {

            bool online = true;

            if (online = true)
            {
                return new MongoClient(string.Format("mongodb+srv://Enmanuel:ditto05@pcmcluster.sjjmn.mongodb.net/{0}?retryWrites=true&w=majority", database));
            }
            else
            {
                return new MongoClient();

            }


        }
     


    }
}
