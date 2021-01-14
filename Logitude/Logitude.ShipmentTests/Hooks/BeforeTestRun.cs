using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.TestData;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.TestData;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupShipmentPreparationVariables()
        {
            ShipmentVariables shipmentVariables = APICaller.CallGet<ShipmentVariables>("ShipmentIntegration/GetShipmentVars", LoginPreparationParameters.Token, null);
            ShipmentTestDataMap(shipmentVariables);
        }

        private static void ShipmentTestDataMap(ShipmentVariables vars)
        {
            ShipmentTestData.CurrencyEURId = vars.CurrencyEURId;
            ShipmentTestData.IncotermLDEId = vars.IncotermLDEId;
            ShipmentTestData.MeasurmentGRWTId = vars.MeasurementGRWTId;
            ShipmentTestData.ChargeGroupCOMMCode = vars.ChargeGroupCOMMCode;
            ShipmentTestData.ChargeGroupCOMMId = vars.ChargeGroupCOMMId;
            ShipmentTestData.ChargeTypeAFTId = vars.ChargeTypeAFTId;
            ShipmentTestData.PortLHRId = vars.PortLHRId;
            ShipmentTestData.PortMIAId = vars.PortMIAId;
            ShipmentTestData.PortJFKId = vars.PortJFKId;
            ShipmentTestData.PortSOUId = vars.PortSOUId;
            ShipmentTestData.PortNYCId = vars.PortNYCId;
            ShipmentTestData.PortLONId = vars.PortLONId;
            ShipmentTestData.PortMANId = vars.PortMANId;
            ShipmentTestData.GlobalZoneEUId = vars.GlobalZoneEUId;
            ShipmentTestData.CountryGBId = vars.CountryGBId;
            ShipmentTestData.CountryUSId = vars.CountryUSId;
            ShipmentTestData.StateAKId = vars.StateAKId;
            ShipmentTestData.AirlineAAId = vars.AirlineAAId;
            ShipmentTestData.AirlineBAId = vars.AirlineBAId;
            ShipmentTestData.ShippingLineMSCUId = vars.ShippingLineMSCUId;
            ShipmentTestData.ShippingLineMAEUId = vars.ShippingLineMAEUId;
            ShipmentTestData.MoveTypeMTAId = vars.MoveTypeMTAId;
            ShipmentTestData.MoveTypeMTOId = vars.MoveTypeMTOId;
            ShipmentTestData.VesselPTId = vars.VesselPTId;
            ShipmentTestData.PackageTypePC1Id = vars.PackageTypePC1Id;
            ShipmentTestData.PackageTypePC2Id = vars.PackageTypePC2Id;
            ShipmentTestData.PackageTypePP1Id = vars.PackageTypePP1Id;
            ShipmentTestData.PackageTypePP2Id = vars.PackageTypePP2Id;
            ShipmentTestData.PaymentTermCashId = vars.PaymentTermCashId;
            ShipmentTestData.VATTypeZeroId = vars.VATTypeZeroId;
            ShipmentTestData.QuoteStageQTDRId = vars.QuoteStageQTDRId;
            ShipmentTestData.VendorId = vars.VendorId;
            ShipmentTestData.AgentId = vars.AgentId;
            ShipmentTestData.CustomerId = vars.CustomerId;
            ShipmentTestData.CustomAgentId = vars.CustomAgentId;
            ShipmentTestData.ShippingAgentId = vars.ShippingAgentId;
            ShipmentTestData.WarehouseId = vars.WarehouseId;
            ShipmentTestData.ShipperExport1 = vars.ShipperExport1;
            ShipmentTestData.ChargesTypes = vars.ChargesTypes;
            ShipmentTestData.VatTypes = vars.VatTypes;
            ShipmentTestData.Currencies = vars.Currencies;
            ShipmentTestData.Rates = vars.Rates;
        }
    }
}
