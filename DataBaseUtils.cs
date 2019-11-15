using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DV_server
{
    public class DataBaseUtils
    {
        private static readonly string PATH = @"C:\Users\Pertenava.A\Основное задание\Сервер\DV_server\bin\DBconnection.ini";

        /// <summary>
        /// Возращает строку подключения к БД
        /// </summary>
        public static string ReadConnectSettings(string path)
        {
            //string result = @"Data Source=.\<source>;Initial Catalog=<catalog>;Integrated Security=True";
            string result = @"Data Source=<source>;Initial Catalog=<catalog>;Password=<password>;User ID=<login>;Integrated Security=True";

            result = result.Replace("<source>", INIfileUtils.ReadKey(path, "mssql", "server"));
            result = result.Replace("<catalog>", INIfileUtils.ReadKey(path, "mssql", "catalog"));

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

                var t = new SqlCommand("SELECT id, name, date, content, ФИО FROM DV_Test_server").ExecuteReader();

                using(SqlDataReader reader = new SqlCommand("SELECT * FROM emails").ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Email tmp_email = new Email()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                name = reader["id"].ToString(),
                                date = reader["id"].ToString(),
                                content = reader["id"].ToString(),
                                FIO = reader["id"].ToString(),
                            };
                        }                        
                    }
                }

                connection.Close();
            }


            return result;
        }
    }
}