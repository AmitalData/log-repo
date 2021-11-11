
using System;
using System.Collections.Generic;
namespace WebFreight.Web.DataProviders
{
    public class CMRDataProvider:BaseDataProvider
    {
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string TenantAddress { get; set; }
        public string DeliveryToAddress { get; set; }
        public string PickUpAddressAndDate { get; set; }
        public string DocumentsAttached { get; set; }
        public string MarksAndNumbers { get; set; }
        public string NumberOfPackages { get; set; }
        public string TotalWeight { get; set; }
        public string Volume { get; set; }
        /// <summary>
        /// Tenant Weight unit
        /// </summary>
        public string WeightUnit { get; set; }
        public string ShipmentWeightUnit { get; set; }
        public string ShipmentVolumUnit { get; set; }
        public string SendersInstructions { get; set; }
        public string CashOnDelivery { get; set; }
        public string TruckNumberAndTrailerNumber { get; set; }
        public string IssueBranchAndDate { get; set; }
        public string PickUpOrDeliveryNumber { get; set; }
        public string ConsigneeAddress { get; set; }
        public string CompanyName { get; set; }
        public string ShipperAddress { get; set; }
        public string CopyNameFirstLanguage { get; set; }
        public string CopyNameSecondLanguage { get; set; }
        public string EnglishLanguage { get; set; }
        public string RussianLanguage { get; set; }
        public string CopyNumber { get; set; }
        //public string CustomCode { get; set; }
        //public string Delivery { get; set; }
        /// <summary>
        /// Harmonize field from ShipmentPickupDelieryPacakge
        /// </summary>
        public string HsCode { get; set; }
        public string Note { get; set; }
        public string Instructions_13 { get; set; }

        public string FromLocation { get; set; }
        public string ToLocation { get; set; }

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
        public string ShipmentField11 { get; set; }
        public string ShipmentField12 { get; set; }
        public string ShipmentField13 { get; set; }
        public string ShipmentField14 { get; set; }
        public string ShipmentField15 { get; set; }
        public string ShipmentField16 { get; set; }
        public string ShipmentField17 { get; set; }
        public string ShipmentField18 { get; set; }
        public string ShipmentField19 { get; set; }
        public string ShipmentField20 { get; set; }
        public string ShipmentField21 { get; set; }
        public string ShipmentField22 { get; set; }
        public string ShipmentField23 { get; set; }
        public string ShipmentField24 { get; set; }
        public string ShipmentField25 { get; set; }
        public string ShipmentField26 { get; set; }
        public string ShipmentField27 { get; set; }
        public string ShipmentField28 { get; set; }
        public string ShipmentField29 { get; set; }
        public string ShipmentField30 { get; set; }
        public string ShipmentField31 { get; set; }
        public string ShipmentField32 { get; set; }
        public string ShipmentField33 { get; set; }
        public string ShipmentField34 { get; set; }
        public string ShipmentField35 { get; set; }
        public string ShipmentField36 { get; set; }
        public string ShipmentField37 { get; set; }
        public string ShipmentField38 { get; set; }
        public string ShipmentField39 { get; set; }
        public string ShipmentField40 { get; set; }

        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }

        public string IssueBranch { get; set; }
        public DateTime? IssueDate { get; set; }
        public string ConsigneePickUpAddress { get; set; }

        public List<ContainerData> ContainersList { get; set; }

        // Warehouse Fields 
        public string WarehouseLegTerminalName { get; set; }
        public string WarehouseLegAddress { get; set; }
        public string WarehouseLegTerminalCode { get; set; }
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        public string WarehouseLegRemarks { get; set; }
        public string WarehouseLegReference { get; set; }
        public DateTime? WarehouseLegEntryDate { get; set; }
        public DateTime? WarehouseLegReleaseDate { get; set; }

        public string EmptyContainer { get; set; }
        public string EmptyContainerRef { get; set; }
        public string EmptyContainerReturn { get; set; }
        public string EmptyContainerReturnRef { get; set; }

        public string PickUpOrDeliveryTransportMode { get; set; }

        public string Trucker { get; set; }
        public string TruckerMainAddress { get; set; }
        public string ShipmentNumber { get; set; }
        public string PickupFromPartnerName { get; set; }
        public string DeliveryToPartnerName { get; set; }
        public string HAWBNumber { get; set; }
        public string MAWBNumber { get; set; }
        public string TruckerNumber { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public string ProjectNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
    }

    public class ContainerData
    {
        public string Weight { get; set; }
        public string Volume { get; set; }
        public int? Pieces { get; set; }
        public string Description { get; set; }
        public string MarksAndNumbers { get; set; }
        public string MarksAndNumbers_Direct { get; set; }
        public string HsCode { get; set; }
        public string Dimensions { get; set; }
        public string PackageType { get; set; }
        public List<InsidePackageLine> InsidePackagesLines { get; set; }
    }
}