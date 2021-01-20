using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupShipmentPreparationVariables()
        {
            var shipmentVariables = APICaller.CallGet<ShipmentVariables>("IntegrationTest/GetBaseShipment", UserTenant.Token);
            ShipmentDataMap(shipmentVariables.Data);
        }

        private static void ShipmentDataMap(ShipmentVariables vars)
        {
            ShipmentData.CurrencyEURId = vars.CurrencyEURId;
            ShipmentData.IncotermLDEId = vars.IncotermLDEId;
            ShipmentData.ChargeGroupCOMMCode = vars.ChargeGroupCOMMCode;
            ShipmentData.ChargeGroupCOMMId = vars.ChargeGroupCOMMId;
            ShipmentData.ChargeTypeAFTId = vars.ChargeTypeAFTId;
            ShipmentData.VesselPTId = vars.VesselPTId;
            ShipmentData.PackageTypePC1Id = vars.PackageTypePC1Id;
            ShipmentData.PackageTypePC2Id = vars.PackageTypePC2Id;
            ShipmentData.PackageTypePP1Id = vars.PackageTypePP1Id;
            ShipmentData.PackageTypePP2Id = vars.PackageTypePP2Id;
            ShipmentData.PaymentTermCashId = vars.PaymentTermCashId;
            ShipmentData.VATTypeZeroId = vars.VATTypeZeroId;
            ShipmentData.QuoteStageQTDRId = vars.QuoteStageQTDRId;
            ShipmentData.ChargesTypes = vars.ChargesTypes;
            ShipmentData.VatTypes = vars.VatTypes;
            ShipmentData.Currencies = vars.Currencies;
            ShipmentData.Rates = vars.Rates;
        }
    }
}