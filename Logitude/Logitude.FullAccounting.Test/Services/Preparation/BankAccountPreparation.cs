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
    public class BankAccountPreparation
    {
        const string BankAccount1 = "BankAccountGeneratedSP";
        public void Prepare()
        {
            FullAccountingData.BankAccountId = GetBankAccount1();
        }

        private string GetBankAccount1()
        {
            return GetIdByName(BankAccount1) ?? Create(CreateBankAccount1Instance());
        }
        public string GetNewBankAccount()
        {
            return  Create(CreateAny());
        }

        private string GetIdByName(string name)
        {
            var filter = GetFilterByName(name);
            var response = APICaller.CallGetByFilters<List<BankAccountPM>>(Urls.BankAccountViewsGetByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private ApiQueryFilters GetFilterByName(string name)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("EnglishName")
                .Filter1Value(name)
                .Build();
        }

        private string Create(BankAccountPM bankAccount)
        {
            var response = APICaller.CallPost<BankAccountPM>(bankAccount, Urls.BankAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }


        private BankAccountPM CreateBankAccount1Instance()
        {
            return new BankAccountPM()
            {
                Tenant = UserTenant.Tenant,
                BankId = FullAccountingData.BankCodeId,
                Inactive = false,
                BranchNumber = DateTime.Now.Ticks.ToString().Substring(6),
                AccountNumber = DateTime.Now.Ticks.ToString().Substring(3),
                CurrencyId = BillingData.CurrencyNISId,
                ChequeCounter = 1,
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = BankAccount1,
                LocalName = BankAccount1

            };
        }

        private BankAccountPM CreateAny()
        {
            string name = "BA" + DateTime.Now.Ticks;
            return new BankAccountPM()
            {
                Tenant = UserTenant.Tenant,
                BankId = FullAccountingData.BankCodeId,
                Inactive = false,
                BranchNumber = DateTime.Now.Ticks.ToString().Substring(6),
                AccountNumber = DateTime.Now.Ticks.ToString().Substring(3),
                CurrencyId = BillingData.CurrencyNISId,
                ChequeCounter = 1,
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = name,
                LocalName = name

            };
        }
    }
}
