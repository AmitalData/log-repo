using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class ShipmentPickUpDeliveryPackagePM : EntityPM
    {
        //[Key]
        //public string Id { get; set; }
        //public int Tenant { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ContainerNumber { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string PackageTypeId { get; set; }
        //public string PackageTypeName { get; set; }
        //public double? PackageTypeTEU { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ShipmentPickUpDeliveryId { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public int? Quantity { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public double? Volume { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public double? Weight { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Description { get; set; }
        
        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Harmonize { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ShipperSeal { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public double? Width { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public double? Height { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public double? Length { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string OriginalShipmentPackageId { get; set; }

        //public ChangeSetOperation ChangeSetOp { get; set; }

        //public bool IsMultiHarmonize { get; set; }

        //private List<PickUpDeliveryPackageHarmonizePM> pickUpDeliveryPackageHarmonizes;
        [Composition]
        [Include]
        [Association("PickUpDeliveryPackagePickUpDeliveryPackageHarmonize", "Id", "PackageId")]
        public virtual List<PickUpDeliveryPackageHarmonizePM> PickUpDeliveryPackageHarmonizes
        {
            get
            {
                if (pickUpDeliveryPackageHarmonizes == null)
                {
                    pickUpDeliveryPackageHarmonizes = new List<PickUpDeliveryPackageHarmonizePM>();
                }

                return pickUpDeliveryPackageHarmonizes;
            }

            set { pickUpDeliveryPackageHarmonizes = value; }
        }

        private List<PickUpDeliveryPackageHarmonizePM> pickUpDeliveryPackageHarmonizesChangeSet;
        public List<PickUpDeliveryPackageHarmonizePM> PickUpDeliveryPackageHarmonizesChangeSet
        {
            get
            {
                if (pickUpDeliveryPackageHarmonizesChangeSet == null)
                {
                    pickUpDeliveryPackageHarmonizesChangeSet = new List<PickUpDeliveryPackageHarmonizePM>();
                }

                return pickUpDeliveryPackageHarmonizesChangeSet;
            }

            set
            {
                pickUpDeliveryPackageHarmonizesChangeSet = value;
            }
        }

        //public string Make { get; set; }
        //public string Model { get; set; }
        //public string Year { get; set; }
        //public string Color { get; set; }
        //public string ChassisNumber { get; set; }
        //public string RegistrationNumber { get; set; }
        //public string CountryId { get; set; }
        //public string ContainerEntityId { get; set; }
        //public string ChangeSet { get; set; }
    }
}
