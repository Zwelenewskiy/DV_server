using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using Npgsql;

namespace DV_server
{
    public class PostgreSqlDriver : DataBaseDriver
    {
        public PostgreSqlDriver(string connection_string) : base(connection_string) { }

        public override bool AddUsers(List<User> users)
        {
            throw new NotImplementedException();
        }

        public override bool ChangeUser(User user)
        {
            throw new NotImplementedException();
        }

        public override List<Email> GetRecords()
        {
            throw new NotImplementedException();
        }

        public override List<KeyValuePair<int, string>> GetTags()
        {
            throw new NotImplementedException();
        }

        public override List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public override bool SaveEmail(Email email)
        {
            throw new NotImplementedException();
        }

        public override List<Email> SearchByDate(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public override List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateEmail(Email email)
        {
            throw new NotImplementedException();
        }

        protected override string ConvertDateForDB(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}