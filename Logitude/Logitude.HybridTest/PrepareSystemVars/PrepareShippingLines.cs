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
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare ShippingLines Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare ShippingLines Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
