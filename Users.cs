using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DV_server
{
    [DataContract]
    public class Users
    {
        /*[DataMember]
        public int id;
        [DataMember]
        public string name;
        [DataMember]
        public string patronymic;
        [DataMember]
        public string lastname;
        [DataMember]
        public string email;*/

        [DataMember]
        public List<int> IDs;

        [DataMember]
        public List<string> data;
    }
}