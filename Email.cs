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
        public string name;
        public string date;
        public string content;
        public string FIO;
    }
}