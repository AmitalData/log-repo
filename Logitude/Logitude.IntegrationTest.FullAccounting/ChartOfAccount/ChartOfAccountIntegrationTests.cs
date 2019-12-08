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

namespace Logitude.IntegrationTest.FullAccounting.ChartOfAccount
{
    [TestClass]
    public class ChartOfAccountIntegrationTests
    {
        [TestMethod]
        public void ChartOfAccount_GetSingleByCode_Exists()
        {
            string urlparameters = QueryFiltersPreparation.GetUrlParameters(PreparationVariables.ChartOfAccountCode);
            Task.Run(async () =>
            {
                ChartOfAccountList chartOfAccount = await PreparationCalls.GetSingleChartOfAccount();
                Assert.AreEqual(PreparationVariables.ChartOfAccountCode, chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ChartOfAccount_Post()
        {
            Task.Run(async () =>
            {
                ChartOfAccountPM entityPM = GetNewChartOfAccountPM();
                HttpResponseMessage response = await RestClientService.PostAsync(entityPM,"ChartOfAccounts");
                var stringResult = response.Content.ReadAsStringAsync().Result;
                ChartOfAccountPM chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountPM>(stringResult);
                Assert.AreEqual(entityPM.Code, chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }

        private ChartOfAccountPM GetNewChartOfAccountPM()
        {
            ChartOfAccountPM entityPM = new ChartOfAccountPM();
            string Code = RandomString(5);//"ME188";
            entityPM.Inactive = true;
            entityPM.TypeCode = "4";
            entityPM.LocalName = Code;
            entityPM.EnglishName = Code;
            entityPM.Code = Code;
            entityPM.Tenant = IntegrationTestLoginParameters.Tenant;
            return entityPM;
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
