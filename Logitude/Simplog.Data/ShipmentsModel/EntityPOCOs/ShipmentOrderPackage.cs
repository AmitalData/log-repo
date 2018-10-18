using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentOrderPackage
    {
       
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string ShipmentId { get; set; }

        public bool IsContainer { get; set; }

        public string PackageTypeId { get; set; }

        //public string ContainerTypeId { get; set; }

        public int? Quantity { get; set; }

        public double? GrossWeight { get; set; }
        public double? Volume { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? VolumetricWeight { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        [ForeignKey("PackageTypeId")]
        public virtual PackageType PackageType { get; set; }

        //public virtual ContainerType ContainerType { get; set; }
    }
}