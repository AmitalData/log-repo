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
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(eventTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
