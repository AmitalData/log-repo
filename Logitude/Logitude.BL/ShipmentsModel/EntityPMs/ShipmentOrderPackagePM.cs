using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ShipmentOrderPackagePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsContainer { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageTypeId { get; set; }

        public string ContainerTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? GrossWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Height { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Width { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Length { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        public string PackageTypeName { get; set; }
        public string Dimensions { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        public string ContainerNumber { get; set; }
    }
}