using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ShippingLineTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_ShippingLine_UPSERT()
        {
            ShippingLinePM shippingLinePM = new ShippingLinePM()
            {
                Code = HybridData.ShippingLineCodeHSL,
                SCACCode = HybridData.ShippingLineCodeHSL,
                EnglishName = "Hybrid ShippingLine changed",
                LocalName = "Hybrid ShippingLine changed",
                CarrierTypeId = "SL",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(shippingLinePM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
