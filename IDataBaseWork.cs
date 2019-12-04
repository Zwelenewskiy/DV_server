using Models;
using System;
using System.Collections.Generic;

namespace DV_server
{
    interface IDataBaseWork
    {
        //string ConvertDateForDB(DateTime dateTime);
        List<Email> GetRecords();
        List<User> GetUsers();
        bool SaveEmail(Email email);
        List<KeyValuePair<int, string>> GetTags();
        bool UpdateEmail(Email email);
        List<Email> SearchByDate(DateTime from, DateTime to);
        List<Email> SearchByTags(List<KeyValuePair<int, string>> tags);
        bool ChangeUser(User user);
        bool AddUsers(List<User> users);
    }
}
