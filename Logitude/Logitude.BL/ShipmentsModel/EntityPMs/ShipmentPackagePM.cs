using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using Logitude.BL.Validators;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
        
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(ShipmentPackageValidator), "IsShipmentPackageValid")]
    public class ShipmentPackagePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsContainer { get; set; }
        public bool IsContainerRefrigerated { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackageTypeId { get; set; }
        public string PackageTypeCode { get; set; }
        public string PackageTypeName { get; set; }
        public string PrintAs { get; set; }
        public int ContainerSize { get; set; }
        public double TEU { get; set; }

        public bool PackageTypeIsAir { get; set; }
        public bool PackageTypeIsOcean { get; set; }
        public bool PackageTypeIsInland { get; set; }
        public string PackageTypeNote { get; set; }
        public string PackageTypeLocalName { get; set; }
        public decimal PackageTypeVolume { get; set; }
        public bool IsPackageAddedManually { get; set; }
        public string WarehouseReleaseNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MaterialDescription { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperSeal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Tare { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? Quantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Weight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Height { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Width { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Length { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UnNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ClassNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Temperature { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Ventilation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierSeal { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? SOC { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PackagingGroup { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IMDGCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FlashPoint { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Harmonize { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MarksAndNumbers { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        public string ShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }       
        public string ShipmentPMId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsDangerous { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OriginalShipmentPackageId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CommodityId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int NumberOfInsidePackages { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NumberOfInsidePackagesDetails { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public decimal? VGM { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MethodUsed { get; set; }

        public bool IsDeliveryFU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeliveryATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryTo { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryFrom { get; set; }

        public bool IsEmptyContainerReturnFU { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? EmptyContainerReturnATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnTo { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmptyContainerReturnFrom { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CeficClass { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string KelmerCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EMS { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProperShippingName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool MarinePollutant { get; set; }

        public bool NonActiveContainer { get; set; }

        //Dummy
        public bool IsAWBWizardDefault { get; set; }
        public string DummyIdGuid { get; set; }        

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OnCarriageETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OnCarriageATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OnCarriageETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OnCarriageATA { get; set; }


        // public string CustomerId { get; set; }
        //public string DirectionId { get; set; }
        //public string MainCarriageCarrierNumber { get; set; }
        // public DateTime CreateDateTime { get; set; }
        //public DateTime? MainCarriageATD { get; set; }
        //public string BookingConfirmationNumber { get; set; }
        //public string MainCarriageFromPortCode { get; set; }
        //public string MainCarriageFinalDestinationPortCode{ get; set; }

        [Include]
        [Association("ShipmentPackagePMShipment1", "ShipmentPMId", "Id", IsForeignKey = true)]
        public virtual ShipmentPM ShipmentPM { get; set; }

        private List<InsideShipmentPackagePM> insideShipmentPackages;
        [Composition]
        [Include]
        [Association("ShipmentPackageInsideShipmentPackage", "Id", "ShipmentPackageId")]
        public virtual List<InsideShipmentPackagePM> InsideShipmentPackages
        {
            get
            {
                if (insideShipmentPackages == null)
                {
                    insideShipmentPackages = new List<InsideShipmentPackagePM>();
                }
                return insideShipmentPackages;
            }
            set { insideShipmentPackages = value; }
        }

        private List<ShipmentPackageItemPM> shipmentPackageItems;
        [Composition]
        [Include]
        [Association("ShipmentPackageShipmentPackageItem", "Id", "PackageId")]
        public virtual List<ShipmentPackageItemPM> ShipmentPackageItems
        {
            get
            {
                if (shipmentPackageItems == null)
                {
                    shipmentPackageItems = new List<ShipmentPackageItemPM>();
                }

                return shipmentPackageItems;
            }

            set { shipmentPackageItems = value; }
        }

        public ChangeSetOperation ChangeSetOp { get; set; }

        private List<InsideShipmentPackagePM> insideShipmentPackagesChangeSet;
        public List<InsideShipmentPackagePM> InsideShipmentPackagesChangeSet
        {
            get
            {
                if (insideShipmentPackagesChangeSet == null)
                {
                    insideShipmentPackagesChangeSet = new List<InsideShipmentPackagePM>();
                }

                return insideShipmentPackagesChangeSet;
            }

            set
            {
                insideShipmentPackagesChangeSet = value;
            }
        }

        private List<ShipmentPackageItemPM> shipmentPackageItemsChangeSet;
        public List<ShipmentPackageItemPM> ShipmentPackageItemsChangeSet
        {
            get
            {
                if (shipmentPackageItemsChangeSet == null)
                {
                    shipmentPackageItemsChangeSet = new List<ShipmentPackageItemPM>();
                }

                return shipmentPackageItemsChangeSet;
            }

            set
            {
                shipmentPackageItemsChangeSet = value;
            }
        }

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

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TemperatureUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FlashPointTemperatureUnitCode { get; set; }

        public int SplitIndex { get; set; }
        public bool IsFromSplit { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastStatusCode { get; set; }

        public string ContainerStatusSourceCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryTransportModeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ECRTransportModeCode { get; set; }

        public bool IsMultiHarmonize { get; set; }

        public int InUse { get; set; }

        private List<ShipmentPackageHarmonizePM> shipmentPackageHarmonizes;
        [Composition]
        [Include]
        [Association("ShipmentPackageShipmentPackageHarmonize", "Id", "PackageId")]
        public virtual List<ShipmentPackageHarmonizePM> ShipmentPackageHarmonizes
        {
            get
            {
                if (shipmentPackageHarmonizes == null)
                {
                    shipmentPackageHarmonizes = new List<ShipmentPackageHarmonizePM>();
                }

                return shipmentPackageHarmonizes;
            }

            set { shipmentPackageHarmonizes = value; }
        }

        private List<ShipmentPackageHarmonizePM> shipmentPackageHarmonizesChangeSet;
        public List<ShipmentPackageHarmonizePM> ShipmentPackageHarmonizesChangeSet
        {
            get
            {
                if (shipmentPackageHarmonizesChangeSet == null)
                {
                    shipmentPackageHarmonizesChangeSet = new List<ShipmentPackageHarmonizePM>();
                }

                return shipmentPackageHarmonizesChangeSet;
            }

            set
            {
                shipmentPackageHarmonizesChangeSet = value;
            }
        }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Routing { get; set; }
        public string RoutingIds { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VoyageTripNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool HasContainerException { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsVehicle { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string HorseId { get; set; }
        public string HorseName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LCLContainerTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContainerEntityId { get; set; }
    }
}
