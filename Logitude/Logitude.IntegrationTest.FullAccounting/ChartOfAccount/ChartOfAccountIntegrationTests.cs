using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting.ChartOfAccount
{
    [TestClass]
    public class ChartOfAccountIntegrationTests
    {
        [TestMethod]
        public void ChartOfAccount_GetSingleById_Exists()
        {
            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/getsingle?id=1-1");
                var stringResult = response.Content.ReadAsStringAsync().Result;
                ChartOfAccountPM chartOfAccountPM = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
                Assert.AreEqual("1-1", chartOfAccountPM.Id);


            }).GetAwaiter().GetResult();
        }
    }
}
