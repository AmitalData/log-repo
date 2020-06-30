using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class EntityStatusTest
    {
        [TestMethod]
        public void Test_EntityStatus_UPSERT()
        {
            EntityStatusPM entityStatusPM = new EntityStatusPM()
            {
                Code = HybridData.EntityStatusCodeHES,
                Name = "Hybrid EntityStatus",
                DisplayName = "Hybrid EntityStatus",
                InActive = false,
                ObjectTableName = "Shipment",
                StatusWeight = 0,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(entityStatusPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
