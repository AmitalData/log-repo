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

namespace Logitude.IntegrationTest.FullAccounting.Tests.ChartofAccount
{
    [TestClass]
    public class UpdateChartOfAccount
    {
        [TestMethod]
        public async Task UpdateChartOfAccount_Put_Successful()
        {
                ChartOfAccountPM entityPM = await GetSingle();
                entityPM.LocalName = "GE:" + VariablesGenerater.GetRandomString(5);
                entityPM.EnglishName = "GE:" + VariablesGenerater.GetRandomString(5);
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "ChartOfAccounts");
                ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
                Assert.AreEqual(entityPM.Id, chartOfAccountPM.Id);
        }
        [TestMethod]
      
        private async Task<ChartOfAccountPM> GetSingle()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/GetSingle?id="+ FullAccountingVariables.ChartOfAccountVendor1PMCFId);
            ChartOfAccountPM chartOfAccountPM= RestClientService.ParseResponse<ChartOfAccountPM>(response);
            return chartOfAccountPM;
        }
    }
}
