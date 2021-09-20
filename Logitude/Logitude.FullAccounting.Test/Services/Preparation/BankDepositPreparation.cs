using Logitude.FullAccounting.Test.Models;
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

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class BankDepositPreparation
    {
        
        public void Prepare()
        {
            FullAccountingData.BankDeposit1 = Create();
        }

        public string Create()
        {
            var bankDeposit = CreateInstance();
            var response = APICaller.CallPost<BankDepositPM>(bankDeposit, Urls.BankDepositsController, UserTenant.Token);
            return response.Data?.Id;
        }
        private BankDepositPM CreateInstance()
        {
            return new BankDepositPM()
            {
                Tenant = UserTenant.Tenant,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                AccountingDate = DateTime.Now,
                DepositDate = DateTime.Now,
                DepositCurrencyId = BillingData.CurrencyNISId,
                DepositBankAccountId = FullAccountingData.BankAccountId,
                IsCashDeposit = true,
                ForeignAmount = 100,
                LocalDepositAmount = 100,
                CashBookId = new CashBookPreparation().Create(),

            };
        }

        
    }
}
