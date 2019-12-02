using System;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class SpecialServicesTypeTest
    {
        [TestMethod]
        public void Test_SpecialServicesType_UPSERT()
        {
            SpecialServicesTypePM specialServicesTypePM = new SpecialServicesTypePM()
            {
                Code = HybridData.SpecialServicesTypeCodeHSST,
                EnglishName = "Hybrid SpecialServicesType",
                LocalName = "Hybrid SpecialServicesType",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(specialServicesTypePM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
