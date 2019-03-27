using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class ManifestDataProvider
    {
        public ManifestDataProvider()
        {
            this.ManifestDetails = new List<ManifestDetailsClass>();
            this.NewManifestDetails = new List<NewManifestDetailsClass>();
        }

        #region Fields From Master
        public string BranchSignature { get; set; }

        public string MasterNumber { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string TodaysDate { get; set; }

        public string ShippingAgentName { get; set; }// 
        public string ShippingAgentAddress { get; set; }

        public string MAWB { get; set; }
        public string OBLNo { get; set; }

        public string Airline { get; set; } //main Carriage Carrier.
        public string FlightDate { get; set; } //maincarriage ETD.
        public string FlightInfo { get; set; }//carrier number.
        public string FlightInfo_New { get; set; }
        public string DeparturePortCode { get; set; } //main carriage from port.
        public string DestinationPortCode { get; set; } //main carriage To Port.

        public string VesselName { get; set; }
        public string VoyageNumber { get; set; }
        public string VoyageDate { get; set; }
        public string ContainerNumber { get; set; }//not implemented right now.
        public string Seal { get; set; }//not implemented yet.

        public string TotalQuantity { get; set; }
        public string TotalWeight { get; set; }
        public string TotalPrepaid { get; set; }
        public string TotalCollect { get; set; }
        public string TotalVolume { get; set; }//for ocean
        public string VolumeUnit { get; set; }
        public string WeightUnit { get; set; }

        public string ContainerNumbersLabel { get; set; }
        public string ContainerNumbers { get; set; }

        public string SealNumbersLablel { get; set; }
        public string SealNumbers { get; set; }

        public byte[] Logo { get; set; }

        public string PortOfLoadingCountryCode { get; set; }
        public string PortOfDischargeCountryCode { get; set; }
        public string PortOfLoadingName { get; set; }
        public string PortOfDischargeName { get; set; }

        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }

        public string IssuingCarrierAgentName { get; set; }
        public string IssuingCarrierAgentAddress { get; set; }

        public string Notes { get; set; }
        public string BookingNumber { get; set; }
        public string MainCarriageETA { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }

        public string ETD { get; set; }
        public string MainCarriageCarrier { get; set; }

        public string MainCarriageCarrierAddress { get; set; }

        public DateTime? MainCarriageETD_DateTime { get; set; }
        public DateTime? MainCarriageETA_DateTime { get; set; }
        public DateTime? MainCarriageATD_DateTime { get; set; }
        public DateTime? MainCarriageATA_DateTime { get; set; }

        public double? OpenPayablesInLocalCurrency { get; set; }
        public double? OpenPayablesInProfitCurrency { get; set; }

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

        public string FMCNumber { get; set; }

        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public string AWBHandlingInformation { get; set; }

        public string ConsolidatorName { get; set; }
        public string ConsolidatorAddress { get; set; }
        public string ReleasingAgentName { get; set; }
        public string ReleasingAgentAddress { get; set; }

        public string AgentPhoneNumber { get; set; }
        public string AgentContactName { get; set; }
        #endregion

        public List<ManifestDetailsClass> ManifestDetails { get; set; }
        public List<NewManifestDetailsClass> NewManifestDetails { get; set; }
        public List<GroupedContainersClass> GroupedManifestDetailsList { get; set; }
    }

    public class ManifestDetailsClass
    {
        public string FileNumber { get; set; } // shipment number.
        public string HAWB { get; set; }
        public string ShipperName { get; set; }        
        public string ShipperAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }

        public string DestinationPortCode { get; set; } //includs on carriage "this might be not true.".
        public string PortOfDischarge { get; set; }//for ocean.

        public string PackageKind { get; set; } // for ocean
        public string PackageQuantity { get; set; }// for ocean        

        public string Quantity { get; set; }
        public string Weight { get; set; }//gross weight
        public string ChargeableWeight { get; set; }
        public string Volume { get; set; }//for ocean
        public string DescriptionOfGoods { get; set; }
        public string Prepaid { get; set; }
        public string Collect { get; set; }
        public string PC { get; set; } // for ocean
        public string VolumeUnit { get; set; }
        public string WeightUnit { get; set; }

        public string Incoterm { get; set; }
        public string House { get; set; }
        public string DestinationPortName { get; set; }
        public string NotifyName { get; set; }
        public string NotifyAddress { get; set; }
        public string PlaceOfDelivery { get; set; }

        public string ShipperVAT { get; set; }
        public string ConsigneeVAT { get; set; }
        public string Direction { get; set; }

        public string ShipperField1 { get; set; }
        public string ShipperField2 { get; set; }
        public string ShipperField3 { get; set; }
        public string ShipperField4 { get; set; }
        public string ShipperField5 { get; set; }
        public string ShipperField6 { get; set; }
        public string ShipperField7 { get; set; }
        public string ShipperField8 { get; set; }
        public string ShipperField9 { get; set; }
        public string ShipperField10 { get; set; }

        public string ConsigneeField1 { get; set; }
        public string ConsigneeField2 { get; set; }
        public string ConsigneeField3 { get; set; }
        public string ConsigneeField4 { get; set; }
        public string ConsigneeField5 { get; set; }
        public string ConsigneeField6 { get; set; }
        public string ConsigneeField7 { get; set; }
        public string ConsigneeField8 { get; set; }
        public string ConsigneeField9 { get; set; }
        public string ConsigneeField10 { get; set; }

        public double? OpenPayablesInLocalCurrency { get; set; }
        public double? OpenPayablesInProfitCurrency { get; set; }

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

        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public string AWBHandlingInformation { get; set; }

        public string ShipperContactName { get; set; } // (main contact English name for the shipper) 
        public string ConsigneeContactName { get; set; }// (main contact English name for the consignee ) 
        public string ITNumber { get; set; } //(from the house\customs tab ) 

        public List<ShipmentAssemblyLine> Assemblies { get; set; }
    }

    public class NewManifestDetailsClass
    {
        public string FileNumber { get; set; }
        public string HAWB { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string DestinationPortCode { get; set; }
        public string PortOfDischarge { get; set; }
        public string ChargeableWeight { get; set; }
        public string Prepaid { get; set; }
        public string Collect { get; set; }
        public string PC { get; set; }
        public string VolumeUnit { get; set; }
        public string WeightUnit { get; set; }
        public string Incoterm { get; set; }
        public string House { get; set; }
        public string DestinationPortName { get; set; }
        public string NotifyName { get; set; }
        public string NotifyAddress { get; set; }
        public string PlaceOfDelivery { get; set; }

        public int? Quantity { get; set; }
        public double? Volume { get; set; }
        public double? Weight { get; set; }        
        public string DescriptionOfGoods { get; set; }

        List<PackageDetails> packageDetails;
        public List<PackageDetails> PackageDetails
        {
            get
            {
                if (packageDetails == null)
                {
                    packageDetails = new List<PackageDetails>();
                }
                return packageDetails;
            }
            set { packageDetails = value; }
        }

        public string ShipperVAT { get; set; }
        public string ConsigneeVAT { get; set; }
        public string Direction { get; set; }

        public string ShipperField1 { get; set; }
        public string ShipperField2 { get; set; }
        public string ShipperField3 { get; set; }
        public string ShipperField4 { get; set; }
        public string ShipperField5 { get; set; }
        public string ShipperField6 { get; set; }
        public string ShipperField7 { get; set; }
        public string ShipperField8 { get; set; }
        public string ShipperField9 { get; set; }
        public string ShipperField10 { get; set; }

        public string ConsigneeField1 { get; set; }
        public string ConsigneeField2 { get; set; }
        public string ConsigneeField3 { get; set; }
        public string ConsigneeField4 { get; set; }
        public string ConsigneeField5 { get; set; }
        public string ConsigneeField6 { get; set; }
        public string ConsigneeField7 { get; set; }
        public string ConsigneeField8 { get; set; }
        public string ConsigneeField9 { get; set; }
        public string ConsigneeField10 { get; set; }

        public double? OpenPayablesInLocalCurrency { get; set; }
        public double? OpenPayablesInProfitCurrency { get; set; }

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

        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public string AWBHandlingInformation { get; set; }

        public List<ShipmentAssemblyLine> Assemblies { get; set; }

        //public string MarksAndNumbers { get; set; }  //new
        //public string PackageQtyKind { get; set; } //new
        //public string Weight { get; set; } //new
        //public string Volume { get; set; } //new
        //public string DescriptionOfGoods { get; set; } //new
    }

    public class PackageDetails
    {
        public string MarksAndNumbers { get; set; }
        public string PackageKind { get; set; }
        public string Quantity { get; set; }
        public string Weight { get; set; }
        public string Volume { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string CommodityNumber { get; set; }
    }

    public class GroupedContainersClass
    {
        public string MasterContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public double? ContainerGrossWeight { get; set; }
        public string ContainerGrossWeightUnitCode { get; set; }
        public double? ContainerVolume { get; set; }
        public string ContainerVolumeUnitCode { get; set; }
        public double? ContainerTare { get; set; }
        public double? ContainerVolumetricWeight { get; set; }
        public string ContainerVolumetricWeightUnitCode { get; set; }
        public List<GroupedShipmentClass> GroupedShipmentList { get; set; }
    }

    public class GroupedShipmentClass
    {
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string PortOfDischarge { get; set; }
        public string PC { get; set; }

        public List<GroupedManifestPackages> GroupedManifestPackagesList { get; set; }
    }

    public class GroupedManifestPackages
    {
        public string MarksAndNumbers { get; set; }
        public int? Quantity { get; set; }
        public string DescriptionOfGoods { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public string ContainerNumber { get; set; }
    }
}