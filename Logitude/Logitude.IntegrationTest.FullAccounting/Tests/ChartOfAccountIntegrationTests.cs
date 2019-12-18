using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting;
using Logitude.Server.Tools.Counters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting.Tests
{
    [TestClass]
    public class ChartOfAccountIntegrationTests
    {
        [TestMethod]
        public async Task ChartOfAccount_Vendor1PMCFId_Put()
        {
            
                ChartOfAccountPM entityPM = await ChartOfAccount_Vendor1PMCFId_GetSingle();
                entityPM.LocalName = "GE:" + RestClientService.GetRandomString(5);
                entityPM.EnglishName = "GE:" + RestClientService.GetRandomString(5);
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "ChartOfAccounts");
                ChartOfAccountPM chartOfAccountVendor1PMCFId = RestClientService.ParseResponse<ChartOfAccountPM>(response);
                Assert.AreEqual(entityPM.Code, chartOfAccountVendor1PMCFId.Code);
           
        }
        private async Task<ChartOfAccountPM> ChartOfAccount_Vendor1PMCFId_GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/GetSingle?id="+ FullAccountingVariables.ChartOfAccountVendor1PMCFId);
            ChartOfAccountPM chartOfAccountVendor1PMCFId = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            return chartOfAccountVendor1PMCFId;
        }
    }
}
