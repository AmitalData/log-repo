using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class AgentTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Test_Agent_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
                AgentPM agentPM = new AgentPM()
                {
                    Code = HybridData.AgentCodeHAgent + "2",
                    EnglishName = "Hybrid Agent",
                    LocalName = "Hybrid Agent",
                    CityName = "Hybrid City",
                    CountryCode = HybridData.CountryCodeUS,
                    PartnerTypeId = "AG",
                    Tenant = EnvironmentGlobalParams.MainTenant,
                };
                ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(agentPM);
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
