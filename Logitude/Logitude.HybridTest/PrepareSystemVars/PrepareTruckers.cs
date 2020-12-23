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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
        public static void PrepareTruckersVars()
        {
            UpsertTruckerCodeHT();
            UpsertTruckerCodeHT2();
        }
        private static void UpsertTruckerCodeHT()
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
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "Hybrid City",
                VatNumber = "Vat 1152",
                CountryCode = HybridData.CountryCodeGB,
                StateCode = HybridData.StateCodeAK,
            });
            Response serviceResponse = AssertResponse(truckerPM);
        }
        private static void UpsertTruckerCodeHT2()
        {
            TruckerPM truckerPM = new TruckerPM()
            {
                Code = HybridData.TruckerCodeHT,
                EnglishName = "Hybrid 2 Trucker",
                LocalName = "Hybrid 2 Trucker",
                CarrierTypeId = "TR",
                AddedManually = true,
                TransportModeId = "I",
                Tenant = EnvironmentGlobalParams.MainTenant,
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
        }
        private static Response AssertResponse<T>(T entityPM)
        {
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(entityPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Prepare Truckers Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Prepare Truckers Vars Failed! " + serviceOutcome.Response.ErrorMessage);
            return serviceOutcome.Response;
        }
    }
}
