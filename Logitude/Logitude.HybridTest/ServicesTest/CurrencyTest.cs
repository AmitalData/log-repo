using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CurrencyTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Currency_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                CurrencyPM currencyPM = new CurrencyPM()
                {
                    Code = HybridData.CurrencyCodeHCR,
                    EnglishName = "Hybrid Currency",
                    LocalName = "Hybrid Currency",
                    AddedManually = true,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(currencyPM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }

        [TestMethod]
        public void Test_Currency_GETLIST()
        {
            //Test_Currency_UPSERT();
            //InvokedProperties serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "Currency",
            //    ServiceOperation = "GetList",
            //    ServiceResponseIndex = 2,
            //    ServiceType = typeof(CurrencyList),
            //    ServiceFilterType = typeof(ApiSearchFilters),
            //};
            //ApiSearchFilters filters = new ApiSearchFilters
            //{
            //    Take = 10,
            //    SearchFields = HybridData.CurrencyCodeEUR
            //};
            //Response serviceResponse = new Response();
            //object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            //ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            //CurrencyList[] currencies = (CurrencyList[])serviceOutcome.Result;
            //Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            //Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            //Assert.AreEqual(currencies[0].Code, HybridData.CurrencyCodeEUR, "Get Hybrid Currency Item From Currencies Failed!");
        }
    }
}
