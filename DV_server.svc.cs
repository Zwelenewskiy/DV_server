using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Models;

namespace DV_server
{
    public class Server : IDV_server
    {
        private static readonly string PATH = @"C:\Users\Pertenava.A\Основное задание\Сервер\DV_server\bin\DBconnection.ini";

        static Server()
        {
            GlobalSettings.connection_string = DataBaseUtils.ReadConnectSettings(PATH);            
        }

        public List<Email> GetEmails()
        {
            return DataBaseUtils.GetRecords();
        }

        public List<User> GetUsers()
        {
            return DataBaseUtils.GetUsers();
        }

        public bool saveEmail(Email email)
        {
            return DataBaseUtils.SaveEmail(email);
        }

        public List<KeyValuePair<int, string>> GetTags()
        {
            return DataBaseUtils.GetTags();
        }
    }
}