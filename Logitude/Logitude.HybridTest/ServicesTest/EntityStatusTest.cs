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
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityStatusPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
