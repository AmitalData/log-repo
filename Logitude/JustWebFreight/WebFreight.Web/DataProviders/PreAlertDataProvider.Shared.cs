using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class PreAlertDataProvider : BaseDataProvider
    {
        public string BranchSignature { get; set; }
        public string ClientName { get; set; }        
        public string Company { get; set; }
        public string OriginAgent { get; set; }
        public string ContactDetails { get; set; }
        public string DeliveryDetails { get; set; }
        public string DeliveryTransportMode { get; set; }
        public string PickupDetails { get; set; }
        public string PickupTransportMode { get; set; }
        public string DestinationAgent { get; set; }
        public string CustomAgent { get; set; }
        public string FileNumber { get; set; }
        public string OpenDate { get; set; }
        public string BookedBy { get; set; }
        public string Type { get; set; }
        public string PCS { get; set; }
        public string Weight { get; set; }
        public string Volume { get; set; }
        public string WeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string MasterNumber { get; set; }
        public string MasterNumber_Label { get; set; }
        public string Carrier { get; set; }
        public string Carrier_Label { get; set; }
        public string FromPort { get; set; }
        public string FromPort_Label { get; set; }
        public string FinalPort { get; set; }
        public string FinalPort_Label { get; set; }
        public string FromPortCode { get; set; }
        public string FinalPortCode { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string MainCarriageCarrierNumber_Label { get; set; }
        public string MainCarriageETD { get; set; }
        public string MainCarriageETA { get; set; }
        public string FinalMainCarriageETA { get; set; }
        public string FirstMainCarriageETD { get; set; }
        public string mainCarriageToPortName { get; set; }
        public string mainCarriageToPortCode { get; set; }
        public string ShippingDetails_FlightDetails { get; set; }
        public string Remarks { get; set; }
        public string TodayDate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string House { get; set; }
        public string GeneralDescriptionOfGoods { get; set; }
        public string Transshipment1Vessel { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment1CarrierName { get; set; }
        public string Transshipment1ToPortName { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1ETA { get; set; }
        public string Transshipment1ETD { get; set; }
        public string Transshipment2Vessel { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment2ToPortName { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2ETA { get; set; }
        public string Transshipment2ETD { get; set; }
        public string Transshipment3Vessel { get; set; }
        public string Transshipment3CarrierNumber { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCode { get; set; }
        public string Transshipment3ETA { get; set; }
        public string Transshipment3ETD { get; set; }
        public string CuttOffDateTime { get; set; }
        public string CuttOffTime { get; set; }
        public List<Packages> PackagesList { get; set; }
        public List<PayableLine> PayablesList { get; set; }
        public List<PickUpDeliveryLine> PickUpsList { get; set; }
        public List<PickUpDeliveryLine> DeliveriesList { get; set; }
        public List<ProductItemLine> ProductItemsLines { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string IncotermCode { get; set; }
        public string OperationalClosingDate { get; set; }
        public string AccountingClosingDate { get; set; }
        public string ShipperReference { get; set; }
        public string ConsigneeReference { get; set; }
        public string CustomerReference { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Containers { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FinalLocation { get; set; }
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
        public string ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }

        List<PreAlertManifestDetails> preAlertmanifestDetails;
        public List<PreAlertManifestDetails> PreAlertManifestDetails
        {
            get
            {
                if (preAlertmanifestDetails == null)
                {
                    preAlertmanifestDetails = new List<PreAlertManifestDetails>();
                }
                return preAlertmanifestDetails;
            }
            set { preAlertmanifestDetails = value; }
        }

        public string SealNumber { get; set; }
        public string ContainerNumbers { get; set; }
        public string Vessel { get; set; }
        public string VoyageNumber { get; set; }
        public string TotalContainers { get; set; }
        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }
        public string MainIncoterm { get; set; }

        // New variables : WI 25371
        public string FullRoutings { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public string PreCarriageCarrierCode { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public DateTime? PreForwardingETD { get; set; }
        public DateTime? PreForwardingATD { get; set; }
        public string PreForwardingCarrierCode { get; set; }
        public string PreForwardingCarrierNumber { get; set; }
        public DateTime? PickupETD { get; set; }
        public DateTime? PickupATD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime?  MainCarriageATA { get; set; }
        public DateTime? Transhipment1ATD { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment1CarrierNumber_New { get; set; }
        public DateTime? Transhipment2ATD { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2CarrierNumber_New { get; set; }
        public DateTime? Transhipment3ATD { get; set; }
        public string Transshipment3CarrierCode { get; set; }
        public string Transshipment3CarrierNumber_New { get; set; }
        public string FullMaster { get; set; } // (prefix-master number)
        public string ShipmentNotes { get; set; }
        public string InvoicesNumbers { get; set; } //It should show the number of the invoices connected to receivables separated by commas like 10001, 1002. If there is only one invoice, it should display as 1000
        public string CustomerPrimaryContactName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string AgentName { get; set; }
        public string AgentPrimaryContactName { get; set; }        
        public int? TotalQuantity { get; set; } //  It must display the total number of packages in a certain shipment.
        public DateTime? Transshipment1ETD_DateTime { get; set; }
        public DateTime? Transshipment2ETD_DateTime { get; set; }
        public DateTime? MainCarriageETD_DateTime { get; set; }
        public DateTime? MainCarriageETA_DateTime { get; set; }
        public DateTime? Transshipment1ETA_DateTime { get; set; }
        public DateTime? Transshipment2ETA_DateTime { get; set; }
        public DateTime? Transshipment3ETD_DateTime { get; set; }
        public DateTime? Transshipment3ETA_DateTime { get; set; }
        public string ReleasingAgentName { get; set; }
        public string ReleasingAgentAddress { get; set; }
        public string BranchAddress { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string PickUpAddress { get; set; }
        public string PlaceOfReceiptCountryName { get; set; }
        public DateTime? OBLDate { get; set; }
        public DateTime? CutOffDate_DateTime { get; set; }
        public DateTime? FreightRelease { get; set; }
        public DateTime? TerminalAvailable { get; set; }
        public string ISFNumber { get; set; }
        public DateTime? ISFDate { get; set; }
        public string ITNumber { get; set; }
        public DateTime? ITDate { get; set; }
        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }
        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public TimeSpan? DocumentsClosingTime { get; set; }
        public List<ShipmentAssemblyLine> Assemblies { get; set; }
        public string OriginCountryName { get; set; }

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
        public string ProjectNumber { get; set; }
        public string BookingConfirmationNumber { get; set; }
        public string Salesman { get; set; }
        public string SpecialServicesType { get; set; }
        public string ShipmentSubTypeName { get; set; }
        public string ConsigneeContactName { get; set; }
        public string ConsigneeContactPhone { get; set; }
        public string ConsigneeAddress { get; set; }
        public string MasterShipmentNumber { get; set; }
    }

    public class Packages
    {
        public Packages()
        {
            this.InsidePackagesLines = new List<InsidePackageLine>();
        }

        public string MarksAndNumbers { get; set; }
        public string PackageType { get; set; }
        public string Quantity { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Weight { get; set; }
        public string Volume { get; set; }
        public string WeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string Dimensions { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string CommodityNumber { get; set; }
        public int? NumberOfInsidePackages { get; set; }
        public string ContainerNumber { get; set; }
        public List<InsidePackageLine> InsidePackagesLines { get; set; }
    }

    public class PreAlertManifestDetails
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
        public string ChargeableWeightUnitCode { get; set; }
        public string Salesman { get; set; }

    }
}