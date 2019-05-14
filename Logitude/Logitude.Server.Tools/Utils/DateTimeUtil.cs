using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Utils
{
    public class DateTimeUtil : IDateTimeUtil
    {
        public DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }
    }
}
