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
            ApiResponse<ShipmentVariables> shipmentVariablesResponse = APICaller.CallGet<ShipmentVariables>(Urls.IntegrationTestGetBaseShipment(), UserTenant.Token);
            ShipmentDataMap(shipmentVariablesResponse.Data);
        }

        private static void ShipmentDataMap(ShipmentVariables shipmentVariables)
        {
            ShipmentData.CurrencyEURId = shipmentVariables.CurrencyEURId;
            ShipmentData.IncotermLDEId = shipmentVariables.IncotermLDEId;
            ShipmentData.ChargeGroupCOMMCode = shipmentVariables.ChargeGroupCOMMCode;
            ShipmentData.ChargeGroupCOMMId = shipmentVariables.ChargeGroupCOMMId;
            ShipmentData.ChargeTypeAFTId = shipmentVariables.ChargeTypeAFTId;
            ShipmentData.VesselPTId = shipmentVariables.VesselPTId;
            ShipmentData.PackageTypePC1Id = shipmentVariables.PackageTypePC1Id;
            ShipmentData.PackageTypePC2Id = shipmentVariables.PackageTypePC2Id;
            ShipmentData.PackageTypePP1Id = shipmentVariables.PackageTypePP1Id;
            ShipmentData.PackageTypePP2Id = shipmentVariables.PackageTypePP2Id;
            ShipmentData.PaymentTermCashId = shipmentVariables.PaymentTermCashId;
            ShipmentData.VATTypeZeroId = shipmentVariables.VATTypeZeroId;
            ShipmentData.QuoteStageQTDRId = shipmentVariables.QuoteStageQTDRId;
            ShipmentData.MoveTypeMTAId = shipmentVariables.MoveTypeMTAId;
            ShipmentData.MoveTypeMTOId = shipmentVariables.MoveTypeMTOId;
            ShipmentData.ChargesTypes = shipmentVariables.ChargesTypes;
            ShipmentData.VatTypes = shipmentVariables.VatTypes;
            ShipmentData.Currencies = shipmentVariables.Currencies;
            ShipmentData.Rates = shipmentVariables.Rates;
        }
    }
}