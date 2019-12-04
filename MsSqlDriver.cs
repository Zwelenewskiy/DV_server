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
    public class MsSqlDriver: DataBaseDriver
    {
        public MsSqlDriver(string connection_string): base(connection_string){}
        
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

                    email_tmp.to = reader["to"].ToString() == "" ? new List<int>() : reader["to"].ToString().FromXMLString<List<int>>();
                    email_tmp.copy = reader["copy"].ToString() == "" ? new List<int>() : reader["copy"].ToString().FromXMLString<List<int>>();
                    email_tmp.hidden_copy = reader["hidden_copy"].ToString() == "" ? new List<int>() : reader["hidden_copy"].ToString().FromXMLString<List<int>>();
                    email_tmp.tags = reader["tags"].ToString() == "" ? new List<KeyValuePair<int, string>>() :
                        new List<KeyValuePair<int, string>>(reader["tags"].ToString().FromXMLString<List<Tag>>().Select(tag => new KeyValuePair<int, string>(tag.id, tag.name)));

                    result.Add(email_tmp);
                }
            }

            reader.Close();

            return result;
        }

        protected override string ConvertDateForDB(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override bool AddUsers(List<User> users)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
                {
                    connection.Open();
                    new SqlCommand($"EXEC AddUser '{users.ToXMLString<List<int>>()}', ", connection).BeginExecuteNonQuery();
                    connection.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public override bool ChangeUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
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

        public override List<Email> GetRecords()
        {
            List<Email> result = new List<Email>();

            using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
            {
                connection.Open();
                result = GetEmailsFromReader(new SqlCommand("EXEC GetAllEmails", connection).ExecuteReader());
                connection.Close();
            }

            return result;
        }

        public override List<KeyValuePair<int, string>> GetTags()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
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

        public override List<User> GetUsers()
        {
            List<User> result = new List<User>();

            using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
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

        public override bool SaveEmail(Email email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
                {
                    connection.Open();

                    new SqlCommand($"EXEC SaveEmail {email.from}, '{ConvertDateForDB(email.date)}', '{email.content}', '{email.header}', '{email.to.ToXMLString<List<int>>()}'," +
                        $" '{email.copy.ToXMLString<List<int>>()}', '{email.hidden_copy.ToXMLString<List<int>>()}'," +
                        $" '{email.tags.Select(tag => tag.Key).ToList().ToXMLString<List<int>>()}'", connection).ExecuteNonQuery();

                    connection.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override List<Email> SearchByDate(DateTime from, DateTime to)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
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

        public override List<Email> SearchByTags(List<KeyValuePair<int, string>> tags)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
                {
                    connection.Open();
                    List<Email> result = GetEmailsFromReader(new SqlCommand($"EXEC SearchByTags '{tags.Select(tag => tag.Key).ToList().ToXMLString<List<int>>()}'", connection).ExecuteReader());
                    connection.Close();

                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        public override bool UpdateEmail(Email email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GlobalHelper.connection_string))
                {
                    connection.Open();

                    new SqlCommand($"EXEC UpdateEmail {email.id}, {email.from}, '{ConvertDateForDB(email.date)}',  '{email.content}',  '{email.header}',  " +
                        $"'{email.to.ToXMLString<List<int>>()}', " +
                        $"'{email.copy.ToXMLString<List<int>>()}', " +
                        $"'{email.hidden_copy.ToXMLString<List<int>>()}', " +
                        $"'{email.tags.Select(tag => tag.Key).ToList().ToXMLString<List<int>>()}'", connection).ExecuteNonQuery();

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