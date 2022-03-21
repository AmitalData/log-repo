
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
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
                ChequeCounter = 1,
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = bankAccountTable.Name,
                LocalName = bankAccountTable.Name
            };

            return bankAccount;

        }


    }
}
