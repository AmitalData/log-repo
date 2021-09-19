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
            FullAccountingData.BankAccountId = GetByName(BankAccount1);
        }
        private string GetByName(string name)
        {
            var id = GetIdByName(name);
            if (string.IsNullOrEmpty(id))
            {
                return Create(name);
            }
            return id;
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

        public string Create(string name = null)
        {
            var bankCode = CreateInstance(name);
            var response = APICaller.CallPost<BankCodePM>(bankCode, Urls.BankAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }
        private BankAccountPM CreateInstance(string name)
        {
            switch (name)
            {
                case BankAccount1:
                    return CreateBankAccount1Instance(name);
                default:
                    return CreateAny();
            }
        }

        private BankAccountPM CreateBankAccount1Instance(string name)
        {
            return new BankAccountPM()
            {
                Tenant = UserTenant.Tenant,
                BankId = FullAccountingData.BankCodeId,
                Inactive = false,
                BranchNumber = DateTime.Now.Ticks.ToString().Substring(6),
                AccountNumber = DateTime.Now.Ticks.ToString().Substring(3),
                CurrencyId = BillingData.CurrencyNISId,
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = name

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
                GLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                TransferGLAcccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                DeferredGLAccountId = new AccountPreparation().Create(ChartOfAccountsTypeEnum.Banks),
                EnglishName = name

            };
        }
    }
}
