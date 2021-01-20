using System.Collections.Generic;
using Logitude.BL.ShipmentsModel.EntityQueries;


namespace Logitude.BL.IntegrationTestModel.EntityPMs
{
    public class ShipmentVariables
    {
        public string AWBShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string CurrencyEURId { get; set; }
        public string IncotermLDEId { get; set; }
        public string MeasurementGRWTId { get; set; }
        public string ChargeGroupCOMMId { get; set; }
        public string ChargeGroupCOMMCode { get; set; }
        public string ChargeTypeAFTId { get; set; }
        public string PackageTypePC1Id { get; set; }
        public string PackageTypePC2Id { get; set; }
        public string PackageTypePP1Id { get; set; }
        public string PackageTypePP2Id { get; set; }
        public string PaymentTermCashId { get; set; }
        public string VATTypeZeroId { get; set; }
        public string QuoteStageQTDRId { get; set; }
        public string VesselPTId { get; set; }
        public string MoveTypeMTAId { get; set; }
        public string MoveTypeMTOId { get; set; }
        public string ShipmentId { get; internal set; }
        public string ConcurrencyGUID { get; set; }
        public List<PreparationShortClass> ChargesTypes { get; set; }
        public List<PreparationShortClass> VatTypes { get; set; }
        public List<PreparationShortClass> Currencies { get; set; }
        public List<PreparationShortClass> Rates { get; set; }

    }
}
