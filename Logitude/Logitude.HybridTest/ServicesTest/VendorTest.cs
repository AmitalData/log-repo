using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class VendorTest
    {
        [TestMethod]
        public void Test_Vendor_UPSERT()
        {
            VendorPM vendorPM = new VendorPM()
            {
                Code = HybridData.VendorCodeHVEN,
                EnglishName = "Hybrid Vendor",
                LocalName = "Hybrid Vendor",
                CountryCode = HybridData.CountryCodeUS,
                PartnerTypeId = "VD",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            vendorPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                City = "New York",
                CountryCode = HybridData.CountryCodeUS,
                CardCode = "new",
            });
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(vendorPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
        }
    }
}
