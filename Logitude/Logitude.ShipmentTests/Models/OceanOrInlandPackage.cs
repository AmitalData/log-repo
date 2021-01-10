namespace Logitude.ShipmentTests.Models
{
    public class OceanOrInlandPackage
    {
        public PackageType PackageType { get; set; }
        public int? Pieces { get; set; }
        public double? GrossWeight { get; set; }
    }
}