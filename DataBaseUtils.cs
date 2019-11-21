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
        private enum QueryParams
        {
            withReader,
            nonQuery
        }

        /*private static T DoQuery<T>(string query, QueryParams query_param, T t)
        {
            using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
            {
                connection.Open();

                switch (query_param)
                {
                    case QueryParams.withReader:
                        using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                }
                            }
                        }

                        break;

                    case QueryParams.nonQuery:
                        return new SqlCommand(query, connection).ExecuteNonQuery();
                }             

                connection.Close();
            }

            return t;
        }*/

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
            List<int[]> to = new List<int[]>();
            List<int[]> copy = new List<int[]>();
            List<int[]> hidden_copy = new List<int[]>();
            List<User> users = new List<User>();
            Dictionary<int, string> tags = new Dictionary<int, string>();

            using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
            {
                connection.Open();
                  
                //получили To
                using (SqlDataReader reader = new SqlCommand("SELECT email_id, user_id FROM [to]", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {              
                        while (reader.Read())
                        {
                            int[] tmp = new int[2];

                            tmp[0] = Convert.ToInt32(reader["email_id"]);
                            tmp[1] = Convert.ToInt32(reader["user_id"]);

                            to.Add(tmp);
                            tmp = new int[2];
                        }
                    }
                }

                //получили copy
                using (SqlDataReader reader = new SqlCommand("SELECT email_id, user_id FROM copy", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int[] tmp = new int[2];

                            tmp[0] = Convert.ToInt32(reader["email_id"]);
                            tmp[1] = Convert.ToInt32(reader["user_id"]);

                            copy.Add(tmp);
                            tmp = new int[2];
                        }
                    }
                }

                //получили hidden_copy
                using (SqlDataReader reader = new SqlCommand("SELECT email_id, user_id FROM hidden_copy", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int[] tmp = new int[2];

                            tmp[0] = Convert.ToInt32(reader["email_id"]);
                            tmp[1] = Convert.ToInt32(reader["user_id"]);

                            hidden_copy.Add(tmp);
                            tmp = new int[2];
                        }
                    }
                }

                //получили user
                using (SqlDataReader reader = new SqlCommand("SELECT id, lastname, name, patronymic, email FROM [user]", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                lastname = reader["lastname"].ToString(),
                                name = reader["name"].ToString(),
                                patronymic = reader["patronymic"].ToString(),
                                email = reader["email"].ToString(),
                            });
                        }
                    }
                }

                //получили tags
                /*using (SqlDataReader reader = new SqlCommand("SELECT email_id, name FROM tag", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tags.Add(Convert.ToInt32(reader["email_id"]), reader["name"].ToString());
                        }
                    }
                }*/

                //получили email'ы
                using (SqlDataReader reader = new SqlCommand("SELECT [id], name, [date], [from], content FROM email", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var tmp_to = new List<int[]>(to.Where(x => x[0] == Convert.ToInt32(reader["id"])));
                            List<int> new_to = new List<int>();
                            foreach(int[] t in tmp_to)
                            {
                                new_to.Add(t[1]);
                            }

                            var tmp_copy = new List<int[]>(copy.Where(x => x[0] == Convert.ToInt32(reader["id"])));
                            List<int> new_copy = new List<int>();
                            foreach (int[] t in tmp_copy)
                            {
                                new_copy.Add(t[1]);
                            }

                            var tmp_hidden_copy = new List<int[]>(hidden_copy.Where(x => x[0] == Convert.ToInt32(reader["id"])));
                            List<int> new_hidden_copy = new List<int>();
                            foreach (int[] t in tmp_hidden_copy)
                            {
                                new_hidden_copy.Add(t[1]);
                            }

                            result.Add(new Email()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                header = reader["name"].ToString(),
                                date = Convert.ToDateTime(reader["date"]),
                                from = Convert.ToInt32(reader["from"]),
                                content = reader["content"].ToString(),
                                to = new_to,
                                copy = new_copy,
                                hidden_copy = new_hidden_copy
                            });
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static List<User> GetUsers()
        {
            List<User> result = new List<User>();

            using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
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
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

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