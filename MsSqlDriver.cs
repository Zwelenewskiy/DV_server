using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DV_server
{
    public class MsSQLDriver: DataBaseDriver
    {
        public MsSQLDriver(string connection_string): base(connection_string){}

        protected override string ConvertDateForDB(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override bool AddUsers(List<User> users)
        {
            return false;
        }

        public override bool ChangeUser()
        {
            return false;
        }        

        public override List<Email> GetRecords()
        {
            return null;
        }

        public override List<KeyValuePair<int, string>> GetTags()
        {
            return null;
        }

        public override List<User> GetUsers()
        {
            return null;
        }

        public override bool SaveEmail()
        {
            return false;
        }

        public override List<Email> SearchByDate(DateTime from, DateTime to)
        {
            return null;
        }

        public override List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            return null;
        }

        public override bool UpdateEmail(Email email)
        {
            return false;
        }
    }
}