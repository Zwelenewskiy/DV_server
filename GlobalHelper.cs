using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace DV_server
{
    public static class GlobalHelper
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

        /// <summary>
        /// Сериализует объект в XML-представление
        /// </summary>
        /// <typeparam name="T">Тип передаваемого объекта</typeparam>
        /// <param name="obj">Объект для сериализации</param>
        public static string ToXMLString<T>(this object obj)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            xs.Serialize(ms, obj);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Создает объект из его XML-представления
        /// </summary>
        /// <typeparam name="T">Тип передаваемого объекта</typeparam>
        /// <param name="xml_string">XML-представление объекта</param>
        /// <returns></returns>
        public static T FromXMLString<T>(this string xml_string)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xml_string))
            {
                return (T)serializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Читает из конфигурационного файла настройки для подключения к БД
        /// </summary>
        public static string ReadConnectSettings(string path)
        {
            string result = null;

            switch (INIfileUtils.ReadKey(path, "type", "db_type"))
            {
                case "mssql":
                    GlobalHelper.db_type = GlobalHelper.DbType.MsSql;

                    if (INIfileUtils.ReadKey(path, "data", "need_auth") == "1")
                    {
                        result = $"Data Source={INIfileUtils.ReadKey(path, "data", "server")};Initial Catalog={INIfileUtils.ReadKey(path, "data", "database")};" +
                            $"Password={INIfileUtils.ReadKey(path, "data", "password")};User ID={INIfileUtils.ReadKey(path, "data", "login")}";
                    }
                    else
                    {
                        result = $@"Data Source=.\<{INIfileUtils.ReadKey(path, "data", "server")}>;Initial Catalog=<{INIfileUtils.ReadKey(path, "data", "database")}>;Integrated Security=True";
                    }
                    break;
                case "postgresql":
                    break;
            }

            return result;
        }
    }
}