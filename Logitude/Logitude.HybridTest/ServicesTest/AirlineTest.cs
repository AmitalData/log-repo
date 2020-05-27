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
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(airlinePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
