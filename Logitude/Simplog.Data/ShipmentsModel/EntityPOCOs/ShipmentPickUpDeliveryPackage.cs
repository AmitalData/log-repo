using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPickUpDeliveryPackage
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContainerNumber { get; set; }
        public string PackageTypeId { get; set; }
        public string ShipmentPickUpDeliveryId { get; set; }
        public int? Quantity { get; set; }
        public double? Volume { get; set; }
        public double? Weight { get; set; }
        public string Description { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public string ShipperSeal { get; set; }
        public string Harmonize { get; set; }
        public string OriginalShipmentPackageId { get; set; }
        public bool IsMultiHarmonize { get; set; }

        [ForeignKey("PackageTypeId")]
        public virtual PackageType PackageType { get; set; }

        [ForeignKey("ShipmentPickUpDeliveryId")]
        public virtual ShipmentPickUpDelivery ShipmentPickUpDelivery { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public string ContainerEntityId { get; set; }
    }
}