using System.Collections.Generic;

namespace Logitude.ShipmentTests.Models
{
    public class ShipmentData
    {




        public static string AWBShipmentId { get; set; }
        public static string ShipmentNumber { get; set; }
        public static string CurrencyEURId { get; set; }
        public static string IncotermLDEId { get; set; }
        public static string MeasurementGRWTId { get; set; }
        public static string ChargeGroupCOMMId { get; set; }
        public static string ChargeGroupCOMMCode { get; set; }
        public static string ChargeTypeAFTId { get; set; }
        public static string PackageTypePC1Id { get; set; }
        public static string PackageTypePC2Id { get; set; }
        public static string PackageTypePP1Id { get; set; }
        public static string PackageTypePP2Id { get; set; }
        public static string PaymentTermCashId { get; set; }
        public static string VATTypeZeroId { get; set; }
        public static string QuoteStageQTDRId { get; set; }
        public static string VesselPTId { get; set; }
        public static string ShipmentId { get; internal set; }
        public static string ConcurrencyGUID { get; set; }
        public static string MoveTypeMTAId { get; set; }
        public static string MoveTypeMTOId { get; set; }
        public static List<PreparationShortClass> ChargesTypes { get; set; }
        public static List<PreparationShortClass> VatTypes { get; set; }
        public static List<PreparationShortClass> Currencies { get; set; }
        public static List<PreparationShortClass> Rates { get; set; }

    }
}
