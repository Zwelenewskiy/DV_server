using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Models;

namespace DV_server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDV_server
    {
        [OperationContract]
        List<Email> GetEmails();

        [OperationContract]
        bool SaveEmail(Email email);

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        List<KeyValuePair<int, string>> GetTags();

        [OperationContract]
        bool UpdateEmail(Email email);

        [OperationContract]
        List<Email> SearchByDate(DateTime from, DateTime to);

        [OperationContract]
        List<Email> SearchByTags(List<KeyValuePair<int, string>> tags);

        [OperationContract]
        bool ChangeUser(User user);

        [OperationContract]
        bool AddUsers(List<User> users);
    }
}
