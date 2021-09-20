
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
    public class BankAccountService
    {
        

        public BankAccountPM Create(Table table)
        {
            dynamic bankAccountTable = table.CreateDynamicInstance();
            var bankAccount = new BankAccountPM()
            {
                Tenant = UserTenant.Tenant,
                BankId = FullAccountingData.BankCodeId,
                BranchNumber = bankAccountTable.BranchNumber.ToString(),
                AccountNumber = DateTime.Now.Ticks.ToString().Substring(3),
                CurrencyId = BillingData.CurrencyNISId,
                Inactive = false,
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = bankAccountTable.Name
            };

            return bankAccount;

        }


    }
}
