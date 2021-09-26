
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
    public class BankDepositService
    {   

        public BankDepositPM Create(Table table)
        {
            dynamic bankDepositTable = table.CreateDynamicInstance();
            var bankDeposit = new BankDepositPM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                AccountingDate = DateTime.Now,
                DepositDate = DateTime.Now,
                DepositCurrencyId = BillingData.CurrencyNISId,
                DepositBankAccountId = FullAccountingData.BankAccountId,
                IsCashDeposit = true,
                ForeignAmount = (decimal)bankDepositTable.ForeignAmount,
                LocalDepositAmount = (decimal)bankDepositTable.LocalDepositAmount,
                CashBookId = new CashBookPreparation().GetNewCashNIS(),
                Tenant = UserTenant.Tenant
            };

            return bankDeposit;

        }


    }
}
