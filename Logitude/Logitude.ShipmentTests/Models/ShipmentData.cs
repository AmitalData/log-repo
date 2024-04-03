using System.Xml;

namespace Logitude.ShipmentTests.Models
{
    public static class ShipmentData
    {
        public static string PackageTypePC1Id { get; set; } 
        public static string PackageTypePC2Id { get; set; }
        public static string PackageTypePP1Id { get; set; }
        public static string PackageTypePP2Id { get; set; }
        public static string QuoteStageQTDRId { get; set; }
        public static string VesselPTId { get; set; }
        public static string MoveTypeMTAId { get; set; }
        public static string MoveTypeTSMId { get; set; }
        public static string MoveTypeMTOId { get; set; }
        public static string ShipmentSubTypeTSSTId { get; set; }
        public static ShipmentPM ShipmentPM { get; set; }
        public static XmlDocument XMLData { get; internal set; }
    }
}