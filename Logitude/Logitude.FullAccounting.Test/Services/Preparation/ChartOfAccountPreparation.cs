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

namespace Logitude.FullAccounting.Test.Services.Preparation
{

    public class ChartOfAccountPreparation
    {
        const string DebtorsAndCreditorsChartCode = "DCCHR";
        const string CustomerChartCode = "CCCHR";
        const string VendorChartCode = "VNCHR";
        public void Prepare()
        {
            FullAccountingData.DebtorsAndCreditorsChartOfAccountId = GetByCode(DebtorsAndCreditorsChartCode);
            FullAccountingData.CustomerChartOfAccountId = GetByCode(CustomerChartCode);
            FullAccountingData.VendorChartOfAccountId = GetByCode(VendorChartCode);
        }
        private string GetByCode(string code)
        {
            var id = GetIdByCode(code);
            if (string.IsNullOrEmpty(id))
            {
                return Create(code);
            }
            return id;
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
        private string Create(string code)
        {
            var ChartOfAccountPM = CreateInstance(code);
            var response = APICaller.CallPost<ChartOfAccountPM>(ChartOfAccountPM, Urls.ChartOfAccountsController, UserTenant.Token);
            return response.Data?.Id;
        }
        private ChartOfAccountPM CreateInstance(string code)
        {
            switch (code)
            {
                case DebtorsAndCreditorsChartCode:
                    return CreateRCHARInstance(code);
                case CustomerChartCode:
                    return CreateCustomersChartCodeInstance(code);
                case VendorChartCode:
                    return CreateVendorChartCodeInstance(code);

                default:
                    return null;
            }
        }
        private ChartOfAccountPM CreateRCHARInstance(string code)
        {
            return new ChartOfAccountPM()
            {
                Code = code,
                EnglishName = "Debtors And Creditors Chart",
                LocalName = "Debtors And Creditors Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Debtors And Creditors Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.DebtorsAndCreditors + ""

            };
        }
        private ChartOfAccountPM CreateCustomersChartCodeInstance(string code)
        {
            return new ChartOfAccountPM()
            {
                Code = code,
                EnglishName = "Customer Chart",
                LocalName = "Customer Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Customer Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Customers + ""

            };
        }
        private ChartOfAccountPM CreateVendorChartCodeInstance(string code)
        {
            return new ChartOfAccountPM()
            {
                Code = code,
                EnglishName = "Vendors Chart",
                LocalName = "Vendors Chart",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Vendors Chart",
                TypeCode = (int)ChartOfAccountsTypeEnum.Vendors + ""

            };
        }

    }
}
