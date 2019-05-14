using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Interfaces
{
    public interface IIdCounterUtil
    {
        string GetNumber(string tableName, int tenant);
    }
}
