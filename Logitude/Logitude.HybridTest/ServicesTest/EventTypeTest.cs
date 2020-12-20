using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class EventTypeTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_EventType_UPSERT()
        {
            EventTypePM eventTypePM = new EventTypePM()
            {
                Code = HybridData.EventTypeCodeHET,
                EnglishName = "Hybrid EventType",
                LocalName = "Hybrid EventType",
                ObjectTableName = "Shipment",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(eventTypePM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
