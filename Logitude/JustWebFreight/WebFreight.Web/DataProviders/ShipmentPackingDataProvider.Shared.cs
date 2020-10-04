using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentPackingDataProvider
    {
        public string FileNumber { get; set; }
        public string Shipper { get; set; }
        public string ShipperAddress { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ShipmentMethod { get; set; }
        public string PackDate { get; set; }
        public string Remarks { get; set; }

        public string DestinationPortName { get; set; }
        public string Signature { get; set; }
        public double? TotalGrossWeight { get; set; }
        public int? TotalNumberOfPackages { get; set; }

        public List<ShipmentPackageProvider> ShipmentPackages { get; set; }
    }

    public class ShipmentPackageProvider
    {
        public string Seal { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerNumber { get; set; }
        public double? GrossWeight { get; set; }
        public string HorseName { get; set; }
        public int? HorseYearOfBirth { get; set; }
        public string HorseColor { get; set; }
        public string HorseGender { get; set; }
        public string HorseBreed { get; set; }
        public string HorseDiscipline { get; set; }
        public string HorseTravelBehavior { get; set; }
        public string HorseMicochipNumber { get; set; }
        public string HorsePassportNumber { get; set; }
        public string HorseCountryOfBirthName { get; set; }
        public string HorseCurrentStable { get; set; }
        public string HorseOwner { get; set; }
        public string HorseRemarks { get; set; }

        public List<PackageItemProvider> PackageItems { get; set; }
    }

    public class PackageItemProvider
    {
        public int Index { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}