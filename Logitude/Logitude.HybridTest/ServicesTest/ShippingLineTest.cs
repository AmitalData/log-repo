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
        public TestContext TestContext { get; set; }
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_ShippingLine_UPSERT()
        {
            try
            {
                RetryTest.InsertTestMethodToDictionary(TestContext.TestName);
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
            catch (Exception ex)
            {
                RetryTest.RetryFailTestRun(TestContext, this, ex.Message);
            }
        }
    }
}
