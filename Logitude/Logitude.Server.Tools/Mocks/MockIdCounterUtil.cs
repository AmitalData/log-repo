using Logitude.Server.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Mocks
{
    public class MockIdCounterUtil:IIdCounterUtil
    {
        public string GetNumber(string tableName, int tenant)
        {
            return "Id_For_Mock";
        }
    }
}
