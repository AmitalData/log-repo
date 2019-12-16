using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class CrossDockReleaseDataProvider
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

        public List<ReleasePackage> ReleasePackages { get; set; }
        public List<ReleasePackageGroup> ReleasePackagesGroupList { get; set; }
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