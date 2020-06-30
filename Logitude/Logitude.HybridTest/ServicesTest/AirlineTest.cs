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
        [TestMethod]
        public void Test_Airline_UPSERT()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHA,
                EnglishName = "Hybrid Airlines",
                LocalName = "Hybrid Airlines",
                CarrierTypeId = "AL",
                Prefix = "999",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(airlinePM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
