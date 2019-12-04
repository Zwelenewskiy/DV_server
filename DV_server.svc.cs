using System;
using System.Collections.Generic;
using Models;

namespace DV_server
{
    public class Server : IDV_server
    {
        private static readonly string PATH = @"C:\Users\Pertenava.A\Основное задание\Сервер\DV_server\bin\DBconnection.ini";
        private static IDataBaseWork data_base_worker;

        static Server()
        {
            GlobalHelper.connection_string = GlobalHelper.ReadConnectSettings(PATH);            

            switch (GlobalHelper.db_type)
            {
                case GlobalHelper.DbType.MsSql:
                    data_base_worker = new MsSqlDriver(GlobalHelper.connection_string);
                    break;

                case GlobalHelper.DbType.PostgreSql:
                    break;
            }
        }

        public List<Email> GetEmails()
        {
            return data_base_worker.GetRecords();
        }

        public List<User> GetUsers()
        {
            return data_base_worker.GetUsers();
        }

        public bool SaveEmail(Email email)
        {
            return data_base_worker.SaveEmail(email);
        }

        public List<KeyValuePair<int, string>> GetTags()
        {
            return data_base_worker.GetTags();
        }

        public bool UpdateEmail(Email email)
        {
            return data_base_worker.UpdateEmail(email);
        }

        public List<Email> SearchByDate(DateTime from, DateTime to)
        {
            return data_base_worker.SearchByDate(from, to);
        }

        public List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            return data_base_worker.SearchByTags(tags);
        }

        public bool ChangeUser(User user)
        {
            return data_base_worker.ChangeUser(user);
        }

        public bool AddUsers(List<User> users)
        {
            return data_base_worker.AddUsers(users);
        }
    }
}