using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.LoginServiceReference;
using Logitude.HybridTest.WcfFactory;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.WcfCallers
{

    class PrepareTruckers
    {
        public static void PrepareTruckersVars()
        {
            UpsertTruckerIdHT();
            UpsertTruckerIdHT2();
        }
        private static void UpsertTruckerIdHT()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid Trucker",
                LocalName = "Hybrid Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            truckerPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                Name = "Hybrid City",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "Hybrid City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            Response serviceResponse = AssertResponse(truckerPM);
            HybridData.TruckerIdHT = serviceResponse.Result;
        }
        private static void UpsertTruckerIdHT2()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid 2 Trucker",
                LocalName = "Hybrid 2 Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            truckerPM.Addresses.Add(new AddressPM
            {
                AddressTypeId = "M",
                Description = "Main Address",
                Name = "Hybrid 2 City",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "Hybrid 2 City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            Response serviceResponse = AssertResponse(truckerPM);
            HybridData.TruckerIdHT2 = serviceResponse.Result;
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceResponse.HasError, "Prepare Truckers Vars Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Prepare Truckers Vars Failed! " + serviceResponse.ErrorMessage);
            return serviceResponse;
        }
    }
}
