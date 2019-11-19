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
        [DataMember]
        public int id;
        [DataMember]
        public string from;
        [DataMember]
        public string header;
        [DataMember]
        public DateTime date;
        [DataMember]
        public string content;
        [DataMember]
        public List<string> to;
        [DataMember]
        public List<string> copy;
        [DataMember]
        public List<string> hidden_copy;
        [DataMember]
        public List<string> tags;
    }
}