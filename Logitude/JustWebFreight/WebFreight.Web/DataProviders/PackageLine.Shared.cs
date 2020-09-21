using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class PackageLine
    {
        public PackageLine()
        {
            this.InsidePackagesLines = new List<InsidePackageLine>();
        }

        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Dimensions { get; set; }

        private string packageMarksAndNumbers = "";
        public string PackageMarksAndNumbers
        {
            get { return packageMarksAndNumbers; }
            set { packageMarksAndNumbers = value; }
        }

        public string PackageMarksAndNumbersNew { get; set; }

        private string packageQuantity = "";
        public string PackageQuantity
        {
            get { return packageQuantity; }
            set { packageQuantity = value; }
        }

        private string packageType = "";
        public string PackageType
        {
            get { return packageType; }
            set { packageType = value; }
        }

        private string packageDescriptionOfGoods = "";
        public string PackageDescriptionOfGoods
        {
            get { return packageDescriptionOfGoods; }
            set { packageDescriptionOfGoods = value; }
        }

        private string descriptionOfGoodsWithoutHCCode = "";
        public string DescriptionOfGoodsWithoutHCCode
        {
            get { return descriptionOfGoodsWithoutHCCode; }
            set { descriptionOfGoodsWithoutHCCode = value; }
        }

        private string packageGrossWeight = "";
        public string PackageGrossWeight
        {
            get { return packageGrossWeight; }
            set { packageGrossWeight = value; }
        }

        private string packageVolume = "";
        public string PackageVolume
        {
            get { return packageVolume; }
            set { packageVolume = value; }
        }

        private string containerNumber = "";
        public string ContainerNumber
        {
            get { return containerNumber; }
            set { containerNumber = value; }
        }

        public string PackageTare { get; set; }

        private string packageQuantityAndType = "";
        public string PackageQuantityAndType
        {
            get { return packageQuantityAndType; }
            set { packageQuantityAndType = value; }
        }

        private string isDangerous = "";
        public string IsDangerous
        {
            get { return isDangerous; }
            set { isDangerous = value; }
        }

        private string totalFor = "";
        public string TotalFor
        {
            get { return totalFor; }
            set { totalFor = value; }
        }

        public string InsidePackagesCount { get; set; }

        public string SealNumber { get; set; }
        public string ContainerSize { get; set; }
        public string PackageTypeName { get; set; }

        public string Seal1 { get; set; }
        public string Seal2 { get; set; }

        public decimal? VGM { get; set; }
        public string MethodUsed { get; set; }

        public double? PackageVolume_Double { get; set; }
        public string PackageVolumetricWeight { get; set; }
        public double? PackageGrossWeight_Double { get; set; }
        public bool IsContainer { get; set; }
        public string PackageMarksAndNumbers_OneLine { get; set; }

        public List<InsidePackageLine> InsidePackagesLines { get; set; }

        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Reference4 { get; set; }
        public string CommodityNumber { get; set; }
        public string PackageTypeCode { get; set; }

        public string HSCode { get; set; }
        public string CeficClass { get; set; }
        public string IMDGCode { get; set; }
        public string KemlerCode { get; set; }
        public string UNCode { get; set; }
        public string MarinePollutant { get; set; }
        public string PackingGroup { get; set; }
        public string EMS { get; set; }
        public string ProperShippingName { get; set; }
        public string PackingCode { get; set; }
        public string FlashPoint { get; set; }
        public double? NetWeight { get; set; }
        public string InsidePackagesDescription { get; set; }
        public string DangerousDescription { get; set; }
        public string Description { get; set; }
        public string InsidePackagesDetails { get; set; }
        public string Notes { get; set; }
        public string MarksAndNumbersOnly { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryName { get; set; }
        public string Temperature { get; set; }

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
    }

    public class InsidePackageLine
    {
        public string PackageType { get; set; }
        public int? Quantity { get; set; }
        public string Dimensions { get; set; }
        public double? Volume { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? Weight { get; set; }
        public string Description { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string CommodityNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryName { get; set; }
        public string HSCode { get; set; }
    }

    public class InsidePackageGroup
    {
        public string PackageTypeName { get; set; }
        public int? Count { get; set; }
    }
}
