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
    public class AccountPreparation
    {
        const string AccountNumber1 = "1921681253";
        const string AccountNumber2 = "1921681254";

        public void Prepare()
        {
            FullAccountingData.GLAccount1921681253Id = GetByNumber(AccountNumber1);
            FullAccountingData.GLAccount1921681254Id = GetByNumber(AccountNumber2);
        }
        public void PrepareNewAccount()
        {
            FullAccountingData.GLAccountId = Create();
        }

        private string Create()
        {
            var gLAccount = CreateInstance();
            var response = APICaller.CallPost<GLAccountPM>(gLAccount, Urls.GLAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private string GetByNumber(string number)
        {
            var id = GetIdByNumber(number);
            if (string.IsNullOrEmpty(id))
            {
                return Create(number);
            }
            return id;
        }
        private string GetIdByNumber(string number)
        {
            var filter = GetFilterByCode(number);
            var response = APICaller.CallGetByFilters<List<GLAccountPM>>(Urls.GlaccountviewsByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }
        private ApiQueryFilters GetFilterByCode(string number)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("DisplayNumber")
                .Filter1Value(number)
                .Build();
        }
        private string Create(string number)
        {
            var gLAccount = CreateInstance();
            var response = APICaller.CallPost<GLAccountPM>(gLAccount, Urls.GLAccountsController, UserTenant.Token);
            UpdateNameAndNumber(response.Data, number);
            response = APICaller.CallPut<GLAccountPM>(response.Data, Urls.GLAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private void UpdateNameAndNumber(GLAccountPM data, string number)
        {
            data.DisplayNumber = number;
            data.LocalName = number + " " + data.LocalName;
            data.EnglishName = data.LocalName;
        }

        private GLAccountPM CreateInstance()
        {
            return new GLAccountPM()
            {
                IsMultiCurrency = false,
                AccountTypeCode = (int)GLAccountTypeEnum.Card + "",
                CurrencyId = BillingData.CurrencyNISId,
                Tenant = UserTenant.Tenant,
                AutomaticReconcileId = FullAccountingData.AutomaticReconcile_A,
                ChartOfAccountsTypeCode = (int)ChartOfAccountsTypeEnum.DebtorsAndCreditors + "",
                ReconcileMethodCode = (int)ReconcileMethodEnum.Local + "",
                RevenueExpenseType = (int)RevenueExpenseTypeEnum.Revenue + "",
                ChartOfAccountsId = FullAccountingData.DebtorsAndCreditorsChartOfAccountId,
                LocalName = $"SpecFlow",
                EnglishName = $"SpecFlow",
            };
        }


    }
}
