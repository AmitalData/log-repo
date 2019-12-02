using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class VesselTest
    {
        [TestMethod]
        public void Test_Vessel_UPSERT()
        {
            VesselPM vesselPM = new VesselPM()
            {
                Code = HybridData.VesselCodeHV,
                EnglishName = "Hybrid Vessel",
                LocalName = "Hybrid Vessel",
                IMOCode = "IMOCode HV",
                AddedManually = true,
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(vesselPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
