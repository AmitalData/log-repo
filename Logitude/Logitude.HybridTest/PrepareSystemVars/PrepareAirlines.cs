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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PrepareAirlinesVars()
        {
            UpsertAirlineCodeHA();
            UpsertAirlineCodeHL();
        }
        private static void UpsertAirlineCodeHA()
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
            Response serviceResponse = AssertResponse(airlinePM);
        }
        private static void UpsertAirlineCodeHL()
        {
            AirlinePM airlinePM = new AirlinePM()
            {
                Code = HybridData.AirlineCodeHL,
                EnglishName = "Hybrid 2 Airlines",
                LocalName = "Hybrid 2 Airlines",
                CarrierTypeId = "AL",
                Prefix = "998",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(airlinePM);
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare Airlines Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare Airlines Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome.Response;
        }
    }
}
