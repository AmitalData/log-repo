using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareAirlines
    {
        public static void PrepareAirlinesVars()
        {
            UpsertAirlineIdHA();
            UpsertAirlineIdHL();
        }
        private static void UpsertAirlineIdHA()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHA,
                EnglishName = "Hybrid Airlines",
                LocalName = "Hybrid Airlines",
                CarrierTypeId = "AL",
                Prefix = "999",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = AssertResponse(airlinePM);
            HybridData.AirlineIdHA = serviceResponse.Result;
        }
        private static void UpsertAirlineIdHL()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHL,
                EnglishName = "Hybrid 2 Airlines",
                LocalName = "Hybrid 2 Airlines",
                CarrierTypeId = "AL",
                Prefix = "998",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = AssertResponse(airlinePM);
            HybridData.AirlineIdHL = serviceResponse.Result;
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare Airlines Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare Airlines Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
