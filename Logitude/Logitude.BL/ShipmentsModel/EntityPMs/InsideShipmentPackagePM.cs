using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class InsideShipmentPackagePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentPackageId { get; set; }
        public string PackageTypeId { get; set; }
        public string PackageTypeCode { get; set; }
        public string PackageTypeName { get; set; }
        public string PrintAs { get; set; }
        public int ContainerSize { get; set; }
        public double TEU { get; set; }
        public bool IsContainer { get; set; }
        
        public bool PackageTypeIsAir { get; set; }
        public bool PackageTypeIsOcean { get; set; }
        public bool PackageTypeIsInland { get; set; }
        public string PackageTypeNote { get; set; }
        public string PackageTypeLocalName { get; set; }
        public decimal PackageTypeVolume { get; set; }
        public bool IsPackageAddedManually { get; set; }

        public string OriginalInsideShipmentPackageId { get; set; }
        
        //public string CommodityTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Width { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Height { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Length { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Weight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OriginalShipmentPackageId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Reference1 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Reference2 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Reference3 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Reference4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CommodityNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CommodityName { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode{ get; set; }
    }
}