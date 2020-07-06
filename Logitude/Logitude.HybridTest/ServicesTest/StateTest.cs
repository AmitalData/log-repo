using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void Test_State_UPSERT()
        {
               StatePM statePM = new StatePM()
            {
                Code = HybridData.StateCodeAK,
                EnglishName = "Alaska",
                LocalName = "Alaska",
                CountryId = HybridData.CountryCodeUS,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(statePM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
