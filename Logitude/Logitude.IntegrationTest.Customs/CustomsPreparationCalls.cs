using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Customs
{
    public class CustomsPreparationCalls
    {
        public static async Task PrepareVariables()
        {
            await GetChartOfAccountVendor1PMCF();

        }



        private static async Task GetChartOfAccountVendor1PMCF()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccountViews" + QueryFiltersPreparation.GetUrlParameters("1PMCF"));
            ChartOfAccountList chartOfAccountList = RestClientService.ParseResponse<ChartOfAccountList>(response);
            if (chartOfAccountList == null)
                await CreateChartOfAccountVendor1PMCF();
            else
            {
                CustomsVariables.ChartOfAccountVendor1PMCFId = chartOfAccountList.Id;
                CustomsVariables.ChartOfAccountVendor1PMCFCode = chartOfAccountList.Code;
            }
        }
        private static async Task CreateChartOfAccountVendor1PMCF()
        {
            ChartOfAccountPM ChartOfAccountVendorPM = GetNewChartOfAccountVendor1PMCF();
            HttpResponseMessage response = await RestClientService.PostAsync(ChartOfAccountVendorPM, "ChartOfAccounts");
            ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            CustomsVariables.ChartOfAccountVendor1PMCFId = chartOfAccountPM.Id;
            CustomsVariables.ChartOfAccountVendor1PMCFCode = chartOfAccountPM.Code;
        }
        private static ChartOfAccountPM GetNewChartOfAccountVendor1PMCF()
        {
            ChartOfAccountPM chartOfAccountPM = new ChartOfAccountPM();
            chartOfAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            chartOfAccountPM.EnglishName = "GE:Vendor";
            chartOfAccountPM.LocalName = "GE:Vendor";
            chartOfAccountPM.SearchFields = "1PMCF,GE:Vendor";
            chartOfAccountPM.Code = "1PMCF";
            chartOfAccountPM.TypeCode = "4";
            chartOfAccountPM.Inactive = true;
            return chartOfAccountPM;
        }
        private static async Task CreateGlAccountVendor458GLPML()
        {
            GLAccountPM GlAccountVendorPM = GetNewGlAccountVendorPM458GLPML();
            HttpResponseMessage response = await RestClientService.PostAsync(GlAccountVendorPM, "GLAccounts");
            GLAccountPM gLAccountPM = RestClientService.ParseResponse<GLAccountPM>(response);
            CustomsVariables.GLAccountVendor458GLPMId = gLAccountPM.Id;
        }
        private static GLAccountPM GetNewGlAccountVendorPM458GLPML()
        {
            GLAccountPM gLAccountPM = new GLAccountPM();
            gLAccountPM.Tenant = IntegrationTestLoginParameters.Tenant;
            gLAccountPM.EnglishName = "GE:Vendor";
            gLAccountPM.LocalName = "GE:Vendor";
            gLAccountPM.SearchFields = "458GLPML,GE:Vendor";
            gLAccountPM.AccountTypeCode = "3";
            gLAccountPM.DisplayNumber = "458GLPML";
            gLAccountPM.IsMultiCurrency = true;
            gLAccountPM.RevenueExpenseType = "3";
            gLAccountPM.ChartOfAccountsId = CustomsVariables.ChartOfAccountVendor1PMCFId;
            gLAccountPM.ChartOfAccountsTypeCode = "4";
            gLAccountPM.ReconcileMethodCode = "0";
            gLAccountPM.NewGLAccountCardId = CustomsVariables.VendorTestGlVendor1s5PMV2Id;
            return gLAccountPM;
        }
    }
}
