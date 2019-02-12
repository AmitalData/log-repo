using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Mocks
{
    public class MockDateTimeUtil : IDateTimeUtil
    {
        public DateTime GetCurrentDateTime(int tenant)
        {
            return DateTime.Now;
        }
    }
}
