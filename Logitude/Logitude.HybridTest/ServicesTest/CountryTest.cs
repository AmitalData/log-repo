using System;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CountryTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Country_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName); 
                CountryPM countryPM = new CountryPM()
                {
                    Code = HybridData.CountryCodeHC,
                    EnglishName = "Hybrid Country",
                    LocalName = "Hybrid Country",
                    GlobalZoneId = HybridData.GlobalZoneCodeHZ,
                    AddedManually = true,
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(countryPM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
            
         }

        [TestMethod]
        public void Test_Country_GETLIST()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                InvokedProperties serviceProperties = new InvokedProperties
                {
                    ServiceName = "Country",
                    ServiceOperation = "GetList",
                    ServiceResponseIndex = 2,
                    ServiceType = typeof(CountryList),
                    ServiceFilterType = typeof(ApiSearchFilters),
                };
                ApiSearchFilters filters = new ApiSearchFilters
                {
                    Take = 10,
                    SearchFields = HybridData.CountryCodeUS
                };
                Response serviceResponse = new Response();
                object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
                ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

                CountryList[] countries = (CountryList[])serviceOutcome.Result;
                Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.AreEqual(countries[0].Code, HybridData.CountryCodeUS, "Get Hybrid Country Item From Countries Failed!");
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
