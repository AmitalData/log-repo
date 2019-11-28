using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class AgentTest
    {
        [TestMethod]
        public void Test_Agent_UPSERT()
        {
            AgentPM agentPM = new AgentPM()
            {
                Code = HybridData.AgentCode,
                EnglishName = "Hybrid Agent",
                LocalName = "Hybrid Agent",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                PartnerTypeId = "AG",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(agentPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
