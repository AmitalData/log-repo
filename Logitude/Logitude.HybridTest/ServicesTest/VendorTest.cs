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
                ComputedLocalName = "Hybrid Vendor",
                //CardPMId = HybridData.AgentCodeHAgent,
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
            ServiceOutcome serviceOutcome = EntityWcfCaller.CallEntityUpsert(vendorPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
