using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.FullAccounting.Preparation;
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
        public void ChartOfAccount_GetSingleByCode_Exists()
        {
            string urlparameters = QueryFiltersPreparation.GetUrlParameters(FullAccountingVariables.ChartOfAccountCode);
            Task.Run(async () =>
            {
                ChartOfAccountList chartOfAccount = await PreparationCalls.GetSingleChartOfAccountByCode();
                Assert.AreEqual(FullAccountingVariables.ChartOfAccountCode, chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void ChartOfAccount_GetSingle()
        {
            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync("ChartOfAccounts/GetSingle?id="+ FullAccountingVariables.ChartOfAccountId);
                var stringResult = response.Content.ReadAsStringAsync().Result;
                ChartOfAccountPM chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
                Assert.AreEqual(FullAccountingVariables.ChartOfAccountCode, chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ChartOfAccount_Put()
        {
            Task.Run(async () =>
            {
                ChartOfAccountPM entityPM = await PreparationCalls.GetSingleChartOfAccount();
                entityPM.LocalName ="GE:"+RandomString(5);
                entityPM.EnglishName = "GE:" + RandomString(5);
                HttpResponseMessage response = await RestClientService.PutAsync(entityPM, "ChartOfAccounts");
                var stringResult = response.Content.ReadAsStringAsync().Result;
                ChartOfAccountPM chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
                Assert.AreEqual(entityPM.Code, chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
