using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class InsideShipmentPackage
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentPackageId { get; set; }
        public string PackageTypeId { get; set; }
        //public string CommodityTypeId { get; set; }
        public int? Quantity { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Weight { get; set; }
        public string Description { get; set; }
        public double? Volume { get; set; }
        public string OriginalShipmentPackageId { get; set; }
        public string OriginalInsideShipmentPackageId { get; set; }
        public double? VolumetricWeight { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Reference4 { get; set; }
        public string CommodityNumber { get; set; }
        public string CommodityName { get; set; }

        public string Harmonize { get; set; }
        public bool IsMultiHarmonize { get; set; }

        //[Include]
        //[Association("ShipmentPackageInsideShipmentPackage","ShipmentPackageId","Id",IsForeignKey=true)]
        [ForeignKey("ShipmentPackageId")]
        public virtual ShipmentPackage ShipmentPackage { get; set; }

        //[ExternalReference]
        //[Association("PackageTypeInsideShipmentPackage", "PackageTypeId", "Id", IsForeignKey = true)]
        [ForeignKey("PackageTypeId")]
        public virtual PackageType PackageType { get; set; }

        ////[ExternalReference]
        ////[Association("CommodityTypeInsideShipmentPackage", "CommodityTypeId", "Id", IsForeignKey = true)]
        //public virtual CommodityType CommodityType { get; set; }


        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public string HorseId { get; set; }
        public virtual Horse Horse { get; set; }


    }
}