using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DV_server
{
    public class MsSqlDriver : IDataBaseWork
    {
        public object ExecuteReader(string query)
        {
            return null;
        }

        public object ExecuteNonQuery(string query)
        {
            return null;
        }

        public object ExecuteScalar(string query)
        {
            return null;
        }
    }
}