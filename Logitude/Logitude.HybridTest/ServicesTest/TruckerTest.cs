using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class TruckerTest
    {
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        [TestMethod]
        public void Test_Trucker_UPSERT()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid Trucker",
                LocalName = "Hybrid Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            truckerPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                Name = "Hybrid City",
                Address1 = "Address change 1",
                Address2 = "Address change 2",
                City = "Hybrid City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(truckerPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }
    }
}
