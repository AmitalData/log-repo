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
        public string ShipmentNumber { get; set; }
        public string ChangeSetOp { get; set; }
        public string ContainerNumber { get; set; }
        public string PackageTypeId { get; set; }
        public string ContainerEntityId { get; set; }
        public string FlashPointTemperatureUnitCode { get; set; }
        public string TemperatureUnitCode { get; set; }
        public bool IsContainer { get; set; }
        public bool IsContainerRefrigerated { get; set; }


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




        public string MaterialDescription { get; set; }


        public string ShipperSeal { get; set; }


        public double? Tare { get; set; }




        public double? Volume { get; set; }


        public double? VolumetricWeight { get; set; }




        public string UnNumber { get; set; }


        public string ClassNumber { get; set; }


        public string Temperature { get; set; }


        public double? Ventilation { get; set; }


        public string CarrierSeal { get; set; }


        public int? SOC { get; set; }


        public string PackagingGroup { get; set; }


        public string IMDGCode { get; set; }


        public string FlashPoint { get; set; }


        public string Harmonize { get; set; }


        public string MarksAndNumbers { get; set; }


        public string Description { get; set; }

        public string ShipmentPMId { get; set; }


        public bool IsDangerous { get; set; }


        public string OriginalShipmentPackageId { get; set; }


        public string CommodityId { get; set; }


        public int NumberOfInsidePackages { get; set; }


        public string NumberOfInsidePackagesDetails { get; set; }


        public decimal? VGM { get; set; }


        public string MethodUsed { get; set; }

        public bool IsDeliveryFU { get; set; }


        public string DeliveryId { get; set; }


        public DateTime? DeliveryETD { get; set; }


        public DateTime? DeliveryATD { get; set; }


        public DateTime? DeliveryETA { get; set; }


        public DateTime? DeliveryATA { get; set; }


        public string DeliveryTo { get; set; }


        public string DeliveryFrom { get; set; }

        public bool IsEmptyContainerReturnFU { get; set; }


        public string EmptyContainerReturnId { get; set; }


        public DateTime? EmptyContainerReturnETD { get; set; }


        public DateTime? EmptyContainerReturnATD { get; set; }


        public DateTime? EmptyContainerReturnETA { get; set; }


        public DateTime? EmptyContainerReturnATA { get; set; }


        public string EmptyContainerReturnTo { get; set; }


        public string EmptyContainerReturnFrom { get; set; }


        public string CeficClass { get; set; }


        public string KelmerCode { get; set; }


        public string EMS { get; set; }


        public string ProperShippingName { get; set; }


        public bool MarinePollutant { get; set; }

        public bool NonActiveContainer { get; set; }

        //Dummy
        public bool IsAWBWizardDefault { get; set; }
        public string DummyIdGuid { get; set; }


        public DateTime? OnCarriageETD { get; set; }


        public DateTime? OnCarriageATD { get; set; }


        public DateTime? OnCarriageETA { get; set; }


        public DateTime? OnCarriageATA { get; set; }


        


        
        public virtual ShipmentPM ShipmentPM { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public string Reference4 { get; set; }


        public string CommodityNumber { get; set; }

        public string CommodityName { get; set; }


        public string Notes { get; set; }


       

        public int SplitIndex { get; set; }
        public bool IsFromSplit { get; set; }


        public string LastStatusCode { get; set; }

        public string ContainerStatusSourceCode { get; set; }


        public string LastStatusName { get; set; }


        public DateTime? LastStatusDate { get; set; }


        public string DeliveryTransportModeCode { get; set; }


        public string ECRTransportModeCode { get; set; }

        public bool IsMultiHarmonize { get; set; }

        public int InUse { get; set; }

       


        public DateTime? ETD { get; set; }


        public DateTime? ETA { get; set; }


        public string Routing { get; set; }
        public string RoutingIds { get; set; }


        public string VoyageTripNumber { get; set; }


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


        public string HorseId { get; set; }
        public string HorseName { get; set; }


        public string LCLContainerTypeId { get; set; }


        public bool IsCreatedFromPickupDelivery { get; set; }
        public bool IsPackageCheckedInLeg { get; set; }
    }
}