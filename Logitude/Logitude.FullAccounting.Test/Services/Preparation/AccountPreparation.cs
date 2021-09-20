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


        public void Prepare()
        {
            FullAccountingData.GLAccount1Id = Create();
            FullAccountingData.GLAccount2Id = Create();
        }
        public void PrepareNewAccount()
        {
            FullAccountingData.GLAccountId = Create();
        }
        public void PrepareAccounts()
        {
            FullAccountingData.GLAccountRevenuesId = Create(ChartOfAccountsTypeEnum.Revenues);
            FullAccountingData.GLAccountExpensesId = Create(ChartOfAccountsTypeEnum.Expenses);
        }

        public string Create(ChartOfAccountsTypeEnum ChartOfAccountsTypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors)
        {
            var gLAccount = CreateInstance(ChartOfAccountsTypeCode);
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

        private GLAccountPM CreateInstance(ChartOfAccountsTypeEnum chartOfAccountsTypeCode = ChartOfAccountsTypeEnum.DebtorsAndCreditors)
        {
            //ChartOfAccountsId = FullAccountingData.DebtorsAndCreditorsChartOfAccountId,
            var chartOfAccountsId = GetChartOfAccountsId(chartOfAccountsTypeCode);
            return new GLAccountPM()
            {
                IsMultiCurrency = false,
                AccountTypeCode = (int)GLAccountTypeEnum.Card + "",
                CurrencyId = BillingData.CurrencyNISId,
                Tenant = UserTenant.Tenant,
                AutomaticReconcileId = FullAccountingData.AutomaticReconcile_A,
                ChartOfAccountsTypeCode = (int)chartOfAccountsTypeCode + "",
                ReconcileMethodCode = (int)ReconcileMethodEnum.Local + "",
                RevenueExpenseType = (int)RevenueExpenseTypeEnum.Revenue + "",
                ChartOfAccountsId = chartOfAccountsId,
                LocalName = $"SpecFlow",
                EnglishName = $"SpecFlow",
            };
        }

        private string GetChartOfAccountsId(ChartOfAccountsTypeEnum chartOfAccountsTypeCode)
        {
            switch (chartOfAccountsTypeCode)
            {
                case ChartOfAccountsTypeEnum.Customers:
                    return FullAccountingData.CustomerChartOfAccountId;
                case ChartOfAccountsTypeEnum.Vendors:
                    return FullAccountingData.VendorChartOfAccountId;
                case ChartOfAccountsTypeEnum.Banks:
                    return FullAccountingData.BankChartOfAccountId;
                case ChartOfAccountsTypeEnum.DebtorsAndCreditors:
                    return FullAccountingData.DebtorsAndCreditorsChartOfAccountId;

                case ChartOfAccountsTypeEnum.Revenues:
                case ChartOfAccountsTypeEnum.Expenses:
                case ChartOfAccountsTypeEnum.Works:
                default:
                    throw new NotImplementedException("Chart Of Accounts Type Code: "+ chartOfAccountsTypeCode+ " Not Implemented");
            }
        }
    }
}
