using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DV_server
{
    public static class GlobalSettings
    {
        /// <summary>
        /// Определяет тип используемой СУБД
        /// </summary>
        public enum DbType
        {
            MsSql,
            PostgreSql
        }

        public static string connection_string;
        public static DbType db_type;
    }
}