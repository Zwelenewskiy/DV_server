using System;
using System.Collections.Generic;

namespace Models
{
    public class Email
    {
        public int id;
        public int from;
        public string header;
        public DateTime date;
        public string content;
        public List<int> to;
        public List<int> copy;
        public List<int> hidden_copy;
        public List<KeyValuePair<int, string>> tags;
    }
}