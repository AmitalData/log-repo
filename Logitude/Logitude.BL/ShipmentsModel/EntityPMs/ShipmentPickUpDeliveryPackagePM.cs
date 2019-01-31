using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ShipmentPickUpDeliveryPackagePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageTypeId { get; set; }
        public string PackageTypeName { get; set; }
        public double? PackageTypeTEU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPickUpDeliveryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Weight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Harmonize { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperSeal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Width { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Height { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Length { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OriginalShipmentPackageId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
