using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DV_server
{
    interface IDataBaseWork
    {
        object doQuery(string query);
    }
}
