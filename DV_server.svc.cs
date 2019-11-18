using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DV_server
{
    public class Server : IDV_server
    {
        public List<Email> GetEmails()
        {
            return DataBaseUtils.GetRecords();
        }

        public bool saveEmail(Email email)
        {
            try
            {
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}
