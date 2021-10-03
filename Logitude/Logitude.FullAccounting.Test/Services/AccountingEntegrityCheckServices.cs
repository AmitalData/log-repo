
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class AccountingEntegrityCheckServices
    {   

        public AccountingIntegrityCheckPM Create(int tenant)
        {
            return new AccountingIntegrityCheckPM()
            {
                StatusCode = "1",
                CreateDateTimeUTC = DateTime.Now,
                FromMonthInclusive = DateTime.Now,
                ToMonthInclusive = DateTime.Now.AddDays(1),
                Tenant = tenant
            };

            

        }


    }
}
