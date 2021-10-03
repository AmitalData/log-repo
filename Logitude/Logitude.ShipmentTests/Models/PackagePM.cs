using System;

namespace Logitude.ShipmentTests.Models
{
    public class PackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Weight { get; set; }
        public double? Width { get; set; }
        public int? Quantity { get; set; }
        public string ShipmentId { get; set; }
        public string PackageTypeId { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerEntityId { get; set; }
        public string LastStatusCode { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public string FlashPointTemperatureUnitCode { get; set; }
        public string TemperatureUnitCode { get; set; }
        public string ShipmentNumber { get; set; }
        public int ChangeSetOp { get; set; }
    }
}