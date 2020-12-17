using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareShippingLines
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PrepareShippingLinesVars()
        {
            UpsertShippingLineCodeHSL();
            UpsertShippingLineCodeHSL2();
        }
        private static void UpsertShippingLineCodeHSL()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCodeHSL,
                SCACCode = HybridData.ShippingLineCodeHSL,
                EnglishName = "Hybrid ShippingLine",
                LocalName = "Hybrid ShippingLine",
                CarrierTypeId = "SL",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(shippingLinePM);
        }
        private static void UpsertShippingLineCodeHSL2()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCodeHSL2,
                SCACCode = HybridData.ShippingLineCodeHSL2,
                EnglishName = "Hybrid 2 ShippingLine",
                LocalName = "Hybrid 2 ShippingLine",
                CarrierTypeId = "SL",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = AssertResponse(shippingLinePM);
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare ShippingLines Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare ShippingLines Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome.Response;
        }
    }
}
