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
            var shipmentVariables = APICaller.CallGet<ShipmentVariables>("ShipmentIntegration/GetShipmentVars", UserTenant.Token);
            ShipmentDataMap(shipmentVariables.Data);
        }

        private static void ShipmentDataMap(ShipmentVariables vars)
        {
            ShipmentData.CurrencyEURId = vars.CurrencyEURId;
            ShipmentData.IncotermLDEId = vars.IncotermLDEId;
            ShipmentData.MeasurmentGRWTId = vars.MeasurementGRWTId;
            ShipmentData.ChargeGroupCOMMCode = vars.ChargeGroupCOMMCode;
            ShipmentData.ChargeGroupCOMMId = vars.ChargeGroupCOMMId;
            ShipmentData.ChargeTypeAFTId = vars.ChargeTypeAFTId;
            ShipmentData.PortLHRId = vars.PortLHRId;
            ShipmentData.PortMIAId = vars.PortMIAId;
            ShipmentData.PortJFKId = vars.PortJFKId;
            ShipmentData.PortSOUId = vars.PortSOUId;
            ShipmentData.PortNYCId = vars.PortNYCId;
            ShipmentData.PortLONId = vars.PortLONId;
            ShipmentData.PortMANId = vars.PortMANId;
            ShipmentData.GlobalZoneEUId = vars.GlobalZoneEUId;
            ShipmentData.CountryGBId = vars.CountryGBId;
            ShipmentData.CountryUSId = vars.CountryUSId;
            ShipmentData.StateAKId = vars.StateAKId;
            ShipmentData.AirlineAAId = vars.AirlineAAId;
            ShipmentData.AirlineBAId = vars.AirlineBAId;
            ShipmentData.ShippingLineMSCUId = vars.ShippingLineMSCUId;
            ShipmentData.ShippingLineMAEUId = vars.ShippingLineMAEUId;
            ShipmentData.MoveTypeMTAId = vars.MoveTypeMTAId;
            ShipmentData.MoveTypeMTOId = vars.MoveTypeMTOId;
            ShipmentData.VesselPTId = vars.VesselPTId;
            ShipmentData.PackageTypePC1Id = vars.PackageTypePC1Id;
            ShipmentData.PackageTypePC2Id = vars.PackageTypePC2Id;
            ShipmentData.PackageTypePP1Id = vars.PackageTypePP1Id;
            ShipmentData.PackageTypePP2Id = vars.PackageTypePP2Id;
            ShipmentData.PaymentTermCashId = vars.PaymentTermCashId;
            ShipmentData.VATTypeZeroId = vars.VATTypeZeroId;
            ShipmentData.QuoteStageQTDRId = vars.QuoteStageQTDRId;
            ShipmentData.VendorId = vars.VendorId;
            ShipmentData.AgentId = vars.AgentId;
            ShipmentData.CustomerId = vars.CustomerId;
            ShipmentData.CustomAgentId = vars.CustomAgentId;
            ShipmentData.ShippingAgentId = vars.ShippingAgentId;
            ShipmentData.WarehouseId = vars.WarehouseId;
            ShipmentData.ShipperExport1 = vars.ShipperExport1;
            ShipmentData.ChargesTypes = vars.ChargesTypes;
            ShipmentData.VatTypes = vars.VatTypes;
            ShipmentData.Currencies = vars.Currencies;
            ShipmentData.Rates = vars.Rates;
        }
    }
}
