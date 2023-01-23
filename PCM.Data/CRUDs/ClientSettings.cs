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

            if (online == true)
            {
                //for online database credentials
                return new MongoClient();
            }
            else
            {
                return new MongoClient();

            }


        }
     


    }
}
