using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using Logitude.IntegrationTest.Shipment.DataContexts;
using Logitude.IntegrationTest.Shipment.Tests;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment
{
    class ShipmentPreperationCalls
    {
        public static async Task PrepareVariables()
        {
            try
            {
                HttpResponseMessage response = await RestClientService.GetAsync("ShipmentIntegration/GetShipmentVars");
                ShipmentIntegrationVariables vars = RestClientService.ParseResponse<ShipmentIntegrationVariables>(response);
                VarsMap(vars);
            }

            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public static void VarsMap(ShipmentIntegrationVariables vars)
        {
            ShipmentVariables.CurrencyEURId = vars.CurrencyEURId;
            ShipmentVariables.IncotermLDEId = vars.IncotermLDEId;
            ShipmentVariables.MeasurmentGRWTId = vars.MeasurementGRWTId;

            ShipmentVariables.ChargeGroupCOMMCode = vars.ChargeGroupCOMMCode;
            ShipmentVariables.ChargeGroupCOMMId = vars.ChargeGroupCOMMId;

            ShipmentVariables.ChargeTypeAFTId = vars.ChargeTypeAFTId;

            ShipmentVariables.PortLHRId = vars.PortLHRId;
            ShipmentVariables.PortMIAId = vars.PortMIAId;
            ShipmentVariables.PortJFKId = vars.PortJFKId;
            ShipmentVariables.PortSOUId = vars.PortSOUId;
            ShipmentVariables.PortNYCId = vars.PortNYCId;
            ShipmentVariables.PortLONId = vars.PortLONId;
            ShipmentVariables.PortMANId = vars.PortMANId;
            ShipmentVariables.GlobalZoneEUId = vars.GlobalZoneEUId;
            ShipmentVariables.CountryGBId = vars.CountryGBId;
            ShipmentVariables.CountryUSId = vars.CountryUSId;
            ShipmentVariables.StateAKId = vars.StateAKId;
            ShipmentVariables.AirlineAAId = vars.AirlineAAId;
            ShipmentVariables.AirlineBAId = vars.AirlineBAId;
            ShipmentVariables.ShippingLineMSCUId = vars.ShippingLineMSCUId;
            ShipmentVariables.ShippingLineMAEUId = vars.ShippingLineMAEUId;
            ShipmentVariables.MoveTypeMTAId = vars.MoveTypeMTAId;
            ShipmentVariables.MoveTypeMTOId = vars.MoveTypeMTOId;
            ShipmentVariables.VesselPTId = vars.VesselPTId;
            ShipmentVariables.PackageTypePC1Id = vars.PackageTypePC1Id;
            ShipmentVariables.PackageTypePC2Id = vars.PackageTypePC2Id;
            ShipmentVariables.PackageTypePP1Id = vars.PackageTypePP1Id;
            ShipmentVariables.PackageTypePP2Id = vars.PackageTypePP2Id;
            ShipmentVariables.PaymentTermCashId = vars.PaymentTermCashId;
            ShipmentVariables.VATTypeZeroId = vars.VATTypeZeroId;
            ShipmentVariables.QuoteStageQTDRId = vars.QuoteStageQTDRId;
            ShipmentVariables.VendorId = vars.VendorId;
            ShipmentVariables.AgentId = vars.AgentId;
            ShipmentVariables.CustomerId = vars.CustomerId;
            ShipmentVariables.CustomAgentId = vars.CustomAgentId;
            ShipmentVariables.ShippingAgentId = vars.ShippingAgentId;
            ShipmentVariables.WarehouseId = vars.WarehouseId;
            ShipmentVariables.ShipperExport1 = vars.ShipperExport1;
            ShipmentVariables.ChargesTypes = vars.ChargesTypes;
            ShipmentVariables.VatTypes = vars.VatTypes;
            ShipmentVariables.Currencies = vars.Currencies;
            ShipmentVariables.Rates = vars.Rates;

            ChargesTypesDataContext.Instance.SetData(vars.ChargesTypes);
        }
    }
}
