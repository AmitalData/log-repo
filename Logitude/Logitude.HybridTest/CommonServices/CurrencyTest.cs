using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class CurrencyTest
    {
        [TestMethod]
        public void Test_Currency_UPSERT()
        {
            LoginService.GetLoginTokenByCredentials();
            Response serviceResponse = CurrencyWcfCaller.CallCurrencyUpsert();
            Assert.IsFalse(serviceResponse.HasError, serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }

        [TestMethod]
        public void Test_Currency_GETLIST()
        {
            LoginService.GetLoginTokenByCredentials();
            Response prepareResponse = CurrencyWcfCaller.PrepareCurrency();
            Assert.IsFalse(prepareResponse.HasError, "Prepare Currency Failed! " + prepareResponse.ErrorMessage);
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Currency",
                ServiceOperation = "GetList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(CurrencyList),
                ServiceFilterType = typeof(ApiSearchFilters),
            };
            ApiSearchFilters filters = new ApiSearchFilters
            {
                Take = 10,
                SearchFields = HybridData.CurrencyCode
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant, serviceResponse };
            CurrencyList[] currencies = (CurrencyList[]) WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsTrue(CheckResult(currencies), "Get Hybrid Currency Item From Currencies Failed!");
        }

        public bool CheckResult(CurrencyList[] currencies)
        {
            return currencies[0].EnglishName == "Hybrid Currency";
        }
    }
}
