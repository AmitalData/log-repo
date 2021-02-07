using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class AirlineTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Airline_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                AirlinePM airlinePM = new AirlinePM()
                {
                    Code = HybridData.AirlineCodeHA,
                    EnglishName = "Hybrid Airlines",
                    LocalName = "Hybrid Airlines",
                    CarrierTypeId = "AL",
                    Prefix = "999",
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(airlinePM);
                Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
                Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            }
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
