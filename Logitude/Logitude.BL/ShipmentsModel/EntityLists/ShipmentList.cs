using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }

        #region Financial properties
        public double? OpenReceivablesInLocalCurrency { get; set; }
        public double? AccountedReceivablesInLocalCurrency { get; set; }
        public double ProfitInLocalCurrency { get; set; }
        public double? EstimateProfitInLocalCurrency { get; set; }
        public double? OpenReceivablesInProfitCurrency { get; set; }
        public double? AccountedReceivablesInProfitCurrency { get; set; }
        public double? ProfitInProfitCurrency { get; set; }
        public double? EstimateProfitInProfitCurrency { get; set; }
        public double? OpenPayablesInLocalCurrency { get; set; }
        public double? AccountedPayablesInLocalCurrency { get; set; }
        public double? OpenPayablesInProfitCurrency { get; set; }
        public double? AccountedPayablesInProfitCurrency { get; set; }
        #endregion
        
        public string LocalCustomsTransmissionsStatusCode { get; set; }
        public string LocalCustomsTransmissionsStatusName { get; set; }
        public string LocalCustomsTransmissionsStatusError { get; set; }
        public DateTime? LocalCustomsTransmissionsStatusDate { get; set; }
        public string LocalCustomsSentByUserId { get; set; }
        public string LocalCustomsSentByUserName { get; set; }
        public string OperationalClosedByUserId { get; set; }
        public string ComputedStatusId { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public  string ComputedStatusName { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public string CarrierNumber { get; set; }
        public string CASSCode { get; set; }
        public bool IsFSRSent { get; set; }
        public DateTime? LastFSRStatusRequestDate { get; set; }
        public DateTime? FHLStatusDate { get; set; }
        public DateTime? FWBStatusDate { get; set; }
        public string CarrierLastStatusCode { get; set; }
        public string CarrierLastStatusName { get; set; }
        public DateTime? CarrierLastStatusDate { get; set; }
        public string FNAReason { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? LastStatusLogDate { get; set; }
        public string CustomFileId { get; set; }
        public bool IsShipmentTracking { get; set; }         
        public string CustomFileNumber { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string TruckNumber { get; set; }
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string AgentComputed { get; set; }
        public string AgentComputedName { get; set; }

        public string CustomFieldId { get; set; }
        public string FromPortId { get; set; }
        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }
        public string ToPortId { get; set; }
        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCode { get; set; }
        public string FromPortCode { get; set; }


        
        public string ToPortCountry { get; set; }
        public string AMSBL { get; set; }
        public string House { get; set; }
        public string ShipmentType { get; set; }
        public string ShipmentTypeId { get; set; }
        public string FollowUpType { get; set; }
        public string FollowUpTypeId { get; set; }
        public string FollowUpId { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpOwner { get; set; }
        public string FollowUpOwnerId { get; set; }
        public bool CustomConnectToShipment { get; set; }
        public string MainCarriageVesselId { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string BookingConfirmationNumber { get; set; }

        [Key]
        public string ShipmentViewId { get; set; }
        public string BasketId { get; set; }
        public bool IsAccountingClosed { get; set; }
        public bool IsOperationalClosed { get; set; }
        public DateTime LastUpdate { get; set; }

        public string ComputedShipmentNumber { get; set; }

        public string Field1Id { get; set; }     
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string Field14 { get; set; }
        public string Field15 { get; set; }
        public string Field16 { get; set; }
        public string Field17 { get; set; }
        public string Field18 { get; set; }
        public string Field19 { get; set; }
        public string Field20 { get; set; }
        public string Field21 { get; set; }
        public string Field22 { get; set; }
        public string Field23 { get; set; }
        public string Field24 { get; set; }
        public string Field25 { get; set; }
        public string Field26 { get; set; }
        public string Field27 { get; set; }
        public string Field28 { get; set; }
        public string Field29 { get; set; }
        public string Field30 { get; set; }
        public string Field31 { get; set; }
        public string Field32 { get; set; }
        public string Field33 { get; set; }
        public string Field34 { get; set; }
        public string Field35 { get; set; }
        public string Field36 { get; set; }
        public string Field37 { get; set; }
        public string Field38 { get; set; }
        public string Field39 { get; set; }
        public string Field40 { get; set; }

        public bool NewMessage { get; set; }
       
        public string FollowUpNotes { get; set; }
        public bool IsAnyConversation { get; set; }
        public int NumberOfShipments { get; set; }
        public DateTime? MainCarriageETA{ get; set; }
        public DateTime? MainCarriageATD { get; set; }        
        public double? ChargeableWeightInKG { get; set; }
        public byte[] LastModified { get; set; }
        public bool hasChanges { get; set; }

        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string ExactStatusName { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusLocation { get; set; }

        public string Master { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeName { get; set; }
        public string QuoteId { get; set; }
        public string QuoteNumber { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public string ShipmentPayableStatusCode { get; set; }

        public string ShipmentReceivableStatusName { get; set; }
        public string ShipmentPayableStatusName { get; set; }

        public string UpdatedByUserId { get; set; }
        public DateTime? LastUpdateDate { get; set; }
      
        public string DepartmentId { get; set; }
        public string BranchId { get; set; }

        public string LongMaster { get; set; }
        
        public string TrailerNumber { set; get; }


        public string ProfitCurrencyCode { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string NextLegCode { get; set; }
        public string NextLegName { get; set; }
        public DateTime? NextETD { get; set; }
        public DateTime? NextETA { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string MasterShipmentDataId { get; set; }
        public string BranchName { get; set; }
        public string MoveTypeName { get; set; }

        public double? GrossWeightInKG { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? GrossWeightPerStorageDays { get; set; }

        public double? ChargeableWeight { get; set; }
        public double? GrossWeight { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string Routing { get; set; }
        public string AgentName { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public string SearchFields { get; set; }
        public string SearchFieldsText { get; set; }
        public DateTime? CutoffDate { get; set; }

        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string MainCarriageFromPortName { get; set; } // origin
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public string IncotermId { get; set; }
        public string IncotermCode { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public DateTime? EstimatedFinalArrivalDate { get; set; }
        public DateTime? ActualFinalArrivalDate { get; set; }

        public string IssuingCarrierAgentId { get; set; }
        public double? AWBChargeAmount { get; set; }
        public string AWBCommodityItemNumber { get; set; }

        public bool AWBPrint { get; set; }
        public string FWBStatusCode { get; set; }
        public string FHLStatusCode { get; set; }
        public string FWBStatusName { get; set; }
        public string FHLStatusName { get; set; }

        public string MainCarriageFromPartnerId { get; set; }
        public string MainCarriageFromAddressId { get; set; }
        public string MainCarriageToPartnerId { get; set; }
        public string MainCarriageToAddressId { get; set; }

        public string MainCarriageCarrierPrefix { get; set; }
        public string Transshipment1CarrierPrefix { get; set; }
        public string Transshipment2CarrierPrefix { get; set; }
        public string Transshipment3CarrierPrefix { get; set; }

        public string MainCarriageFullCarrierNumber { get; set; }
        public string Transshipment1FullCarrierNumber { get; set; }
        public string Transshipment2FullCarrierNumber { get; set; }
        public string Transshipment3FullCarrierNumber { get; set; }
        public bool AsAgreedFreight { get; set; }
        public bool AsAgreedOtherCharges { get; set; }
        public string AccountNumber { get; set; }

        public DateTime? ActivityDate { get; set; }
        public string ActivityTypeName { get; set; }
        public string ActivityByUserName { get; set; }


        public bool IsManifestSentToAgent { get; set; }
        public string AgentSharedManifestRef { get; set; }


        public string Shipper { get; set; }
        public string ShipperId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public string Consignee { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }

        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }
        public double? TEU { get; set; }

        public string ChargeableWeightUnitCode { get; set; }
        public int? PackagesQuantity { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }    
        
        
        public string ConsigneeId { get; set; }
        public double? VolumeInCBM { get; set; }

        public int? BookingNumberOfPackages { get; set; }
        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }

        public string MainCarriageFromCity { get; set; }
        public string MainCarriageFromCountryCode { get; set; }

        public string MainCarriageToCity { get; set; }
        public string MainCarriageToCountryCode { get; set; }

        public double? ProfitExchangeRate { get; set; }
        public string AWBCurrencyCode { get; set; }

        //public string ToPortNameFinalDestination { get; set; }
        //public string ToCountySRCFinalDestination { get; set; }
       
        public string DeliveryOrder { get; set; }
        public string ImportManifest { get; set; }
        public string FreightLocationId { get; set; }
        public string TransportDocumentNumber { get; set; }
        public string CarrierTransportDocumentNumber { get; set; }
        public bool IsMultipleCommodities { get; set; }

        public string FreelancerId { get; set; }
        public string FreelancerAddressId { get; set; }
        public string FreelancerContactId { get; set; }

        public string SpecialServicesTypeId { get; set; }
       
        public string SpecialServicesTypeName { get; set; }

        public string FromPortCountryCode { get; set; }
        public string FromPortCountryName { get; set; }
        public string ToPortCountryCode {get; set;}
        public string ToPortCountryName { get; set; }
        public string AgentId { get; set; }
        public bool ARInvoiceIssued { get; set; }
        public bool CreditNoteIssued { get; set; }
        public string FreightForwarderId { get; set; }
        public string FreightForwarderName { get; set; }
        public string ProductCode { get; set; }
        public bool IsOccurChange { get; set; }
        public string CargonautFHLStatusCode { get; set; }
        public string CargonautFHLStatusName { get; set; }
        public DateTime? CargonautFHLStatusDate { get; set; }
        public string CargonautFWBStatusCode { get; set; }
        public string CargonautFWBStatusName { get; set; }
        public DateTime? CargonautFWBStatusDate { get; set; }
        public int NumberOfInsidePackages { get; set; }
        public string NumberOfInsidePackagesDetails { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }

        public string ConsolidatorId { get; set; }
        public string ConsolidatorName { get; set; }
        public string ConsolidatorNote { get; set; }
        public string ConsolidatorReference { get; set; }
        public string ConsolidatorAddressId { get; set; }
        public string ConsolidatorContactId { get; set; }

        public string AccountManagerUserId { get; set; }
        public string AccountManagerUserName { get; set; }

        public string SalesmanUserId { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreForwardingETD { get; set; }
        public bool IsException { get; set; }

        public string SalesmanUserName { get; set; }
        public string CreatedByUserName { get; set; }

        public string AirlinePrefix { get; set; }
        public string ManifestReason { get; set; }
        public string ManifestStatusCode { get; set; }
        public string ExceptionDescription { get; set; }
        public string ExceptionResolvedDescription { get; set; }
        public string LastExceptionDescription { get; set; }
        public DateTime? ExceptionDate { get; set; }
        public bool HasException { get; set; }
        public bool IsMissingDocument { get; set; }
        public string DocumentsSearchFields { get; set; }

        public string ShipperName{ get; set; }
        public string ConsigneeName { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public string CustomerShipmentNumber { get; set; }
        public string CustomsDeclarationNumber { get; set; }
        public bool ResultFromDocument { get; set; }
        public bool IncludesCustoms { get; set; }
        public string DeclarationNumber { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }

        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool NoFreightFile { get; set; }
        public string ShipmentMasterDataId { get; set; }

        public DateTime? OperationalCloseDate { get; set; }
        public DateTime? AccountingCloseDate { get; set; }

        public DateTime? LastDocumentDateTime { get; set; }

        public DateTime? MainCarriageExpectedOrActual { get; set; }
        public string MainCarriageETAOrATA { get; set; }
            
        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortCountryName { get; set; }
        public string MainCarriageFromPortCountryCode { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageToPortName { get; set; }
        public string MainCarriageToPortCountryCode { get; set; }
        public string MainCarriageToPortCountryName { get; set; }
        public string PartnerLogoId { get; set; }

        public int MissingDocumentsCount { get; set; }
        public string MissingDocumentsCountWords { get; set; }

        public string PartnerName { get; set; }
        public string MissingDocsNames { get; set; }
    
        public int? NumberOfFollowUps { get; set; }

        public int? CustomerTenantNumber { get; set; }
        public DateTime? MainCarriageFinalDestinationETA { get; set; }
        public DateTime? MainCarriageFinalDestinationATA { get; set; }
        public DateTime? DepartureArrivalFromDate { get; set; }
        public DateTime? DepartureArrivalToDate { get; set; }

        public bool IsArchived { get; set; }

        public string ArchivedText { get; set; }
        public int RequestedDocumentsCount { get; set; }
        public bool IsRequestedDocuments { get; set; }
        public bool IsDigitalSignRequired { get; set; }

        public string DescriptionOfGoods { get; set; }
        public string Notes { get; set; }

        public bool IsImporterApprovalRequried { get; set; }
        public string ApprovedByUserName { get; set; }
        public bool IsNewARInvoiceBlocked { get; set; }
        public DateTime? OperationalDate { get; set; }
        public string MobileShipmentReference { get; set; }
        
        public double? ValueOfGoods { get; set; }
        public int NumberOfHouses { get; set; }
        public bool ProrateReceivables { get; set; }

        public DateTime? FreightRelease { get; set; }
        public DateTime? TerminalAvailable { get; set; }
        public string ISFNumber { get; set; }
        public DateTime? ISFDate { get; set; }
        public string ITNumber { get; set; }
        public DateTime? ITDate { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public string OBLTypeCode { get; set; }
        
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }
        public DateTime? RegistryDate { get; set; }
        public bool IsAssembly { get; set; }
        
        public string LastSharedEventId { get; set; }
        public string LastSharedEventLocation { get; set; }
        public string LastSharedEventNotes { get; set; }
        public DateTime? LastSharedEventDate { get; set; }
        public string LastSharedEventName { get; set; }

        public double? GrossWeightPerTon { get; set; }
        public DateTime? ManifestLastSharingDate { get; set; }
        //public string MainCarriageFinalDestinationPortId { get; set; }
        public string MainCarriageFinalDestinationPortCode { get; set; }
        //public string MainCarriageFinalDestinationPortName { get; set; }
        //public string MainCarriageFinalDestinationCountryCode { get; set; }
        //public string MainCarriageFinalDestinationCountryName { get; set; }

        #region WarehouseLeg
        public string WarehouseLegWarehouseId { get; set; }
        public string WarehouseLegAddressId { get; set; }
        public string WarehouseLegTerminalCode { get; set; }
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        public string WarehouseLegRemarks { get; set; }
        public string WarehouseLegReference { get; set; }
        public string WarehouseLegTerminalName { get; set; }
        public DateTime? WarehouseLegEntryDate { get; set; }
        public DateTime? WarehouseLegReleaseDate { get; set; }
        public string WarehouseLegAddressCountryName { get; set; }
        public string WarehouseLegAddressCountryCode { get; set; }
        public DateTime? WarehouseLegVGMCutOffDate { get; set; }
        public DateTime? WarehouseLegCutOffDate { get; set; }
        #endregion 

        public DateTime? FirstOperationalCloseDate { get; set; }
        public DateTime? FirstAccountingCloseDate { get; set; }
        public DateTime? AMSClosingDate { get; set; }
        public string UpdatedByPartner { get; set; }

        public string INTTRASIError { get; set; }
        public DateTime? INTTRASIStatusDate { get; set; }
        public string INTTRASIStatusCode { get; set; }
        public string INTTRASIStatusName { get; set; }
        public string EmergencyContactId { get; set; }
        public string INTTRAContractNumber { get; set; }
        public string INTTRAInstructions { get; set; }
        public string INTTRAComments { get; set; }
        public int INTTRADocumentQTY { get; set; }
        public bool SIHasAttachList { get; set; }
        public bool INTTRAIsFreighted { get; set; }
        public string INTTRADocumentTypeCode { get; set; }
        public string INTTRABookingTransStatusCode { get; set; }
        public string INTTRABookingStatusCode { get; set; }
        public string INTTRABookingTransStatusName { get; set; }
        public string INTTRABookingStatusName { get; set; }
        public DateTime? INTTRALastEBbookingSendDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Origin { get; set; }

        public string LastFinalDestination { get; set; }
        public DateTime? FirstPickupETD { get; set; }
        public DateTime? FirstPickupETA { get; set; }
        public DateTime? INTTRALastStatusDate { get; set; }

        public string INTTRABookingError { get; set; }
        public string INTTRALastBookingResponse { get; set; }
       

        public string Notify1Reference { get; set; }
        public string Notify2Reference { get; set; }
        public string ShipperNotExporterReference { get; set; }
        public string ConsigneeNotImporterReference { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? ContainerLastStatusDate { get; set; }

        public string BasicFreightId { get; set; }
        public string DestinationPortChargesId { get; set; }
        public string DestinationHaulageChargesId { get; set; }
        public string AdditionalChargesId { get; set; }
        public string FreightPayerId { get; set; }
        public string FreightPayerAddressId { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }

        public bool IsDepositionRequired { get; set; }
        public string ImporterDepositionRequestDetails { get; set; }
        public string ForwarderPartnerId { get; set; }
        public string ARInvoices { get; set; }
        public double? NotInvoicedReceivablesAmount { get; set; }
        public string CreatedByPartner { get; set; }
        public DateTime? FirstARInvoiceApprovalDate { get; set; }
        public string SLAC { get; set; }
        public bool CreatedFromDigital { get; set; }
        public string ShipmentSubTypeId { get; set; }
        public string ShipmentSubTypeName { get; set; }

        public bool IsDangerous { get; set; }
        public string DangerousUnNumber { get; set; }
        public bool IsAccrualsApproved { get; set; }
        public DateTime? AccrualsApprovalDate { get; set; }

        public string MainHarmonize { get; set; }

        public string PreForwardingFromPortId { get; set; }
        public string OnForwardingToPortId { get; set; }
        public bool IsStandalonePickupDelivery { get; set; }
        public DateTime? ExpectedCargoReadyDate { get; set; }
        public DateTime? ActualCargoReadyDate { get; set; }
        public string HandlerUserId { get; set; }
        public bool IsHTSMissing { get; set; }
    }
}
