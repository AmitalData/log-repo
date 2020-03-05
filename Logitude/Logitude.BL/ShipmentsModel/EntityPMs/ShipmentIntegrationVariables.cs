using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentIntegrationVariables
    {
        public static string AWBShipmentId { get; set; }
        public static string ShipmentNumber { get; set; }
        public string CurrencyEURId { get; set; }
        public string IncotermLDEId { get; set; }
        public string MeasurementGRWTId { get; set; }
        public string ChargeGroupCOMMId { get; set; }
        public static string ChargeGroupCOMMCode { get; set; }
        public static string ChargeTypeAFTId { get; set; }
        public static string PortLHRId { get; set; }
        public static string PortLASId { get; set; }
        public static string PortMIAId { get; set; }
        public static string PortJFKId { get; set; }
        public static string PortSOUId { get; set; }
        public static string PortNYCId { get; set; }
        public static string PortLONId { get; set; }
        public static string PortMANId { get; set; }
        public static string GlobalZoneEUId { get; set; }
        public static string CountryGBId { get; set; }
        public static string CountryUSId { get; set; }
        public static string StateAKId { get; set; }
        public static string AirlineAAId { get; set; }
        public static string AirlineBAId { get; set; }
        public static string ShippingLineMSCUId { get; set; }
        public static string ShippingLineMAEUId { get; set; }
        public static string MoveTypeMTAId { get; set; }
        public static string MoveTypeMTOId { get; set; }
        public static string VesselPTId { get; set; }
        public static string PackageTypePC1Id { get; set; }
        public static string PackageTypePC2Id { get; set; }
        public static string PackageTypePP1Id { get; set; }
        public static string PackageTypePP2Id { get; set; }
        public static string PaymentTermCashId { get; set; }
        public static string VATTypeZeroId { get; set; }
        public static string QuoteStageQTDRId { get; set; }
        public static string VendorId { get; set; }
        public static string AgentId { get; set; }
        public static string CustomerId { get; set; }
        public static string PotentialCustomerId { get; set; }
        public static string CustomAgentId { get; set; }
        public static string ShippingAgentId { get; set; }
        public static string WarehouseId { get; set; }
        public static string TruckerId { get; set; }
        public static string ShipperExport1 { get; set; }
        public static string ShipmentId { get; internal set; }
        public static string ConcurrencyGUID { get; set; }
    }
}
