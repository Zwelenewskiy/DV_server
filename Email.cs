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
        public int id;
        public string from;
        public string header;
        public string date;
        public string content;
        public List<string> to;
        public List<string> copy;
        public List<string> hidden_copy;
        public List<string> tags;
    }
}