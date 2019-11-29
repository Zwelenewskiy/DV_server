using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace DV_server
{
    [Serializable]
    public class Tag
    {
        public int id;
        public string name;
    }

    public class DataBaseUtils
    {
        private enum QueryParams
        {
            withReader,
            nonQuery
        }

        private static string ToXMLString(object obj, Type objType)
        {
            XmlSerializer xs = new XmlSerializer(objType);
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, obj);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static T FromXMLString<T>(string xml_string)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xml_string))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        private static string ConvertDateForDB(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static List<Email> GetEmailsFromReader(SqlDataReader reader)
        {
            List<Email> result = new List<Email>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Email email_tmp = new Email()
                    {
                        id = Convert.ToInt32(reader["id"]),
                        from = Convert.ToInt32(reader["from"]),
                        date = Convert.ToDateTime(reader["date"]),
                        content = reader["content"].ToString(),
                        header = reader["name"].ToString()
                    };

                    email_tmp.to = reader["new_to"].ToString() == "" ? new List<int>() : FromXMLString<List<int>>(reader["new_to"].ToString());
                    email_tmp.copy = reader["new_copy"].ToString() == "" ? new List<int>() : FromXMLString<List<int>>(reader["new_copy"].ToString());
                    email_tmp.hidden_copy = reader["new_hidden_copy"].ToString() == "" ? new List<int>() : FromXMLString<List<int>>(reader["new_hidden_copy"].ToString());
                    email_tmp.tags = reader["new_tags"].ToString() == "" ? new List<KeyValuePair<int, string>>() :
                        new List<KeyValuePair<int, string>>(FromXMLString<List<Tag>>(reader["new_tags"].ToString()).Select(tag => new KeyValuePair<int, string>(tag.id, tag.name)));

                    result.Add(email_tmp);
                }
            }

            reader.Close();

            return result;
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
            List<KeyValuePair<int, int>> to = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> copy = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> hidden_copy = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> email_tag = new List<KeyValuePair<int, int>>();
            List<User> users = GetUsers();
            List<KeyValuePair<int, string>> tags = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
            {
                connection.Open();

                //получили To
                using (SqlDataReader reader = new SqlCommand("EXEC GetTo", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {              
                        while (reader.Read())
                        {
                            to.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["email_id"]), Convert.ToInt32(reader["user_id"])));
                        }
                    }
                }

                //получили copy
                using (SqlDataReader reader = new SqlCommand("EXEC GetCopy", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            copy.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["email_id"]), Convert.ToInt32(reader["user_id"])));
                        }
                    }
                }

                //получили hidden_copy
                using (SqlDataReader reader = new SqlCommand("EXEC GetHiddenCopy", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            hidden_copy.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["email_id"]), Convert.ToInt32(reader["user_id"])));
                        }
                    }
                }

                //получили tags
                using (SqlDataReader reader = new SqlCommand("EXEC GetTags", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tags.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["name"].ToString()));
                        }
                    }
                }

                //получили EmailTag
                using (SqlDataReader reader = new SqlCommand("EXEC GetEmailTag", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            email_tag.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["email_id"]), Convert.ToInt32(reader["tag_id"])));
                        }
                    }
                }

                //получили email'ы
                using (SqlDataReader reader = new SqlCommand("EXEC GetEmails", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int current_email_id = Convert.ToInt32(reader["id"]);

                            List<int> new_to = new List<int>(to.Where(x => x.Key == current_email_id).Select(x => x.Value).ToList());

                            List<int> new_copy = new List<int>(copy.Where(x => x.Key == current_email_id).Select(x => x.Value).ToList());

                            List<int> new_hidden_copy = new List<int>(hidden_copy.Where(x => x.Key == current_email_id).Select(x => x.Value).ToList());

                            var tmp_email_tag = new List<KeyValuePair<int, int>>(email_tag.Where(x => x.Key == current_email_id));
                            List<KeyValuePair<int, string>> new_tags = new List<KeyValuePair<int, string>>(tags.Join(tmp_email_tag, 
                                                                    tag_id => tag_id.Key,
                                                                    em_tag => em_tag.Value,
                                                                    (tag_id, em_tag) => new KeyValuePair<int, string>(em_tag.Value, tag_id.Value)));

                            result.Add(new Email()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                header = reader["name"].ToString(),
                                date = Convert.ToDateTime(reader["date"]),
                                from = Convert.ToInt32(reader["from"]),
                                content = reader["content"].ToString(),
                                to = new_to,
                                copy = new_copy,
                                hidden_copy = new_hidden_copy,
                                tags = new_tags
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

                using (SqlDataReader reader = new SqlCommand("EXEC GetUsers", connection).ExecuteReader())
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

                    foreach (var tag_name in email.tags)
                    {
                        new SqlCommand($"EXEC SaveEmailTag {tag_name.Key}, '{ID}'", connection).ExecuteNonQuery();
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

        public static List<KeyValuePair<int, string>> GetTags()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
            {
                connection.Open();
                
                using (SqlDataReader reader = new SqlCommand("EXEC GetTags", connection).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["name"].ToString()));
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
         
        public static bool UpdateEmail(Email email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

                    new SqlCommand($"EXEC UpdateEmail {email.id}, {email.from}, '{email.date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}',  '{email.content}',  '{email.header}',  " +
                        $"'{ToXMLString(email.to, typeof(List<int>))}', " +
                        $"'{ToXMLString(email.copy, typeof(List<int>))}', " +
                        $"'{ToXMLString(email.hidden_copy, typeof(List<int>))}', " +
                        $"'{ToXMLString(email.tags.Select(tag => tag.Key).ToList(), typeof(List<int>))}'", connection).ExecuteNonQuery();

                    connection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }            
        }

        public static List<Email> SearchByDate(DateTime from, DateTime to)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

                    List<Email> result = GetEmailsFromReader(new SqlCommand($"EXEC SearchByDate '{ConvertDateForDB(from)}', '{ConvertDateForDB(to)}'", connection).ExecuteReader());

                    connection.Close();
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

                    List<Email> result = GetEmailsFromReader(new SqlCommand($"EXEC SearchByTags '{ToXMLString(tags.Select(tag => tag.Key).ToList(), typeof(List<int>))}'", connection).ExecuteReader());                    

                    connection.Close();
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool ChangeUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

                    new SqlCommand($"EXEC ChangeUser {user.id}, '{user.name}', '{user.patronymic}', '{user.lastname}', '{user.email}', ", connection).BeginExecuteNonQuery();

                    connection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool AddUsers(List<User> users)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalSettings.connection_string))
                {
                    connection.Open();

                    new SqlCommand($"EXEC AddUser '{ToXMLString(users, users.GetType())}', ", connection).BeginExecuteNonQuery();

                    connection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}