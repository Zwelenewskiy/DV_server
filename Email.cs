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
        public int from;
        [DataMember]
        public string header;
        [DataMember]
        public DateTime date;
        [DataMember]
        public string content;
        [DataMember]
        public List<int> to;
        [DataMember]
        public List<int> copy;
        [DataMember]
        public List<int> hidden_copy;
        [DataMember]
        public List<int> tags;
    }
}