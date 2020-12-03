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
    public class GetChartOfAccount
    {
         
        [TestMethod]
        public async Task GetChartOfAccount_Get_Successful()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/GetSingle?id=" + FullAccountingVariables.ChartOfAccountVendor1PMCFId);
            ChartOfAccountPM chartOfAccountPM = RestClientService.ParseResponse<ChartOfAccountPM>(response);
            Assert.IsNotNull(chartOfAccountPM);
            Assert.AreEqual(chartOfAccountPM.Id, FullAccountingVariables.ChartOfAccountVendor1PMCFId);
        }
        
    }
}
