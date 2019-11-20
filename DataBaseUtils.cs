using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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

        public static List<User> GetUsers()
        {
            List<User> result = new List<User>();

            using (SqlConnection connection = new SqlConnection(ReadConnectSettings(PATH)))
            {
                connection.Open();

                using (SqlDataReader reader = new SqlCommand("SELECT ID, name, patronymic, lastname, email FROM [user]", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new User()
                            {
                                id = Convert.ToInt32(reader["ID"]),
                                lastname = reader["lastname"].ToString(),
                                name = reader["name"].ToString(),
                                patronymic = reader["patronymic"].ToString(),
                                email = reader["email"].ToString()                                
                            });
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static bool SaveEmail(Email email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ReadConnectSettings(PATH)))
                {
                    connection.Open();

                    /*int ID = Convert.ToInt32(new SqlCommand($"INSERT INTO [dbo].[email] ([from] ,[date] ,[content] ,[name]) VALUES('{email.from}', " +
                        $"'{email.date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}', '{email.content}', '{email.header}'); SELECT SCOPE_IDENTITY()", connection).ExecuteScalar());*/

                    int ID = Convert.ToInt32(new SqlCommand($"EXEC Add_in_email {email.from}, " +
                        $"'{email.date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}', '{email.content}', '{email.header}'", connection).ExecuteScalar());

                    foreach (int user_id in email.to)
                    {
                        new SqlCommand($"EXEC Add_in_to {ID}, {user_id}", connection).ExecuteNonQuery();
                    }

                    foreach (int user_id in email.copy)
                    {
                        new SqlCommand($"EXEC Add_in_copy {ID}, {user_id}", connection).ExecuteNonQuery();
                    }

                    foreach (int user_id in email.hidden_copy)
                    {
                        new SqlCommand($"EXEC Add_in_hidden_copy {ID}, {user_id}", connection).ExecuteNonQuery();
                    }

                    foreach (string tag_name in email.tags)
                    {
                        new SqlCommand($"EXEC Add_in_tag {ID}, '{tag_name}'", connection).ExecuteNonQuery();
                    }

                    connection.Close();
                }

                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}