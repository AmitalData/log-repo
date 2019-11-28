using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.CommonServices
{
    [TestClass]
    public class VendorTest
    {
        [TestMethod]
        public void Test_Vendor_UPSERT()
        {
            VendorPM vendorPM = new VendorPM()
            {
                Code = HybridData.VendorCode,
                EnglishName = "Hybrid Vendor",
                LocalName = "Hybrid Vendor",
                CityName = "Hybrid City",
                CountryCode = HybridData.CountryCode,
                PartnerTypeId = "VD",
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(vendorPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
