using Logitude.Test.Base.Models.UserTenantPreparation;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingTests.Services
{
    public class DateHelper
    {
        public static DateTime FillTodayDate(string Date)
        {
            if (Date.ToUpper() == "TODAY")
            {
                return TenantServerConfigration.GetCurrentDateTime(UserTenant.Tenant);
            }
            return TenantServerConfigration.GetCurrentDateTime(UserTenant.Tenant); 
        }
    }
}
