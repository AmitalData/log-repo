using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class Shipment
    {
        private string id;
        [Key]
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string LocalCustomsTransmissionsStatusCode { get; set; }
        public string LocalCustomsTransmissionsStatusError { get; set; }
        public DateTime? LocalCustomsTransmissionsStatusDate { get; set; }

        public bool IncludesCustoms { get; set; }
        public string DeclarationNumber { get; set; }
        public DateTime? DeclarationDate { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }
        public string LocalCustomsSentByUserId { get; set; }
        public virtual User LocalCustomsSentByUser { get; set; }

        //[Timestamp]
        // public byte[] LastModified { get; set; }
        public string ConcurrencyGUID { get; set; }

        #region Fields
        public string CountryForStatisticsId { get; set; }
        public double? ProfitExchangeRate { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public DateTime? EstimatedFinalArrivalDate { get; set; }
        public DateTime? ActualFinalArrivalDate { get; set; }
        public string Routing { get; set; }
        public string CarrierLastStatusCode { get; set; }
        public DateTime? CarrierLastStatusDate { get; set; }
        public string FNAReason { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? GrossWeight { get; set; }
        public string CurrentUserId { get; set; }
        public int Tenant { get; set; }
        public string BasketId { get; set; }

        public DateTime? LastStatusLogDate { get; set; }

        public bool CustomConnectToShipment { get; set; }

        public string CustomFileId { get; set; }

        public string CustomFileNumber { get; set; }

        public string ShipmentNumber { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentTypeId { get; set; }
        public string House { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string BranchId { get; set; }
        public string IncotermId { get; set; }
        public string SalesmanUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string OperationalClosedByUserId { get; set; }
        
        public string DepartmentId { get; set; }
        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }
        public DateTime? HAWBDate { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? VolumeInCBM { get; set; }
        public int? PackagesQuantity { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }
        public double? Ratio { get; set; }
        public double? DimFactor { get; set; }
        public bool IsOperationalClosed { get; set; }
        public bool GrossWeightEdited { get; set; }
        public bool ChargeableWeightEdited { get; set; }
        public string VolumeUnitCode { get; set; }

        public string StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusLocation { get; set; }

        public string MainHarmonize { get; set; }
        public bool IsDangerous { get; set; }
        public string DangerousClassNumber { get; set; }
        public string DangerousUnNumber { get; set; }
        public string DangerousPackagingGroup { get; set; }
        public string DangerousIMDGCode { get; set; }
        public string DangerousFlashPoint { get; set; }
        public string DangerousMaterialDescription { get; set; }
        public bool LTCWEdited { get; set; }
        public int ShipmentPickUpIndex { get; set; }
        public int ShipmentDeliveryIndex { get; set; }
        public int ShipmentContainerReturnIndex { get; set; }

        public string QuoteId { get; set; }
        public string BookingId { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsAccountingClosed { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public string ShipmentPayableStatusCode { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime? CancelledDate { get; set; }

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

        public string ProfitCurrencyId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string NextLegCode { get; set; }
        public DateTime? NextETD { get; set; }
        public DateTime? NextETA { get; set; }
        public string MasterShipmentDataId { get; set; }
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

        public string SearchFields { get; set; }
        public DateTime? CutoffDate { get; set; }
        public double? Volume { get; set; }

        public string AWBSpecialHandlingCodeId1 { get; set; }
        public string AWBSpecialHandlingCodeId2 { get; set; }
        public string AWBSpecialHandlingCodeId3 { get; set; }
        public string AWBSpecialHandlingCodeId4 { get; set; }
        public string AWBSpecialHandlingCodeId5 { get; set; }
        public string AWBSpecialHandlingCodeId6 { get; set; }
        public string AWBSpecialHandlingCodeId7 { get; set; }
        public string AWBSpecialHandlingCodeId8 { get; set; }
        public string AWBSpecialHandlingCodeId9 { get; set; }


        public bool AsAgreedFreight { get; set; }
        public bool AsAgreedOtherCharges { get; set; }
        public string AccountNumber { get; set; }

        public bool IsFSRSent { get; set; }
        public DateTime? LastFSRStatusRequestDate { get; set; }
        #endregion

        public string ExceptionDescription { get; set; }
        public string ExceptionResolvedDescription { get; set; }

        public bool IsManifestSentToAgent { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public DateTime? ManifestLastSharingDate { get; set; }

        public DateTime? ExceptionDate { get; set; }
        public bool HasException { get; set; }
        public string LastExceptionDescription { get; set; }

        public string ComputedStatusId { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public string CASSCode { get; set; }

        public string AccountManagerUserId { get; set; }

        #region Partners
        public string IssuingCarrierIATACode { get; set; }
        public string IssuingCarrierAgentId { get; set; }
        public string IssuingCarrierAddressId { get; set; }
        public string IssuingCarrierReference1 { get; set; }
        public string ShipmentCustomerTypeCode { get; set; }
        public bool ConsigneeAddressOneTime { get; set; }
        public bool ShipperAddressOneTime { get; set; }

        public string CustomerId { get; set; }
        public string CustomerAddressId { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }

        public string FreightForwarderId { get; set; }
        public string FreightForwarderAddressId { get; set; }
        public string FreightForwarderContactId { get; set; }
        public string FreightForwarderReference { get; set; }

        public string ShipperId { get; set; }
        public string ShipperAddressId { get; set; }
        public string ShipperContactId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public string ConsigneeId { get; set; }
        public string ConsigneeAddressId { get; set; }
        public string ConsigneeContactId { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }

        public string AgentId { get; set; }
        public string AgentAddressId { get; set; }
        public string AgentContactId { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }

        // Computed Agent
        public string AgentComputed { get; set; }

        public string CustomAgentExportId { get; set; }
        public string CustomAgentExportAddressId { get; set; }
        public string CustomAgentExportContactId { get; set; }
        public string CustomAgentExportReference { get; set; }

        public string CustomAgentImportId { get; set; }
        public string CustomAgentImportAddressId { get; set; }
        public string CustomAgentImportContactId { get; set; }
        public string CustomAgentImportReference { get; set; }

        public string Notify1Id { get; set; }
        public string Notify1AddressId { get; set; }
        public string Notify1ContactId { get; set; }
        public string Notify1Reference { get; set; }

        public string Notify2Id { get; set; }
        public string Notify2AddressId { get; set; }
        public string Notify2ContactId { get; set; }
        public string Notify2Reference { get; set; }

        public string ShipperNotExporterId { get; set; }
        public string ShipperNotExporterAddressId { get; set; }
        public string ShipperNotExporterContactId { get; set; }
        public string ShipperNotExporterReference { get; set; }

        public string ConsigneeNotImporterId { get; set; }
        public string ConsigneeNotImporterAddressId { get; set; }
        public string ConsigneeNotImporterContactId { get; set; }
        public string ConsigneeNotImporterReference { get; set; }

        public string FreelancerId { get; set; }
        public string FreelancerAddressId { get; set; }
        public string FreelancerContactId { get; set; }

        public string ConsolidatorId { get; set; }
        public string ConsolidatorAddressId { get; set; }
        public string ConsolidatorContactId { get; set; }
        public string ConsolidatorReference { get; set; }
        public virtual Card ConsolidatorCard { get; set; }
        public virtual Contact ConsolidatorContact { get; set; }
        public virtual Address ConsolidatorAddress { get; set; }

        public string ReleasingAgentId { get; set; }
        public string ReleasingAgentAddressId { get; set; }
        public string ReleasingAgentContactId { get; set; }
        public string ReleasingAgentReference1 { get; set; }
        public string ReleasingAgentReference2 { get; set; }
        #endregion

        #region Routings

        public string FromPortId { get; set; }
        public string ToPortId { get; set; }

        public string PreCarriageTransportModeId { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string PreCarriageToPortId { get; set; }
        public string PreCarriageCarrierId { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public string PreCarriageVesselId { get; set; }

        public string OnCarriageTransportModeId { get; set; }
        public string OnCarriageFromPortId { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnCarriageCarrierId { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        public string OnCarriageVesselId { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Origin { get; set; }
        public string LastFinalDestination { get; set; }

        #endregion

        #region Booking
        public double? OrderGrossWeight { get; set; }
        public double? BookingVolume { get; set; }
        public int? BookingNumberOfPackages { get; set; }
        public bool OrderIsDangerouseGoods { get; set; }
        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }
        #endregion

        #region AWB
        public string AWBChargesCodeCode { get; set; }
        public string AWBCurrencyId { get; set; }
        public double? AWBFreightAmountPrepaid { get; set; }
        public double? AWBFreightAmountCollect { get; set; }
        public string AWBCarrierTarrifReference { get; set; }
        public string AWBDeclaredValueForCarriage { get; set; }
        public string AWBDeclaredValueForCustoms { get; set; }
        public string AWBAccountingInformation { get; set; }
        public string AWBInsurrenceValue { get; set; }
        public string AWBHandlingInformation { get; set; }
        public string SCI { get; set; }
        public string AWBComments { get; set; }
        public string AWBPrintingComments { get; set; }
        public string AWBSignature { get; set; }
        public string AWBPlace { get; set; }
        public string FHLStatusCode { get; set; }
        public bool AWBPrint { get; set; }
        public DateTime? FHLStatusDate { get; set; }

        #endregion

        #region Objects
        public Country CountryForStatistics { get; set; }
        public AWBStatus CarrierLastStatus { get; set; }
        public FHLStatus FHLStatus { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode1 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode2 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode3 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode4 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode5 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode6 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode7 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode8 { get; set; }
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode9 { get; set; }
        public virtual Card IssuingCarrierAgent { get; set; }
        public virtual Address IssuingCarrierAddress { get; set; }
        public virtual AWBChargesCode AWBChargesCode { get; set; }
        public virtual ShipmentLevel ShipmentLevel { get; set; }
        public virtual Port FromPort { get; set; }
        public virtual Port ToPort { get; set; }
        public ShipmentMasterData ShipmentMasterData { get; set; }
        public virtual NextLeg NextLeg { get; set; }
        public virtual Currency ProfitCurrency { get; set; }
        public virtual Card CustomerCard { get; set; }
        public virtual Address CustomerAddress { get; set; }
        public virtual Contact CustomerContact { get; set; }
        public virtual Card CustomAgentExportCard { get; set; }
        public virtual Card FreightForwarderCard { get; set; }
        public virtual Address CustomAgentExportAddress { get; set; }
        public virtual Address FreightForwarderAddress { get; set; }
        public virtual Contact CustomAgentExportContact { get; set; }
        public virtual Contact FreightForwarderContact { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual ShipmentReceivableStatus ShipmentReceivableStatus { get; set; }
        public virtual ShipmentPayableStatus ShipmentPayableStatus { get; set; }
        public virtual EntityStatus EntityStatus { get; set; }


        public virtual EntityStatus ComputedEntityStatus { get; set; }

        public VolumeUnit VolumeUnit { get; set; }
        public virtual WeightUnit GrossWeightUnit { get; set; }
        public virtual WeightUnit ChargeableWeightUnit { get; set; }
        public DimensionsUnit DimensionsUnit { get; set; }
        public Vessel OnCarriageVessel { get; set; }
        public Vessel PreCarriageVessel { get; set; }
        public virtual Address ConsigneeNotImporterAddress { get; set; }
        public virtual Address ShipperNotExporterAddress { get; set; }
        public virtual Contact ConsigneeNotImporterContact { get; set; }
        public virtual Contact ShipperNotExporterContact { get; set; }
        public PrepaidCollect OtherPrepaidCollect { get; set; }
        public PrepaidCollect FreightPrepaidCollect { get; set; }
        public virtual Currency AWBCurrency { get; set; }
        public virtual Card ShipperCard { get; set; }
        public virtual Card AgentCard { get; set; }
        public virtual Card CustomAgentImportCard { get; set; }
        public virtual Card Notify1Card { get; set; }
        public virtual Card Notify2Card { get; set; }
        public virtual Card ConsigneeCard { get; set; }
        public virtual Card ShipperNotExporterCard { get; set; }
        public virtual Card ConsigneeNotImporterCard { get; set; }
        public virtual Card ReleasingAgentCard { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Incoterm Incoterm { get; set; }
        ////[ForeignKey("AWBCurrencyId")]
        //public virtual Currency Currency { get; set; }
        //[ForeignKey("SalesmanUserId")]
        public virtual User SalesmanUser { get; set; }
        //[ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        public virtual User OperationalClosedByUser { get; set; }



        //[ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        //[ForeignKey("ShipmentTypeId")]
        public virtual ShipmentType ShipmentType { get; set; }
        //[ForeignKey("ShipperAddressId")]
        public virtual Address ShipperAddress { get; set; }
        //[ForeignKey("ConsigneeAddressId")]
        public virtual Address ConsigneeAddress { get; set; }

        //[ForeignKey("TransportModeId")]
        public virtual TransportMode TransportMode { get; set; }

        //[ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }

        //[ForeignKey("PreCarriageTransportModeId")]
        public virtual TransportMode PreCarriageTransportMode { get; set; }
        //[ForeignKey("PreCarriageFromPortId")]
        public virtual Port PreCarriageFromPort { get; set; }
        //[ForeignKey("PreCarriageToPortId")]
        public virtual Port PreCarriageToPort { get; set; }
        //[ForeignKey("PreCarriageCarrierId")]
        public virtual Card PreCarriageCarrierCard { get; set; }

        //[ForeignKey("OnCarriageTransportModeId")]
        public virtual TransportMode OnCarriageTransportMode { get; set; }
        //[ForeignKey("OnCarriageFromPortId")]
        public virtual Port OnCarriageFromPort { get; set; }
        //[ForeignKey("OnCarriageToPortId")]
        public virtual Port OnCarriageToPort { get; set; }
        //[ForeignKey("OnCarriageCarrierId")]
        public virtual Card OnCarriageCarrierCard { get; set; }
        //[ForeignKey("AgentContactId")]
        public virtual Contact AgentContact { get; set; }
        //[ForeignKey("ShipperContactId")]
        public virtual Contact ShipperContact { get; set; }
        //[ForeignKey("Notify1ContactId")]
        public virtual Contact Notify1Contact { get; set; }
        //[ForeignKey("Notify2ContactId")]
        public virtual Contact Notify2Contact { get; set; }
        //[ForeignKey("CustomAgentImportContactId")]
        public virtual Contact CustomAgentImportContact { get; set; }
        //[ForeignKey("ConsigneeContactId")]
        public virtual Contact ConsigneeContact { get; set; }
        //[ForeignKey("AgentAddressId")]
        public virtual Address AgentAddress { get; set; }
        //[ForeignKey("Notify1AddressId")]
        public virtual Address Notify1Address { get; set; }
        //[ForeignKey("Notify2AddressId")]
        public virtual Address Notify2Address { get; set; }
        //[ForeignKey("CustomAgentImportAddressId")]
        public virtual Address CustomAgentImportAddress { get; set; }
        //[ForeignKey("ShipmentCustomerTypeCode")]
        public virtual ShipmentCustomerType ShipmentCustomerType { get; set; }

        [ForeignKey("AccountManagerUserId")]
        public virtual User AccountManagerUser { get; set; }

        public virtual Address ReleasingAgentAddress { get; set; }
        public virtual Contact ReleasingAgentContact { get; set; }
        #endregion

        public double? TEU { get; set; }

        public string MoveTypeId { get; set; }

        //[ForeignKey("MoveTypeId")]
        public virtual MoveType MoveType { get; set; }
        public string AMSBL { get; set; }

        public string SecurityKey { get; set; }

        //[ForeignKey("CustomClearancePointId")]
        public virtual Card CustomClearancePoint { get; set; }
        //[ForeignKey("CustomClearancePointAddressId")]
        public virtual Address CustomClearancePointAddress { get; set; }
        //[ForeignKey("CustomClearancePointContactId")]
        public virtual Contact CustomClearancePointContact { get; set; }
        public string CustomClearancePointId { get; set; }
        public string CustomClearancePointAddressId { get; set; }
        public string CustomClearancePointContactId { get; set; }
        public string CustomClearancePointReference1 { get; set; }
        public virtual Card Coloader { get; set; }
        public virtual Card FreelancerCard { get; set; }
        public virtual Address FreelancerAdress { get; set; }
        public virtual Contact FreelancerContact { get; set; }
        public virtual Address ColoaderAddress { get; set; }
        public virtual Contact ColoaderContact { get; set; }
        public string ColoaderId { get; set; }
        public string ColoaderAddressId { get; set; }
        public string ColoaderContactId { get; set; }
        public string ColoaderReference1 { get; set; }
        public string DeliveryOrder { get; set; }
        public string FreightLocationId { get; set; }
        public string TransportDocumentNumber { get; set; }
        public virtual Card FreightLocationWarehouse { get; set; }
        public bool NoFreightFile { get; set; }
        public bool IsMultipleCommodities { get; set; }
        public string SpecialServicesTypeId { get; set; }
        public string ProductCode { get; set; }
        public SpecialServicesType SpecialServicesType { get; set; }
        public bool ARInvoiceIssued { get; set; }
        public bool CreditNoteIssued { get; set; }
        public string NominatedHandlingPartyId { get; set; }
        public virtual Card NominatedHandlingParty { get; set; }
        public string OtherParticipantIdCode1 { get; set; }
        public string OtherParticipantIdCode2 { get; set; }
        public string OtherParticipantIdCode3 { get; set; }
        public string OtherParticipantInformationCode1 { get; set; }
        public string OtherParticipantInformationCode2 { get; set; }
        public string OtherParticipantInformationCode3 { get; set; }
        public string OtherParticipantInformationName1 { get; set; }
        public string OtherParticipantInformationName2 { get; set; }
        public string OtherParticipantInformationName3 { get; set; }
        public string OtherParticipantInformationPortCode1 { get; set; }
        public string OtherParticipantInformationPortCode2 { get; set; }
        public string OtherParticipantInformationPortCode3 { get; set; }
        public string OtherParticipantInformationReference1 { get; set; }
        public string OtherParticipantInformationReference2 { get; set; }
        public string OtherParticipantInformationReference3 { get; set; }
        public string AccountingInformation1 { get; set; }
        public string AccountingInformation2 { get; set; }
        public string AccountingInformation3 { get; set; }
        public string AccountingInformation4 { get; set; }
        public string AccountingInformation5 { get; set; }
        public string AccountingInformation6 { get; set; }
        public string AccountingInformationIdentifierCode1 { get; set; }
        public string AccountingInformationIdentifierCode2 { get; set; }
        public string AccountingInformationIdentifierCode3 { get; set; }
        public string AccountingInformationIdentifierCode4 { get; set; }
        public string AccountingInformationIdentifierCode5 { get; set; }
        public string AccountingInformationIdentifierCode6 { get; set; }

        public string CustomsDeclarationNumber { get; set; }

        public virtual OtherParticipantId OtherParticipantId1 { get; set; }
        public virtual OtherParticipantId OtherParticipantId2 { get; set; }
        public virtual OtherParticipantId OtherParticipantId3 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode1")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier1 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode2")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier2 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode3")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier3 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode4")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier4 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode5")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier5 { get; set; }

        //[ForeignKey("AccountingInformationIdentifierCode6")]
        public virtual AccountingInformationIdentifier AccountingInformationIdentifier6 { get; set; }

        public string ReferenceNumber { get; set; }
        public string SupplementaryShipmentInformation1 { get; set; }
        public string SupplementaryShipmentInformation2 { get; set; }

        public string CargonautFHLStatusCode { get; set; }
        public DateTime? CargonautFHLStatusDate { get; set; }

        //[ForeignKey("CargonautFHLStatusCode")]
        public FHLStatus CargonautFHLStatus { get; set; }

        public int NumberOfInsidePackages { get; set; }
        public string NumberOfInsidePackagesDetails { get; set; }

        public bool ViaColoader { get; set; }

        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public string CustomerShipmentNumber { get; set; }

        public int? NumberOfFollowUps { get; set; }
        public string ForwarderPartnerId { get; set; }
        public string ForwardingPartnerId { get; set; }

        // [ForeignKey("ForwarderPartnerId")]
        public HybridPartner HybridPartner { get; set; }
        public ShipmentComputedFields ShipmentComputedFields { get; set; }
        public ShipmentAdditionalCloudData ShipmentAdditionalCloudData { get; set; }

        public DateTime? OperationalCloseDate { get; set; }
        public DateTime? AccountingCloseDate { get; set; }

        public string LastSentByUserId { get; set; }
        public virtual User LastSentByUser { get; set; }
        public int? CustomerTenantNumber { get; set; }

        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrencyId { get; set; }

        public virtual Currency ValueOfGoodsCurrency { get; set; }

        public bool IsNewARInvoiceBlocked { get; set; }
        public DateTime? OperationalDate { get; set; }
        public string OriginShipmentId { get; set; }

        public bool FBLIsFromStock { get; set; }
        public string ComputedForwarderShipmentNumber { get; set; }

        [ForeignKey("LocalCustomsTransmissionsStatusCode")]
        public CustomsTransmissionsStatus CustomsTransmissionsStatus { get; set; }

        public DateTime? FreightRelease { get; set; }
        public DateTime? TerminalAvailable { get; set; }
        public string ISFNumber { get; set; }
        public DateTime? ISFDate { get; set; }
        public string ITNumber { get; set; }
        public DateTime? ITDate { get; set; }

        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

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

        public virtual Address WarehouseLegAddress { get; set; }
        public virtual Card WarehouseLegCard { get; set; }
        public string WarehouseLegReference { get; set; }
        public DateTime? WarehouseLegCutOffDate { get; set; }
        public DateTime? WarehouseLegVGMCutOffDate { get; set; }
        #endregion

        public DateTime? RegistryDate { get; set; }
        public bool IsAssembly { get; set; }
        public string LastSharedEventId { get; set; }
        public string LastSharedEventLocation { get; set; }
        public string LastSharedEventNotes { get; set; }
        public DateTime? LastSharedEventDate { get; set; }
        public virtual EventType LastSharedEvent { get; set; }
        public double? GrossWeightPerTon { get; set; }
        public DateTime? FirstOperationalCloseDate { get; set; }
        public DateTime? FirstAccountingCloseDate { get; set; }
        public DateTime? AMSClosingDate { get; set; }
        public string UpdatedByPartner { get; set; }

        #region INTTRA
        public DateTime? INTTRASIStatusDate { get; set; }
        public string INTTRASIError { get; set; }

        [ForeignKey("INTTRASIStatusCode")]
        public INTTRASIStatus INTTRASIStatus { get; set; }
        public string INTTRASIStatusCode { get; set; }

        [ForeignKey("EmergencyContactId")]
        public Contact EmergencyContact { get; set; }
        public string EmergencyContactId { get; set; }

        public string INTTRAContractNumber { get; set; }
        public string INTTRAInstructions { get; set; }
        public string INTTRAComments { get; set; }

        public int? INTTRADocumentQTY { get; set; }
        public bool SIHasAttachList { get; set; }
        public bool INTTRAIsFreighted { get; set; }

        [ForeignKey("INTTRADocumentTypeCode")]
        public INTTRADocumentType INTTRADocumentType { get; set; }
        public string INTTRADocumentTypeCode { get; set; }
        #endregion

        public string OnCarriageAdditionalTransportModeCode { get; set; }
        public virtual PickUpDeliveryTransportMode OnCarriageAdditionalTransportMode { get; set; }

        public DateTime? FirstPickupETD { get; set; }
        public DateTime? FirstPickupETA { get; set; }
        public bool SplitOnCarriage { get; set; }
        public DateTime? INTTRALastStatusDate { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? ContainerLastStatusDate { get; set; }

        public string BasicFreightId { get; set; }
        [ForeignKey("BasicFreightId")]
        public PrepaidCollect BasicFreight { get; set; }

        public string DestinationPortChargesId { get; set; }
        [ForeignKey("DestinationPortChargesId")]
        public PrepaidCollect DestinationPortCharges { get; set; }

        public string DestinationHaulageChargesId { get; set; }
        [ForeignKey("DestinationHaulageChargesId")]
        public PrepaidCollect DestinationHaulageCharges { get; set; }

        public string AdditionalChargesId { get; set; }
        [ForeignKey("AdditionalChargesId")]
        public PrepaidCollect AdditionalCharges { get; set; }

        public string FreightPayerId { get; set; }
        [ForeignKey("FreightPayerId")]
        public virtual Card FreightPayer { get; set; }

        public string FreightPayerAddressId { get; set; }
        [ForeignKey("FreightPayerAddressId")]
        public virtual Address FreightPayerAddress { get; set; }

        public bool HasContainerException { get; set; }
        public string ARInvoices { get; set; }
    }
}
