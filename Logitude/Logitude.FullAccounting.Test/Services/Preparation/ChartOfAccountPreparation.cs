using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.FullAccounting.Test.Models.Codes;

namespace Logitude.FullAccounting.Test.Services.Preparation
{

    public class ChartOfAccountPreparation
    {
        
        public void Prepare()
        {
            FullAccountingData.DebtorsAndCreditorsChartOfAccountId = GetDebtorsAndCreditors();
            FullAccountingData.CustomerChartOfAccountId = GetCustomer();
            FullAccountingData.VendorChartOfAccountId = GetVendor();
            FullAccountingData.BankChartOfAccountId = GetBank();
            FullAccountingData.RevenuesChartOfAccountId = GetRevenues();
        }

        
        private string GetDebtorsAndCreditors()
        {
            return GetIdByCode(ChartOfAccountCodes.DebtorsAndCreditorsChart) ?? Create(CreateDebtorsAndCreditorsInstance());
        }

        private string GetCustomer()
        {
            return GetIdByCode(ChartOfAccountCodes.CustomerChart) ?? Create(CreateCustomersChartInstance());
        }

        private string GetVendor()
        {
            return GetIdByCode(ChartOfAccountCodes.VendorChart) ?? Create(CreateVendorChartInstance());
        }

        private string GetBank()
        {
            return GetIdByCode(ChartOfAccountCodes.BankChart) ?? Create(CreateBankChartInstance());
        }
        private string GetRevenues()
        {
            return GetIdByCode(ChartOfAccountCodes.RevenuesChart) ?? Create(CreateRevenuesChartInstance());
        }

        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<ChartOfAccountPM>>(Urls.ChartOfAccountViewsByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }
        private ApiQueryFilters GetFilterByCode(string code)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Value(code)
                .Build();
        }
        private string Create(ChartOfAccountPM chartOfAccountPM)
        {
            var response = APICaller.CallPost<ChartOfAccountPM>(chartOfAccountPM, Urls.ChartOfAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }
        
        private ChartOfAccountPM CreateDebtorsAndCreditorsInstance()
        {
            return new ChartOfAccountPM()
            {
                Code = ChartOfAccountCodes.DebtorsAndCreditorsChart,
                EnglishName = "Debtors And Creditors Chart",
                LocalName = "Debtors And Creditors Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChartOfAccountCodes.DebtorsAndCreditorsChart},Debtors And Creditors Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.DebtorsAndCreditors + ""

            };
        }
        private ChartOfAccountPM CreateCustomersChartInstance()
        {
            return new ChartOfAccountPM()
            {
                Code = ChartOfAccountCodes.CustomerChart,
                EnglishName = "Customer Chart",
                LocalName = "Customer Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChartOfAccountCodes.CustomerChart},Customer Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Customers + ""

            };
        }
        private ChartOfAccountPM CreateVendorChartInstance()
        {
            return new ChartOfAccountPM()
            {
                Code = ChartOfAccountCodes.VendorChart,
                EnglishName = "Vendors Chart",
                LocalName = "Vendors Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChartOfAccountCodes.VendorChart},Vendors Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Vendors + ""

            };
        }
        private ChartOfAccountPM CreateBankChartInstance()
        {
            return new ChartOfAccountPM()
            {
                Code = ChartOfAccountCodes.BankChart,
                EnglishName = "Banks Chart",
                LocalName = "Banks Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChartOfAccountCodes.BankChart},Banks Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Banks + ""

            };
        }
        private ChartOfAccountPM CreateRevenuesChartInstance()
        {
            return new ChartOfAccountPM()
            {
                Code = ChartOfAccountCodes.RevenuesChart,
                EnglishName = "Revenues Chart",
                LocalName = "Revenues Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChartOfAccountCodes.RevenuesChart},Revenues Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Revenues + ""

            };
        }

    }
}
