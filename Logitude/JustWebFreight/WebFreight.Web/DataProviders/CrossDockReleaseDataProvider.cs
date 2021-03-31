using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CrossDockReleaseDataProvider:BaseDataProvider
    {
        public string CustomerName { get; set; }
        public string CustomerRef1 { get; set; }
        public string CustomerRef2 { get; set; }
        public string SpecialInstruction { get; set; }
        public string IntenalNotes { get; set; }
        public DateTime? ExpectedReleaseDate { get; set; }
        public DateTime? ActualReleaseDate { get; set; }
        public string UpdatedBy { get; set; }
        public string ReleaseBy { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseAddress1 { get; set; }
        public string WarehouseAddress2 { get; set; }
        public string WarehouseCity { get; set; }
        public string WarehouseState { get; set; }
        public string WarehouseZipCode { get; set; }
        public string WarehouseCountry { get; set; }
        public string WraehousePhone { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string TenantAddress { get; set; }
        public byte[] TenantLogo { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public string ShipmentNumber { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string DestinationCountryName { get; set; }
        public string ReleaseNumber { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ReleaseReference { get; set; }
        public string DimensionsHeader { get; set; }
        public string VolumetricWeightUnit { get; set; }
        public string WeightUnit { get; set; }
        public string VolumeUnit { get; set; }
        public string ReleaseDateIndicator { get; set; }

        public string Trucker { get; set; }
        public string TruckNumber { get; set; }

        public List<ReleasePackage> ReleasePackages { get; set; }
        public List<ReleasePackageGroup> ReleasePackagesGroupList { get; set; }

        public DateTime? DeclarationDate { get; set; }
        public string DeclarationNumber { get; set; }
        public int NumberofDaysInTheWarehouse { get; set; }
        public string ConsigneeVATNumber { get; set; }
        public string ConsigneeContactPersonName { get; set; }
        public string ConsigneeContactPersonEmail { get; set; }
        public double? ValueofGoods { get; set; }
        public string ValueofGoodsCurrency { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }

        public string GeneralDescriptionofGoods { get; set; }
        public string ShipperVATNumber { get; set; }
        public string ShipperContactPersonName { get; set; }
        public string ShipperContactPersonEmail { get; set; }
        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
        public string ShipmentField5 { get; set; }
        public string ShipmentField6 { get; set; }
        public string ShipmentField7 { get; set; }
        public string ShipmentField8 { get; set; }
        public string ShipmentField9 { get; set; }
        public string ShipmentField10 { get; set; }

        public string ActualEntryDate { get; set; }
        public string EntryTruckerName { get; set; }
        public string EntryTruckerReference { get; set; }
        public string TerminalCode { get; set; }
        
        public string ImportManifest { get; set; }
        public string MasterImportManifest { get; set; }
        public string ConnectedShipmentTransportMode { get; set; }
        public string Trailer { get; set; }
        public int StorageDays { get; set; }
        public string ProjectNumber { get; set; }
        public int? StorageFreeDays { get; set; }
        public string MainCarriageTruckerNumber { get; set; }

    }


    public class ReleasePackageGroup
    {
        public string EntryId { get; set; }
        public string EntryNumber { get; set; }
        public List<ReleasePackage> ReleasePackagesList { get; set; }
    }

    public class ReleasePackage
    {
        public string Dimensions { get; set; }
        public string PackageType { get; set; }
        public int Quantity { get; set; }
        public decimal? Volume { get; set; }
        public string VolumeUnit { get; set; }
        public decimal? Weight { get; set; }
        public string WeightUnit { get; set; }
        public string Harmonize { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string ContainerNumber { get; set; }
        public string Seal { get; set; }
        public double? VolumetricWeight { get; set; }
        public string VolumetricWeightUnit { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryName { get; set; }
        public int? InStock { get; set; }
        public string EntryId { get; set; }
        public string EntryNumber { get; set; }

        


    }
}