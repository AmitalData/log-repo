using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class DeliveryNoteDataProvider:BaseDataProvider
    {
        public DeliveryNoteDataProvider()
        {
            this.PackagesLines = new List<PackageLine>();
            this.AttachmentList = new List<PackageLine>();
            this.InsidePackagesLines = new List<InsidePackageLine>();
        }

        public string To { get; set; }
        public string Address { get; set; }
        public string Attention { get; set; }
        public string OurReferenceNumber { get; set; }
        public string DateSent { get; set; }
        public DateTime DateSent_New { get; set; }
        public string ClientReferenceNumber { get; set; }
        public string Salesman { get; set; }
        public string Telephone { get; set; }
        public string SpecialInstructions { get; set; }

        //Pickup From
        public string PickupCompanyName { get; set; }
        public string PickupAddress { get; set; }
        public string FromAddressDescription { get; set; }
        public string PickupContactName { get; set; }
        public string PickupContactPhone { get; set; }
        public string PickupDate { get; set; }
        public string PickupTime { get; set; }
        public DateTime? PickupTime_DateTime_New { get; set; }

        //Delivery To
        public string DeliveryCompanyName { get; set; }
        public string DeliveryAddress { get; set; }
        public string ToAddressDescription { get; set; }
        public string DeliveryContactName { get; set; }
        public string DeliveryContactPhone { get; set; }
        public string DeliveryContactEmail { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public DateTime? DeliveryTime_DateTime_New { get; set; }
        public string TruckNumber { get; set; }
        public string TruckerNumber { get; set; }
        public string ForwarderAgentCode { get; set; }
        public string ForwarderAgentAddress { get; set; }
        public List<PackageLine> PackagesLines { get; set; }
        public List<PackageLine> AttachmentList { get; set; }
        public double VerticalShift { get; set; }
        public double HorizontalShift { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string ContainersNumbersArray { get; set; }
        public bool InServerSide { get; set; }
        public string TenantName { get; set; }
        public string Signature { get; set; }
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
        public string HAWB { get; set; }
        public string BookingNumber { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string CutOffDate { get; set; }
        public string MainCarriageETD { get; set; }
        public string MainCarriageETA { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string DeliveryCompanyLocalName { get; set; }
        public string DeliveryCompanyContactLocalName { get; set; }
        public string DeliveryCompanyContactPhone { get; set; }
        public string MasterNumber { get; set; }
        public string ConsigneeRef1 { get; set; }
        public string ShipmentNotes { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }
        public string ShipperReference2 { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperContactName { get; set; }
        public string ShipperContactMobileNumber { get; set; }
        public int? TotalNumberOfPackages { get; set; }
        public double? TotalGrossWeight { get; set; }
        public double? TotalVolume { get; set; }
        public DateTime? DeliveryETATime { get; set; } //( the time ) 
        public DateTime? DeliveryETADate { get; set; } //( the date )
        public string LongMaster { get; set; }
        public string LastMainCarriageVesselNameAndNumber { get; set; }
        public string MainCarriageVesselNameAndNumber { get; set; }
        public string LoadingPortName { get; set; }
        public string LoadingPortCode { get; set; }
        public string DischargePortName { get; set; }
        public string DischargePortCode { get; set; }
        public DateTime? MainCarriageETD_DateTime { get; set; }
        public DateTime? MainCarriageETA_DateTime { get; set; }
        public DateTime? MainCarriageATA_DateTime { get; set; }
        public string OriginCountry { get; set; }
        public List<InsidePackageLine> InsidePackagesLines { get; set; }
        public string EmptyContainer { get; set; }
        public string EmptyContainerRef { get; set; }
        public string EmptyContainerReturn { get; set; }
        public string EmptyContainerReturnRef { get; set; }
        public DateTime? CutOffDateAsDate { get; set; }
        public string CutOffTime { get; set; }
        public string UserName { get; set; }
        public string IssuingCarrierAgentName { get; set; }
        public string IssuingCarrierAgentAddress { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string TransportMode { get; set; }
        public string ShipmentSalesman { get; set; }
        public string FinalDestinationCode { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Reference4 { get; set; }
        public string OnCarriageCarrier { get; set; }
        public string OnForwardingCarrier { get; set; }
        public string FreightLocation { get; set; }
        public DateTime? LastFreeDate { get; set; }
        public string CustomerContactName { get; set; }
        public string CustomerContactPhoneNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string DriverName { get; set; }
        public string ProjectNumber { get; set; }
        public string ITNumber { get; set; }
        public string AMSBL { get; set; }
        public string SalesmanEmail { get; set; }
        public string ToPartnerAddressName { get; set; }
        public string IncotermName { get; set; }
        public string ShipperVATNumber { get; set; }
        public string ConsigneeVATNumber { get; set; }
        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrency { get; set; }
        public string TruckerName { get; set; }
        public string CustomsClearancePointName { get; set; }
        public string CustomsClearancePointFullAddress { get; set; }
        public string CustomsClearancePointContactName { get; set; }
        public string CustomsClearancePointContactEmail { get; set; }
        public string CustomsClearancePointTelephoneNumber { get; set; }
        public string DeclarationNumber { get; set; }
        public string WarehouseReferenceNumber { get; set; }
        public int? StorageFreeDays { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchLocalName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string TruckerCompanyContactName { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public DateTime? PickupDeliveryDeparture { get; set; }//(ATD, if null then take ETD)
        public DateTime? PickupDeliveryArrival { get; set; }//(ATA, if null then take ETA)
        public string PickupDeliveryNumber { get; set; }
        public string CarrierCode { get; set; }//(Airline code from Main carriage for example)        
        public string ImportCustomsAgentFullDetails { get; set; }
        public string ExportCustomsAgentFullDetails { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string TruckerAddress { get; set; }
        public string ContainerSeals { get; set; }
        public string ShipmentType { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public DateTime? FinalDestinationETA { get; set; }
        public string TrailerNumber { get; set; }
    }
}