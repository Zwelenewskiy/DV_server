using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DV_server
{
    public abstract class DataBaseDriver : IDataBaseWork
    {
        protected string conn_string;

        public DataBaseDriver(string connection_string)
        {
            conn_string = connection_string;
        }

        protected abstract string ConvertDateForDB(DateTime dateTime);

        public abstract bool AddUsers(List<User> users);

        public abstract bool ChangeUser();

        public abstract List<Email> GetRecords();

        public abstract List<KeyValuePair<int, string>> GetTags();

        public abstract List<User> GetUsers();

        public abstract bool SaveEmail();

        public abstract List<Email> SearchByDate(DateTime from, DateTime to);

        public abstract List<Email> SearchByTags(List<KeyValuePair<int, string>> tags);

        public abstract bool UpdateEmail(Email email);
    }
}