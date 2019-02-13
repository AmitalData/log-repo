using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Utils
{
    public class IdCounterUtil : IIdCounterUtil
    {
        public string GetNumber(string tableName, int tenant) {
            return IdCounter.GetNumber(tableName, tenant);
        }
    }
}
