using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class ShippingDeclarationDataProvider : BaseDataProvider
    {
        public string MasterAMSBL { get; set; }
        public string CopyNumber { get; set; }
        public string CopyName { get; set; }
        public string BranchSignature { get; set; }

        /// <summary>
        /// Consignor
        /// </summary>
        public string ShipperAddress { get; set; }
        public string ShipperAddress_NoTel { get; set; }
        public string Messers { get; set; }
        public string ShipmentNumber { get; set; }
        public string BookingNumber { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        /// <summary>
        /// Exporter's country code + HBL ( HBL not supported yet)
        /// </summary>
        public string TenantCountryCode { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeNameAddress { get; set; }
        public string TenantLogo { get; set; }
        /// <summary>
        /// If notify exists,show notify.Else show importer
        /// </summary>
        public string NotifyAddress { get; set; }
        public string Notify2Address { get; set; }
        public string Notes { get; set; }
        /// <summary>
        /// Place of Receipt
        /// </summary>
        public string ProjectNumber { get; set; }
        public string MasterNumber { get; set; }
        public string PreCarriageCarrierName { get; set; }
        public string ForeignPortOfUnloading { get; set; }

        public bool Containerized { get; set; }
        public bool NotContainerized { get; set; }

        public string SwornDate { get; set; }
        public string TodayDate { get; set; }
        public DateTime? TodayDate_DateTime { get; set; }
        public string MoveType { get; set; } // custom
        public string Remark { get; set; } // custom
        public string EmptyContainer { get; set; } // custom
        public string EmptyContainerRef { get; set; }
        public string EmptyContainerName { get; set; } // custom
        public string EmptyContainerAddress { get; set; } // custom
        public string EmptyContainerReturn { get; set; }
        public string EmptyContainerReturnRef { get; set; }
        public string EmptyContainerReturnName { get; set; }
        public string EmptyContainerReturnAddress { get; set; }
        public string ShipmentType { get; set; }
        public string Branch { get; set; }
        public string StateCode { get; set; }
        public string TenantCountryName { get; set; }
        public string TenantAddress { get; set; }
        public string TenantAddressWithPhone { get; set; }
        public string ETA { get; set; }
        public DateTime? ETA_DateTime { get; set; }
        public string CuttOffDateTime { get; set; }
        public string CuttOffTime { get; set; }
        public DateTime? CuttOffDateTime_Date { get; set; }
        public string ETD { get; set; }
        public string DeliveryAddress { get; set; }
        public string PickUpAddress { get; set; }

        public string PreCarriageFromPort { get; set; }
        public string PreCarriageToPort { get; set; }
        public string OnCarriageToPort { get; set; }
        public string MainCarriageVesselCode { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string MainCarriageVesselNameAndNumber { get; set; }
        public string MainCarriageOBL { get; set; }
        public string LoadingPortName { get; set; }
        public string DischargePortName { get; set; }
        public string PlaceOfReceipt { get; set; }
        public string PlaceOfDelivery { get; set; }
        /// <summary>
        /// Final country and port
        /// </summary>

        public List<PackageLine> PackagesLines { get; set; }
        public List<PackageLine> AttachmentList { get; set; }
        public string TotalQuantity { get; set; }
        public string TotalWeight { get; set; }
        public string TotalVolume { get; set; }
        public string TotalVolumetricWeight { get; set; }
        public string TotalMarks { get; set; }
        public string PrepaidCollect { get; set; }
        /// <summary>
        /// Load Port + main carriage OBL date
        /// </summary>
        public string PlaceAndDateOfIssue { get; set; }
        public string Signature { get; set; }
        public string AgentInfo { get; set; }
        public string CopyOrOriginal { get; set; }//custom
        public string Value { get; set; }//custom
        public string Date { get; set; }
        public string AgentFullDetails { get; set; }

        public string Instructions { get; set; } // custom  
        public string HasAttachmentList { get; set; }
        public string OriginalsOrCopiesNo { get; set; }//custom

        //The following 3 proprties are used when attach list is exist
        public string CustomPackagesNumber { get; set; }
        public string CustomPackageType { get; set; }
        public string CustomWeight { get; set; }
        public string CustomVolume { get; set; }

        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public string MainCarriageETD { get; set; }
        public DateTime? MainCarriageETD_DateTime { get; set; }
        public DateTime? MainCarriageETA_DateTime { get; set; }
        public string MainCarriageETA { get; set; }
        public string OnCarriageETA { get; set; }
        public DateTime? OnCarriageETA_DateTime { get; set; }
        public string FinalDestinationETA { get; set; }
        public DateTime? FinalDestinationETA_DateTime { get; set; }
        public string Broker { get; set; }
        public string BrokerName { get; set; }
        public string BrokerEmail { get; set; }
        public string HouseNumber { get; set; }
        public string LastFreeDate { get; set; }
        public string CarrierNumberLabel { get; set; }
        public string CarrierNumber { get; set; }

        public string SubNumber { get; set; }
        public string AgentFor { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string TenantCity { get; set; }
        public string FinalDestination { get; set; }
        public string GeneralDescriptionOfGoods { get; set; }
        public string GeneralPackageslinesDescriptionOfGoods { get; set; }

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
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation_Label { get; set; }
        public string ToLocation_Label { get; set; }
        public string FinalLocation { get; set; }

        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }
        public string AMSBL { get; set; }
        public string CustomClearencePointCode { get; set; }
        public string CustomClearencePointName { get; set; }
        public string ColoaderRefLable { get; set; }
        public string ColoaderRef { get; set; }

        public string FromLocationCountryCode { get; set; }
        public string ToLocationCountryCode { get; set; }

        public string FromPartnerName { get; set; }
        public string FromPartnerFullAddress { get; set; }
        public string FromPartnerReference { get; set; }
        public string ToPartnerName { get; set; }
        public string ToPartnerFullAddress { get; set; }
        public string ToPartnerReference { get; set; }

        public string ShipperName { get; set; }
        public string ShipperReference { get; set; }

        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment3CarrierNumber { get; set; }

        public string DeliveryOrder { get; set; }
        public string ImportManifest { get; set; }

        public string TransportDocumentNumber { get; set; }
        public string CarrierTransportDocumentNumber { get; set; }
        public string LocalCustomsCode { get; set; }
        public string ShippingAgentLocalCustomsCode { get; set; }

        public List<ReceivableLine> ReceivablesLines { get; set; }
        public string TotalReceivablesAmountInLocal { get; set; }
        public string TotalReceivablesAmountInProfit { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string ProfitCurrencyCode { get; set; }

        public string CustomerVat { get; set; }
        public string VoyageNumber { get; set; }
        public string MainCarriageATA { get; set; }
        public string ShippingAgentName { get; set; }

        public string Dimensions { get; set; }

        public string ContainersNumbersArray { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }

        public string SendersInstructions { get; set; }
        public string CustomsDeclarationNumber { get; set; }

        public string TransportationType { get; set; }

        public string IRSPlace { get; set; }
        public string IRSNumber { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ContactDetails { get; set; }

        public string ConsigneeName { get; set; }
        public string DeliveryTo { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageCarrier_Label { get; set; }
        public string MainCarriageVessel_Label { get; set; }
        public string MainCarriageLastDestination_Label { get; set; }
        public string MainCarriageMAWBOBLBL_Label { get; set; }
        public string MainCarriageMAWBOBLBL { get; set; }
        public string MainCarriageCarrierType_Label { get; set; }
        public string ClientNumber { get; set; }
        public string DeliveryDriverName { get; set; }
        public string DeliveryTruckNumber { get; set; }
        public string DeliveryTrailerNumber { get; set; }
        public string DeliveryATA { get; set; }
        public DateTime? DeliveryETA_DateTime { get; set; }

        public string FreightLocation { get; set; }
        public string FreightLocationLocalName { get; set; }
        public string FreightLocationId { get; set; }
        public string FreightLocationName { get; set; }
        public string FreightLocationAddress { get; set; }
        public string FreightLocationAddressWithPhone { get; set; }
        public string FreightLocationCode { get; set; }

        public string ShipperContactDetails { get; set; }
        public string ConsigneeContactDetails { get; set; }
        public string Notify1ContactDetails { get; set; }
        public string Notify2ContactDetails { get; set; }

        public string LastMainCarriageVesselCode { get; set; }
        public string LastMainCarriageVesselName { get; set; }
        public string LastMainCarriageVesselNameAndNumber { get; set; }
        public string InsidePackagesDetails { get; set; }

        public string PickupTruckerName { get; set; }
        public string PickupTruckerNumber { get; set; }
        public string PickupTruckerInfo { get; set; }
        public string DeliveryTruckerName { get; set; }
        public string DeliveryTruckerInfo { get; set; }

        public string DeliveryToName { get; set; }
        public string DeliveryToAddress { get; set; }

        public string DeliveryToContactName { get; set; }
        public string DeliveryToContactEmail { get; set; }
        public string DeliveryToContactPhone { get; set; }
        public string FirstDeliveryToContactPhone { get; set; }

        public DateTime? FirstPickupETD { get; set; }

        public string FreightForwardedAddress { get; set; }
        public string FreightForwardedAddressWithoutCountry { get; set; }
        public string Incoterm { get; set; }
        public string Salesman { get; set; }
        public string SalesmanEmail { get; set; }
        public string UserPhoneNumber { get; set; }    

     
        public double? TotalPayables { get; set; }
        public string CustomsAgent { get; set; }
        public double? TotalPayablesForMainCarriageCarrier { get; set; }
        public double? TotalPayablesForCustomsAgent { get; set; }
        public double? TotalPayablesForAgent { get; set; }
        public string FirstFrom { set; get; }
        public string LastTo { set; get; }
        public string FirstFromCityCountryZipCodeDetails { set; get; }
        public string LastToCityCountryZipCodeDetails { set; get; }

        public string DeliveryFromName { get; set; }
        public string DeliveryFromAddress { get; set; }

        public DateTime? ETD_DateTime { get; set; }
        public string ValueOfGoodsCurrencyCode { get; set; }
        public string ValueOfGoodsCurrency { get; set; }
        public double? ValueOfGoods { get; set; }

        public string ReleasingAgentName { get; set; }
        public string ReleasingAgentAddress { get; set; }

        public string LoadingPortCode { get; set; }
        public string DischargePortCode { get; set; }
        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public string Transshipment2FromPortCode { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string PreCarriageCarrierAddress { get; set; }
        public DateTime? Transshipment1ETA { get; set; }

        public string BranchAddress { get; set; }

        public string Shipper2 { get; set; }
        public string Shipper3 { get; set; }
        public string Shipper4 { get; set; }
        public string Shipper5 { get; set; }
        public string HAWB2 { get; set; }
        public string HAWB3 { get; set; }
        public string HAWB4 { get; set; }
        public string HAWB5 { get; set; }

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
        public string ShipperRef2 { get; set; }
        public string ConsigneeRef2 { get; set; }

        public string MainCarriageCarrierAddress { get; set; }
        public string ConfirmationNotes { get; set; }

        // Warehouse Fields 
        public string WarehouseLegTerminalName { get; set; }
        public string WarehouseLegAddress { get; set; }
        public string WarehouseLegTerminalCode { get; set; }
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        public DateTime? WarehouseLegCutOffDate { get; set; }
        public DateTime? WarehouseLegVGMCutOffDate { get; set; }
        public string WarehouseLegRemarks { get; set; }
        public string WarehouseLegReference { get; set; }
        public DateTime? WarehouseLegEntryDate { get; set; }
        public DateTime? WarehouseLegReleaseDate { get; set; }
        public string DeliveryNotes { get; set; }
        public bool IsDangerous { get; set; }

        public string TenantName { get; set; }
        public string TenantPhone { get; set; }
        public string IssuedByUser { get; set; }
        public string IssuedByUserPosition { get; set; }

        public DateTime? MainCarriageATD { get; set; }

        public string ShipperVAT { get; set; }
        public string ConsigneeVAT { get; set; }
        public string AgentVAT { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByUserEmail { get; set; }
        public List<PackageLine> DangerousPackages { get; set; }
        public List<PayableLine> PayablesLines { get; set; }
        public List<PickUpDeliveryLine> PickUpsLines { get; set; }
        public List<PickUpDeliveryLine> DeliveriesLines { get; set; }
        public string MasterInternalNumber { get; set; }
        public string CompleteShipmentType { get; set; }

        public string DeliveryToContactMobile { get; set; }

        public string PlaceOfReceiptCountryCode { get; set; }
        public string PlaceOfReceiptCountryName { get; set; }
        public string PlaceOfReceiptStateCode { get; set; }
        public string PlaceOfDeliveryCountryCode { get; set; }
        public string PlaceOfDeliveryCountryName { get; set; }
        public string PlaceOfDeliveryStateCode { get; set; }

        public string NotifyContactDetails { get; set; }
        public string FullPickupAddress { get; set; }
        public string PickupFromPartnerName { get; set; }
        public double? ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }

        public string PickupTo { get; set; }
     
        public string IssuingCarrierAgentName { get; set; }
        public string ARInvoices { get; set; }
        public string SpecialServicesTypeName { get; set; }

        public string TenantCBSA { get; set; }
        public string TenantCAAT { get; set; }
        public string CarrierCBSA { get; set; }
        public string CarrierCAAT { get; set; }
        public string InlandDriver { get; set; }
        public string ShipperAddress_WithName { get; set; }
        public string ShipperNotExporterAddress_WithName { get; set; }
        public string ConsigneeAddress_WithName { get; set; }
        public string NotifyAddress_WithName { get; set; }
        public string Notify2Address_WithName { get; set; }
        public string PickUpInstructions { get; set; }
        public string DeliveryInstructions { get; set; }
        public string ConnectedQuoteNumber { get; set; }

        public byte[] MainCarriageCarrierLogo { get; set; }
        public string DischargePortStateCode { get; set; }
        public string TotalContainers { get; set; }
        public DateTime? FirstPickupETA { get; set; }
        public string MasterPreCarriageCarrierNumber { get; set; }
        public string MasterPreCarriageVesselName { get; set; }

        public string TrailerNumber { get; set; }
        public DateTime? MainCarriageATADateTime { get; set; }
        public string OriginCountryName { get; set; }

        public string Transhipment1Vessel { get; set; }
        public string Transhipment2Vessel { get; set; }
        public string Transhipment3Vessel { get; set; }
        public DateTime? Transhipment3ETA { get; set; }
        public DateTime? Transhipment1ETD { get; set; }
        public DateTime? Transhipment3ETD { get; set; }
        public string ShipmentSubTypeName { get; set; }

        public string Transshipment1FromPortName { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Notify1ReferenceNumber { get; set; }

        public int? StorageFreeDays { get; set; }
        public string MasterPreCarriageFromPortName { get; set; }
        public string MasterProjectNumber { get; set; }
        public string ConsigneeNotImporter { get; set; }
        public string Transshipment1ETA_String { get; set; }
        public string Transshipment1ETD_String { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }

        //To show the first pickup address
        //If no pickup, show pickup/delivery address of the shipper
        //If no pickup/delivery address, show the chosen address on the shipper
        public string PickUpAddress_New { get; set; }       

        //To show the last delivery address
        //If no delivery, show pickup/delivery address of the consignee
        //If no pickup/delivery address, show the chosen address on the consignee
        public string DeliveryAddress_New { get; set; }

        public string AWBCommodityItemNumber { get; set; }
    }
}
