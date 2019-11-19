using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DV_server
{
    [DataContract]
    public class Email
    {
        public int _id;
        public string _from;
        public string _header;
        public DateTime _date;
        public string _content;
        public List<string> _to;
        public List<string> _copy;
        public List<string> _hidden_copy;
        public List<string> _tags;

        public Email(int id, string from, string header, DateTime date, string content, List<string> to, List<string> copy, List<string> hidden_copy, List<string> tags)
        {
            _id = id;
            _from = from;
            _header = header;
            _date = date;
            _content = content;
            _to = to;
            _copy = copy;
            _hidden_copy = hidden_copy;
            _tags = tags;
        }
    }
}