using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DV_server
{
    public class DataBaseUtils
    {
        public static readonly string PATH = @"C:\Users\Pertenava.A\Основное задание\Сервер\DV_server\bin\DBconnection.ini";

        /// <summary>
        /// Возращает строку подключения к БД
        /// </summary>
        public static string ReadConnectSettings(string path)
        {
            //string result = @"Data Source=.\<source>;Initial Catalog=<catalog>;Integrated Security=True";
            string result = @"Data Source=<source>;Initial Catalog=<catalog>;Password=<password>;User ID=<login>";

            result = result.Replace("<source>", INIfileUtils.ReadKey(path, "mssql", "server"));
            result = result.Replace("<catalog>", INIfileUtils.ReadKey(path, "mssql", "database"));

           if(INIfileUtils.ReadKey(path, "mssql", "need_auth") == "1")
           {
                result = result.Replace("<password>", INIfileUtils.ReadKey(path, "mssql", "password"));
                result = result.Replace("<login>", INIfileUtils.ReadKey(path, "mssql", "login"));
            }

            return result;
        }

        public static List<Email> GetRecords()
        {
            List<Email> result = new List<Email>();

            using (SqlConnection connection = new SqlConnection(ReadConnectSettings(PATH)))
            {
                connection.Open();

                using(SqlDataReader reader = new SqlCommand("SELECT * FROM email", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //new Email

                            //result.Add(tmp_email);
                        }                        
                    }
                }

                connection.Close();
            }

            return result;
        }//ПЕРЕДЕЛАТЬ ЗАПРОС

        //public static List<User>
    }
}