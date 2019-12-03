using System;
using System.Collections.Generic;
using System.IO;
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

        public bool ChangeEmail(Email email)
        {
            return DataBaseUtils.UpdateEmail(email);

        }

        public List<Email> SearchByDate(DateTime from, DateTime to)
        {
            return DataBaseUtils.SearchByDate(from, to);
        }

        public List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            return DataBaseUtils.SearchByTags(tags);
        }

        public bool ChangeUser(User user)
        {
            return DataBaseUtils.ChangeUser(user);
        }

        public bool AddUsers(List<User> users)
        {
            return DataBaseUtils.AddUsers(users);
        }

        public List<Email> SomeMethod()
        {
            return DataBaseUtils.DataBaseQueryManager<List<Email>>(DataBaseUtils.QueryType.getEmails);
        }
    }
}