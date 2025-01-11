using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class CommodityPackagePM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string PackageTypeId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Weight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Height { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Width { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public double? Length { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string Description { get; set; }

        public string ShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string CommodityId { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public int NumberOfInsidePackages { get; set; }

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string NumberOfInsidePackagesDetails { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}