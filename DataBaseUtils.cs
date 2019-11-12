using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DV_server
{
    public class DataBaseUtils
    {
        //public static readonly string PATH = @"C:\Users\Pertenava.A\Основное задание\Сервер\DV_server\bin\Debug\DBconnection.ini";
        public static readonly string PATH = "DBconnection.ini";

        /// <summary>
        /// Возращает строку подключения к БД
        /// </summary>
        public static string ReadConnectSettings(string path)
        {
            string result = @"Data Source=.\<source>;Initial Catalog=<catalog>;Integrated Security=True";

            result = result.Replace("<source>", INIfileUtils.ReadKey(path, "mssql", "server"));
            result = result.Replace("<catalog>", INIfileUtils.ReadKey(path, "mssql", "catalog"));

            return result;
        }

        public static List<Email> GetRecords()
        {
            /*using (SqlConnection connection = new SqlConnection(ReadConnectSettings(PATH)))
            {
                connection.Open();

                Console.WriteLine(connection.State == System.Data.ConnectionState.Open);

                connection.Close();
            }*/

            Console.WriteLine(ReadConnectSettings(PATH));
            return new List<Email>();
        }
    }
}