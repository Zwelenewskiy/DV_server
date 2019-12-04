using System;
using System.Collections.Generic;
using System.ServiceModel;
using Models;

namespace DV_server
{
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
