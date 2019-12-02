using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
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
          PreparationVariables prepVariables = new PreparationVariables();
          string urlparameters=QueryFiltersPreparation.Geturlparameters(prepVariables.ChartOfAccountCode);
            Task.Run(async () =>
            {
            HttpResponseMessage response = await RestClientService.GetAsync("chartofaccountviews"+ urlparameters);
                var stringResult = response.Content.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(stringResult);
                string resultArray = (string) jObject.SelectToken("Result")[0].ToString();
                ChartOfAccountList chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountList>(resultArray);
                Assert.AreEqual("1PMCF", chartOfAccount.Code);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ChartOfAccount_Post()
        {
            //PreparationVariables prepVariables = new PreparationVariables();
            //string urlparameters = QueryFiltersPreparation.Geturlparameters(prepVariables.ChartOfAccountCode);
            //Task.Run(async () =>
            //{


            //    HttpResponseMessage response = await RestClientService.PostAsync("ChartOfAccounts");
            //    var stringResult = response.Content.ReadAsStringAsync().Result;
            //    JObject jObject = JObject.Parse(stringResult);
            //    string resultArray = (string)jObject.SelectToken("Result")[0].ToString();
            //    ChartOfAccountList chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountList>(resultArray);

            //    Assert.AreEqual("1PMCF", chartOfAccount.Code);
            //}).GetAwaiter().GetResult();
        }
    }
}
