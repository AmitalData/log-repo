using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentIntegrationVariables : EntityPM
    {
        public string AWBShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string CurrencyEURId { get; set; }
        public string IncotermLDEId { get; set; }
        public string MeasurementGRWTId { get; set; }
        public string ChargeGroupCOMMId { get; set; }
        public string ChargeGroupCOMMCode { get; set; }
        public string ChargeTypeAFTId { get; set; }
        public string PortLHRId { get; set; }
        public string PortLASId { get; set; }
        public string PortMIAId { get; set; }
        public string PortJFKId { get; set; }
        public string PortSOUId { get; set; }
        public string PortNYCId { get; set; }
        public string PortLONId { get; set; }
        public string PortMANId { get; set; }
        public string GlobalZoneEUId { get; set; }
        public string CountryGBId { get; set; }
        public string CountryUSId { get; set; }
        public string StateAKId { get; set; }
        public string AirlineAAId { get; set; }
        public string AirlineBAId { get; set; }
        public string ShippingLineMSCUId { get; set; }
        public string ShippingLineMAEUId { get; set; }
        public string MoveTypeMTAId { get; set; }
        public string MoveTypeMTOId { get; set; }
        public string VesselPTId { get; set; }
        public string PackageTypePC1Id { get; set; }
        public string PackageTypePC2Id { get; set; }
        public string PackageTypePP1Id { get; set; }
        public string PackageTypePP2Id { get; set; }
        public string PaymentTermCashId { get; set; }
        public string VATTypeZeroId { get; set; }
        public string QuoteStageQTDRId { get; set; }
        public string VendorId { get; set; }
        public string AgentId { get; set; }
        public string CustomerId { get; set; }
        public string PotentialCustomerId { get; set; }
        public string CustomAgentId { get; set; }
        public string ShippingAgentId { get; set; }
        public string WarehouseId { get; set; }
        public string TruckerId { get; set; }
        public string ShipperExport1 { get; set; }
        public string ShipmentId { get; internal set; }
        public string ConcurrencyGUID { get; set; }
        public List<PreparationShortClass> ChargesTypes { get; set; }
        public List<PreparationShortClass> VatTypes { get; set; }
        public List<PreparationShortClass> Currencies { get; set; }
        public List<PreparationShortClass> Rates { get; set; }
    }
}
