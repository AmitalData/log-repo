using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;
using System.Runtime.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(ShipmentValidator), "IsShipmentValid")]
    public class ShipmentPM//: EntityPMBase
    {
        [Key]
        public string Id { get; set; }

        public bool IsHybrid { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BaseShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConcurrencyGUID { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CASSCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeliveryOrder { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImportManifest { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightLocationId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportDocumentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierTransportDocumentNumber { get; set; }

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

        #region Fields

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryForStatisticsId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierWebSite { get; set; }

        public bool IsFSRSent { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastFSRStatusRequestDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FHLStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FWBStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierLastStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CarrierLastStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CarrierLastStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FNAReason { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId5 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId6 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId7 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId8 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSpecialHandlingCodeId9 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AWBChargeRate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AWBChargeAmount { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBCommodityItemNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PPCC { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FlightDate { get; set; }

        public bool IsFlightDateActual { get; set; }
        public int ConnectedShipments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ChargeableWeightInKG { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? GrossWeightInKG { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? GrossWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CurrentUserId { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BasketId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DirectionId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DirectionName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string House { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime CreateDateTime { get; set; }

        public bool MAWBTakenFromStack { get; set; }
        public bool MAWBReturnedToStack { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchAddress { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IncotermId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IncotermCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IncotermName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Routing { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DepartmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DescriptionOfGoods { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [Timestamp]
        public byte[] LastModified { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? HAWBDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MAWBStackNumber { get; set; }

        public bool IsOperationalClosed { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field5 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field6 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field7 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field8 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field9 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field10 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field11 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field12 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field13 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field14 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field15 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field16 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field17 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field18 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field19 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field20 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field21 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field22 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field23 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field24 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field25 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field26 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field27 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field28 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field29 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field30 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field31 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field32 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field33 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field34 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field35 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field36 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field37 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field38 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field39 { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field40 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }
        public bool IsSecured { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Master { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeViewField { get; set; }

        public string LongMaster { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherPrepaidCollectId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GrossWeightUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargeableWeightUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DimensionsUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RateClassCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? VolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? VolumeInCBM { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Volume { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? PackagesQuantity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? NumberOfPackages { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? NumberOfContainers { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? Ratio { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? DimFactor { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? MAWBOBLDate { get; set; }
        public bool GrossWeightEdited { get; set; }
        public bool ChargeableWeightEdited { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VolumeUnitCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string StatusId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string StatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? StatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string StatusLocation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int StatusWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteId { get; set; }
        public string QuoteNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BookingId { get; set; }
        public string BookingNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TotalContainers { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentType { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FollowUpType { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FollowUpId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FollowUpDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPMId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime LastUpdate { get; set; }

        public bool NewMessage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FollowUpNotes { get; set; }
        public bool IsAnyConversation { get; set; }
        public int NumberOfShipments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFinalDestinationPortId { get; set; }
        public string OriginFinalDestinationPortId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFinalDestinationPortCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFinalDestinationPortName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFinalDestinationPortCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFinalDestinationPortCountryName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainHarmonize { get; set; }
        public bool IsDangerous { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousClassNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousUnNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousPackagingGroup { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousIMDGCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousFlashPoint { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DangerousMaterialDescription { get; set; }

        public bool LTCWEdited { get; set; }
        public int ShipmentPickUpIndex { get; set; }
        public int ShipmentDeliveryIndex { get; set; }
        public int ShipmentContainerReturnIndex { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPayableStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentReceivableStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentReceivableStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentPayableStatusName { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledDate { get; set; }
        public bool IsAccountingClosed { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime AccessDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime LastUpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProfitCurrencyId { get; set; }

        public string ProfitCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? ProfitExchangeRate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NextLegCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NextLegName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? NextETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? NextETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentLevelCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentLevelName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterShipmentDataId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? NumberOfFollowUps { get; set; }

        #endregion

        #region Partners

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentCustomerTypeCode { get; set; }
        public bool ConsigneeAddressOneTime { get; set; }
        public bool ShipperAddressOneTime { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierAgentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierAgentName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierAgentNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierIATACode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightForwarderNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperAddressText { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperAddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeVatNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeAddressText { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeAddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentComputed { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentAddressText { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentAddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentExportNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomAgentImportNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1AddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1ContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1Note { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2AddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2ContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2Name { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2Note { get; set; }
        public string Notify2Address1 { get; set; }
        public string Notify2Address2 { get; set; }
        public string Notify2ZipCode { get; set; }
        public string Notify2StateId { get; set; }
        public string Notify2AddressCountryCode { get; set; }
        public string Notify2City { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomClearancePointNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsolidatorNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForwarderShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedForwarderShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerShipmentNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomsDeclarationNumber { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReleasingAgentNote { get; set; }
        #endregion

        public bool NoFreightFile { get; set; }

        public bool IsExceptionResolved { get; set; }
        public bool IsRefreshFollowUp { get; set; }

        #region Routings
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AirlinePrefix { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageCarrierPrefix { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Transshipment1CarrierPrefix { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Transshipment2CarrierPrefix { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Transshipment3CarrierPrefix { get; set; }

        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string PreCarriageTransportModeId { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string PreCarriageToPortId { get; set; }
        public string PreCarriageCarrierId { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public string PreCarriageCarrierName { get; set; }
        public string PreCarriageCarrierCode { get; set; }
        public string PreCarriageFromPortCode { get; set; }
        public string PreCarriageFromPortName { get; set; }
        public string PreCarriageFromPortCountryCode { get; set; }
        public string PreCarriageFromPortCountryName { get; set; }
        public string PreCarriageToPortCode { get; set; }
        public string PreCarriageToPortName { get; set; }
        public string PreCarriageToPortCountryCode { get; set; }
        public string PreCarriageToPortCountryName { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public string PreCarriageCarrierWebSite { get; set; }
        public string PreCarriageVesselId { get; set; }
        public string PreCarriageVesselName { get; set; }
        public bool HasPreCarriage { get; set; }
        public DateTime? PreCarriageATD_Original { get; set; }
        public DateTime? PreCarriageETD_Original { get; set; }
        public DateTime? PreCarriageATA_Original { get; set; }
        public DateTime? PreCarriageETA_Original { get; set; }
        public string MasterPreCarriageCarrierNumber { get; set; }
        public string MasterPreCarriageVesselName { get; set; }
        public string MasterPreCarriageFromPortName { get; set; }

        public string OnCarriageTransportModeId { get; set; }
        public string OnCarriageFromPortId { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnCarriageCarrierId { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public string OnCarriageCarrierName { get; set; }
        public string OnCarriageCarrierCode { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortName { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageFromPortCountryName { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortCountryCode { get; set; }
        public string OnCarriageToPortCountryName { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        public string OnCarriageCarrierWebSite { get; set; }
        public string OnCarriageVesselId { get; set; }
        public string OnCarriageVesselName { get; set; }
        public string OnCarriageAdditionalTransportModeCode { get; set; }
        public bool HasOnCarriage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SplitOnCarriage { get; set; }
        public DateTime? OnCarriageATD_Original { get; set; }
        public DateTime? OnCarriageETD_Original { get; set; }
        public DateTime? OnCarriageATA_Original { get; set; }
        public DateTime? OnCarriageETA_Original { get; set; }

        public string MainCarriageTransportModeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string OriginMainCarriageFromPortId { get; set; }

        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageFromPortCountryName { get; set; }
        public string MainCarriageFromPortCountryCode { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageToPortName { get; set; }
        public string MainCarriageToPortCountryCode { get; set; }
        public string MainCarriageToPortCountryName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MainCarriageVesselId { get; set; }
        
        public string Transshipment1VesselId { get; set; }
        public string Transshipment2VesselId { get; set; }
        public string Transshipment3VesselId { get; set; }

        public string MainCarriageVesselName { get; set; }

        public string Transshipment1VesselName { get; set; }
        public string Transshipment2VesselName { get; set; }
        public string Transshipment3VesselName { get; set; }

        public bool MainCarriageIsFromStack { get; set; }

        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }

        public string Transshipment1FromPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment1CarrierId { get; set; }
        public string Transshipment1CarrierName { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment1FromPortName { get; set; }
        public string Transshipment1FromPortCountryCode { get; set; }
        public string Transshipment1FromPortCountryName { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1ToPortName { get; set; }
        public string Transshipment1ToPortCountryCode { get; set; }
        public string Transshipment1ToPortCountryName { get; set; }
        public string Transshipment1CarrierWebSite { get; set; }
        public string Transshipment2CarrierWebSite { get; set; }
        public string Transshipment3CarrierWebSite { get; set; }

        public string Transshipment2FromPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment2CarrierId { get; set; }
        public string Transshipment2CarrierName { get; set; }
        public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2FromPortCode { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment2FromPortCountryCode { get; set; }
        public string Transshipment2FromPortCountryName { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2ToPortName { get; set; }
        public string Transshipment2ToPortCountryCode { get; set; }
        public string Transshipment2ToPortCountryName { get; set; }

        public string Transshipment3FromPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }
        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Transshipment3CarrierNumber { get; set; }
        public string Transshipment3CarrierId { get; set; }
        public string Transshipment3CarrierName { get; set; }
        public string Transshipment3CarrierCode { get; set; }
        public string Transshipment3FromPortCode { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Transshipment3FromPortCountryCode { get; set; }
        public string Transshipment3FromPortCountryName { get; set; }
        public string Transshipment3ToPortCode { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCountryCode { get; set; }
        public string Transshipment3ToPortCountryName { get; set; }

        public string FinalDistenationPortId { get; set; }

        public string Transshipment1AdditionalMAWBOBLBL { get; set; }
        public string Transshipment2AdditionalMAWBOBLBL { get; set; }
        public string Transshipment3AdditionalMAWBOBLBL { get; set; }

        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }
        public string FromPortCountryName { get; set; }

        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }
        public string ToPortCountryName { get; set; }
        #endregion

        #region Booking
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? OrderGrossWeight { get; set; }
        public double? BookingVolume { get; set; }

        public bool OrderGrossWeightEdited { get; set; }
        public bool OrderChargeableWeightEdited { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? BookingNumberOfPackages { get; set; }
        public bool OrderIsDangerouseGoods { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BookingConfirmationNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BookingConfirmedBy { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BookingConfirmationNotes { get; set; }
        public double? OrderVolumetricWeight { get; set; }
        public double? OrderChargeableWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CutoffDate { get; set; }
        #endregion

        #region AWB

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AWBPrint { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FWBStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FWBStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FHLStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FHLStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBCurrencyId { get; set; }
        public string AWBCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AWBFreightAmountPrepaid { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? AWBFreightAmountCollect { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBCarrierTarrifReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBDeclaredValueForCarriage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBDeclaredValueForCustoms { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBAccountingInformation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBInsurrenceValue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBHandlingInformation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SCI { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBComments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBPrintingComments { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBSignature { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBPlace { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBChargesCodeCode { get; set; }
        #endregion

        #region FWB CCS Dummy fields
        public string TenantZeroAirlineId { get; set; }
        public string TenantZeroAirlineTTY { get; set; }
        public string TenantZeroAirlinePIMA { get; set; }
        public bool TenantZeroAirlineChampFWB { get; set; }
        public bool TenantZeroAirlineChampFHL { get; set; }
        public bool TenantZeroAirlineChampFSU { get; set; }
        public bool TenantZeroAirlineChampFSRFSA { get; set; }
        public bool TenantZeroAirlineChampFVRFVA { get; set; }
        public bool CarrierIsChampRegistered { get; set; }
        public bool TenantZeroAirlineChampNeedsRegistration { get; set; }
        public bool TenantZeroAirlineGLSHKFWB { get; set; }
        public bool TenantZeroAirlineGLSHKFHL { get; set; }
        public bool TenantZeroAirlineGLSHKFSU { get; set; }
        public bool TenantZeroAirlineGLSHKFSRFSA { get; set; }
        public bool TenantZeroAirlineGLSHKFVRFVA { get; set; }
        public bool CarrierIsGLSHKRegistered { get; set; }
        public bool TenantZeroAirlineGLSHKNeedsRegistration { get; set; }
        public bool CarrierIsCheckDigit { get; set; }
        public bool CarrierIsLimitedLength { get; set; }
        #endregion

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalCustomsTransmissionsStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalCustomsTransmissionsStatusName { get; set; }

        public string LocalCustomsTransmissionsStatusError { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LocalCustomsTransmissionsStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalCustomsSentByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OperationalClosedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalCustomsSentByUserName { get; set; }

        public bool IncludesCustoms { get; set; }
        public bool IsUpdateByAutomation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeclarationNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DeclarationDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CustomsClearanceDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MasterShipmentNumber { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProductCode { get; set; }

        public string SecurityKey { get; set; }
        public double? TEU { get; set; }

        public bool DontCreateConvertEvent { get; set; }
        public bool ConvertFromHouseToDirect { get; set; }
        public bool ConvertFromDirectToHouse { get; set; }
        public bool IsRefreshShipmentFollowUps { get; set; }
        public string CustomerRankName { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public DateTime? EstimatedFinalArrivalDate { get; set; }
        public DateTime? ActualFinalArrivalDate { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public DateTime? LastStatusLogDate { get; set; }
        public string MainCarriageFullCarrierNumber { get; set; }
        public string Transshipment1FullCarrierNumber { get; set; }
        public string Transshipment2FullCarrierNumber { get; set; }
        public string Transshipment3FullCarrierNumber { get; set; }

        public string ExceptionDescription { get; set; }
        public string ExceptionResolvedDescription { get; set; }
        public string LastExceptionDescription { get; set; }

        public DateTime? ExceptionDate { get; set; }
        public bool HasException { get; set; }
        public string HasExceptionMessage { get; set; }

        public bool IsManifestSentToAgent { get; set; }
        public string AgentSharedManifestRef { get; set; }


        public string FromLocation { get; set; }
        public string ToLocation { get; set; }

        public string MoveTypeId { get; set; }
        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }

        public bool IsCreatedFromAgentSharedManifest { get; set; }

        public string AMSBL { get; set; }
        public string CustomFileId { get; set; }
        public string CustomFileNumber { get; set; }
        public DateTime? MainCarriageSTD { get; set; }
        public DateTime? MainCarriageSTA { get; set; }
        public DateTime? Transshipment1STD { get; set; }
        public DateTime? Transshipment1STA { get; set; }
        public DateTime? Transshipment2STD { get; set; }
        public DateTime? Transshipment2STA { get; set; }
        public DateTime? Transshipment3STD { get; set; }
        public DateTime? Transshipment3STA { get; set; }

        public bool IsMultipleCommodities { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NominatedHandlingPartyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantIdCode1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantIdCode2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantIdCode3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationCode1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationCode2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationCode3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationPortCode1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationPortCode2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationPortCode3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationName1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationName2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationName3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationReference2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OtherParticipantInformationReference3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation5 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformation6 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode3 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode4 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode5 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountingInformationIdentifierCode6 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReferenceNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SupplementaryShipmentInformation1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SupplementaryShipmentInformation2 { get; set; }

        public string LastSentByUserId { get; set; }

        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrencyId { get; set; }
        public DateTime? OperationalDate { get; set; }

        public bool FBLIsFromStock { get; set; }
        public bool FBLReturnedToStock { get; set; }
        public bool FBLTakenFromStock { get; set; }
        public string FBLStockNumber { get; set; }
        public bool FBLReturnedToStockWithCancel { get; set; }


        public string Transshipment3ToPortStateCode { get; set; }
        public string Transshipment2ToPortStateCode { get; set; }
        public string Transshipment1ToPortStateCode { get; set; }

        

        private List<ShipmentFollowUpPM> followUps;
        [Include]
        [Composition]
        [Association("FollowUpShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentFollowUpPM> FollowUps
        {
            get
            {
                if (followUps == null)
                {
                    followUps = new List<ShipmentFollowUpPM>();
                }

                return this.followUps;
            }
            set
            {
                if (value != null)
                {
                    followUps = value;
                }
            }
        }

        private List<ShipmentReceivablePM> shipmentReceivables;
        [Include]
        [Association("ShipmentReceivableShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentReceivablePM> ShipmentReceivables
        {
            get
            {

                if (this.shipmentReceivables == null)
                {
                    shipmentReceivables = new List<ShipmentReceivablePM>();
                }
                return this.shipmentReceivables;
            }
            set
            {
                if (value != null)
                {
                    shipmentReceivables = value;
                }
            }
        }

        private List<ShipmentPayablePM> shipmentPayables;
        [Include]
        [Association("ShipmentPayableShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentPayablePM> ShipmentPayables
        {
            get
            {

                if (this.shipmentPayables == null)
                {
                    shipmentPayables = new List<ShipmentPayablePM>();
                }
                return this.shipmentPayables;
            }
            set
            {
                if (value != null)
                {
                    shipmentPayables = value;
                }
            }
        }

        private List<ShipmentPackagePM> shipmentPackages;
        [Include]
        [DataMember]
        [Composition]
        [Association("ShipmentPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPackagePM> ShipmentPackages
        {
            get
            {
                if (shipmentPackages == null)
                {
                    shipmentPackages = new List<ShipmentPackagePM>();
                }

                return this.shipmentPackages;
            }

            set
            {
                if (value != null)
                {
                    shipmentPackages = value;
                }
            }
        }

        private List<ShipmentPackagePM> connectedMasterPackages;
        [Include]
        [DataMember]
        [Composition]
        [Association("MasterPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPackagePM> ConnectedMasterPackages
        {
            get
            {
                if (connectedMasterPackages == null)
                {
                    connectedMasterPackages = new List<ShipmentPackagePM>();
                }

                return this.connectedMasterPackages;
            }

            set
            {
                if (value != null)
                {
                    connectedMasterPackages = value;
                }
            }
        }

        private List<ShipmentOrderPackagePM> shipmentOrderPackages;
        [Include]
        [Composition]
        [Association("ShipmentOrderPackagePMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentOrderPackagePM> ShipmentOrderPackages
        {
            get
            {
                if (shipmentOrderPackages == null)
                {
                    shipmentOrderPackages = new List<ShipmentOrderPackagePM>();
                }

                return this.shipmentOrderPackages;
            }

            set
            {
                if (value != null)
                {
                    shipmentOrderPackages = value;
                }
            }
        }

        private List<ShipmentPickUpPM> shipmentPickUps;
        [Include]
        [Composition]
        [DataMember]
        [Association("ShipmentPickUpPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentPickUpPM> ShipmentPickUps
        {
            get
            {
                if (shipmentPickUps == null)
                {
                    shipmentPickUps = new List<ShipmentPickUpPM>();
                }

                return this.shipmentPickUps;
            }
            set
            {
                if (value != null)
                {
                    shipmentPickUps = value;
                }
            }
        }

        private List<ShipmentDeliveryPM> shipmentDeliveries;
        [Include]
        [Composition]
        [Association("ShipmentDeliveryPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentDeliveryPM> ShipmentDeliveries
        {
            get
            {
                if (shipmentDeliveries == null)
                {
                    shipmentDeliveries = new List<ShipmentDeliveryPM>();
                }

                return this.shipmentDeliveries;
            }
            set
            {
                if (value != null)
                {
                    shipmentDeliveries = value;
                }
            }
        }

        private List<ShipmentARInvoicePM> shipmentArInvoices;
        [Include]
        [Composition]
        [Association("ShipmentARInvoicePMShipment", "Id", "ShipmentId")]
        public List<ShipmentARInvoicePM> ShipmentARInvoices
        {
            get
            {
                if (shipmentArInvoices == null) { shipmentArInvoices = new List<ShipmentARInvoicePM>(); }
                return shipmentArInvoices;
            }

            set
            {
                if (value != null) { shipmentArInvoices = value; }
            }
        }

        private List<ShipmentAPInvoicePM> shipmentApInvoices;
        [Include]
        [Composition]
        [Association("ShipmentAPInvoicePMShipment", "Id", "ShipmentId")]
        public List<ShipmentAPInvoicePM> ShipmentAPInvoices
        {
            get
            {
                if (shipmentApInvoices == null) { shipmentApInvoices = new List<ShipmentAPInvoicePM>(); }
                return shipmentApInvoices;
            }

            set
            {
                if (value != null) { shipmentApInvoices = value; }
            }
        }

        private List<ConsoleShipmentPM> shipmentConsoleShipments;
        [Include]
        [Composition]
        [Association("ConsoleShipmentPMShipment", "Id", "MasterShipmentDataId")]
        public List<ConsoleShipmentPM> ShipmentConsoleShipments
        {
            get
            {
                if (shipmentConsoleShipments == null) { shipmentConsoleShipments = new List<ConsoleShipmentPM>(); }
                return shipmentConsoleShipments;
            }

            set
            {
                if (value != null) { shipmentConsoleShipments = value; }
            }
        }

        private List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnlies;
        [Include]
        [Association("ShipmentAWBPrintOnlyPMShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentAWBPrintOnlyPM> ShipmentAWBPrintOnlies
        {
            get
            {

                if (this.shipmentAWBPrintOnlies == null)
                {
                    shipmentAWBPrintOnlies = new List<ShipmentAWBPrintOnlyPM>();
                }
                return this.shipmentAWBPrintOnlies;
            }
            set
            {
                if (value != null)
                {
                    shipmentAWBPrintOnlies = value;
                }
            }
        }

        private List<ShipmentCarrierStatusPM> shipmentCarrierStatuses;
        [Include]
        [Composition]
        [Association("ShipmentShipmentCarrierStatuses", "Id", "ShipmentId")]
        public List<ShipmentCarrierStatusPM> ShipmentCarrierStatuses
        {
            get
            {
                if (shipmentCarrierStatuses == null) { shipmentCarrierStatuses = new List<ShipmentCarrierStatusPM>(); }
                return shipmentCarrierStatuses;
            }

            set
            {
                if (value != null) { shipmentCarrierStatuses = value; }
            }
        }

        private List<AWBOCIPM> aWBOCIPMs;
        [Include]
        [Composition]
        [Association("ShipmentAWBOCI", "Id", "ShipmentId")]
        public List<AWBOCIPM> AWBOCIPMs
        {
            get
            {
                if (aWBOCIPMs == null) { aWBOCIPMs = new List<AWBOCIPM>(); }
                return aWBOCIPMs;
            }

            set
            {
                if (value != null) { aWBOCIPMs = value; }
            }
        }


        private List<ShipmentCommodityPM> shipmentCommodities;
        [Include]
        [Composition]
        [Association("ShipmentCommodityPMShipment", "Id", "ShipmentId")]
        public virtual List<ShipmentCommodityPM> ShipmentCommodities
        {
            get
            {
                if (shipmentCommodities == null)
                {
                    shipmentCommodities = new List<ShipmentCommodityPM>();
                }

                return this.shipmentCommodities;
            }

            set
            {
                if (value != null)
                {
                    shipmentCommodities = value;
                }
            }
        }

        private List<ShipmentAssemblyPM> shipmentAssemblies;
        [Include]
        [Association("ShipmentAssemblyShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentAssemblyPM> ShipmentAssemblies
        {
            get
            {

                if (this.shipmentAssemblies == null)
                {
                    shipmentAssemblies = new List<ShipmentAssemblyPM>();
                }
                return this.shipmentAssemblies;
            }
            set
            {
                if (value != null)
                {
                    shipmentAssemblies = value;
                }
            }
        }


        private List<DocumentsFilingPM> missingDocuments;
        public virtual List<DocumentsFilingPM> MissingDocuments
        {
            get
            {
                if (missingDocuments == null)
                {
                    missingDocuments = new List<DocumentsFilingPM>();
                }

                return this.missingDocuments;
            }

            set
            {
                if (value != null)
                {
                    missingDocuments = value;
                }
            }
        }

        private List<DocumentsFilingPM> receivedDocuments;
        public virtual List<DocumentsFilingPM> ReceivedDocuments
        {
            get
            {
                if (receivedDocuments == null)
                {
                    receivedDocuments = new List<DocumentsFilingPM>();
                }

                return this.receivedDocuments;
            }

            set
            {
                if (value != null)
                {
                    receivedDocuments = value;
                }
            }
        }

        private List<DocumentsFilingPM> requiredDocuments;
        public virtual List<DocumentsFilingPM> RequiredDocuments
        {
            get
            {
                if (requiredDocuments == null)
                {
                    requiredDocuments = new List<DocumentsFilingPM>();
                }

                return this.requiredDocuments;
            }

            set
            {
                if (value != null)
                {
                    requiredDocuments = value;
                }
            }
        }

        public string MainCarriageFromPartnerId { get; set; }
        public string MainCarriageFromAddressId { get; set; }
        public string MainCarriageToPartnerId { get; set; }
        public string MainCarriageToAddressId { get; set; }
        public string Driver { get; set; }
        public string TruckNumber { get; set; }
        public string TrailerNumber { get; set; }
        public bool AsAgreedFreight { get; set; }
        public bool AsAgreedOtherCharges { get; set; }
        public bool ARInvoiceIssued { get; set; }
        public bool CreditNoteIssued { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CargonautFHLStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CargonautFHLStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CargonautFHLStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CargonautFWBStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CargonautFWBStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CargonautFWBStatusDate { get; set; }

        // Dummy
        public bool MarkFollowUpsAsDone { get; set; }
        public bool CalculateProfit { get; set; }
        public bool CalculateStatus { get; set; }
        public string ComputedStatusId { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public string ComputedStatusName { get; set; }
        public string CustomFilePocoId { get; set; }
        public DateTime? ManifestLastSharingDate { get; set; }
        public bool CalculatePayables { get; set; }
        public bool CalculateReceivables { get; set; }
        public bool IsAddingStackEvents { get; set; }
        public bool IsRemovingStackEvents { get; set; }
        public string StackAirlineId { get; set; }
        public string FromPartnerCity { get; set; }
        public string FromPartnerCountryCode { get; set; }
        public string FromPartnerCountryName { get; set; }
        public string ToPartnerCity { get; set; }
        public string ToPartnerCountryCode { get; set; }
        public string ToPartnerCountryName { get; set; }
        public bool IsSendFSRCreatingShipment { get; set; }
        public string ToCountryId { get; set; }
        public string FromCountryId { get; set; }
        public string CopyFromShipmentId { get; set; }
        public bool IsCopyFromShipment { get; set; }
        public bool IsBuildFromQuote { get; set; }
        public bool IsBuildFromBooking { get; set; }
        public string ShipperMainAddressId { get; set; }
        public string ShipperPickAddressId { get; set; }
        public string ConsigneeMainAddressId { get; set; }
        public string ConsigneePickAddressId { get; set; }
        
        public bool IncludePickUp { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromAddressCountryId { get; set; }
        public string PickUpAddressId { get; set; }
        public bool CustomConnectToShipment { get; set; }
        public bool IncludeDelivery { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }
        public string DeliveryAddressId { get; set; }
        public string SpecialServicesTypeId { get; set; }
        public string SpecialServicesTypeName { get; set; }
        #region Dummy fields needed for the Build from Quote screen
        public int? Quantity1 { get; set; }
        public int? Quantity2 { get; set; }
        public int? Quantity3 { get; set; }
        public int? Quantity4 { get; set; }
        public int? Quantity5 { get; set; }
        public string PackageTypeId1 { get; set; }
        public string PackageTypeId2 { get; set; }
        public string PackageTypeId3 { get; set; }
        public string PackageTypeId4 { get; set; }
        public string PackageTypeId5 { get; set; }
        #endregion
        public string NewConcurrencyGUID { get; set; }

        public int ConnectedShipmentsPayablesCount { get; set; }
        public int ConnectedShipmentsReceivablesCount { get; set; }
        public int NumberOfInsidePackages { get; set; }
        public string NumberOfInsidePackagesDetails { get; set; }

        public string AccountManagerUserId { get; set; }
        public string AccountManagerUserName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ManifestReason { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ManifestStatusCode { get; set; }

        public bool IsKnownCargo { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegulatedAgentRANumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string KnownConsignorNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? KCExpirationDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ColoaderRANumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBPrintingSecurityStatusId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AWBPrintingRANumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AdditionalHandlingInfo { get; set; }

        public bool AWBPrintingSecurityStatusEdited { get; set; }
        public bool AWBPrintingRANumberEdited { get; set; }
        public bool AdditionalHandlingInfoEdited { get; set; }

        public bool ViaColoader { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IssuingCarrierReference1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsMissingDocument { get; set; }
        public string DocumentsSearchFields { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InterlineId { get; set; }

        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperStateId { get; set; }
        public string ShipperCountryId { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPhoneNumber { get; set; }
        public string ShipperFaxNumber { get; set; }

        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeStateId { get; set; }
        public string ConsigneeCountryId { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneePhoneNumber { get; set; }
        public string ConsigneeFaxNumber { get; set; }

        public string Notify1Address1 { get; set; }
        public string Notify1Address2 { get; set; }
        public string Notify1ZipCode { get; set; }
        public string Notify1StateId { get; set; }
        public string Notify1CountryId { get; set; }
        public string Notify1City { get; set; }
        public string Notify1PhoneNumber { get; set; }
        public string Notify1FaxNumber { get; set; }

        public string IssuingCarrierCity { get; set; }
        public string Notify1AddressCountryCode { get; set; }

        public bool MAWBReturnedToStackWithCancel { get; set; }

        public string MAWBStackAirlineId { get; set; }

        public bool DontAddToImportersQueue { get; set; }

        public bool DontAddToForwarderQueue { get; set; }

        public string ForwarderPartnerId { get; set; }

        public DateTime? OperationalCloseDate { get; set; }
        public DateTime? AccountingCloseDate { get; set; }
        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }

        //islam: for testing the importers data mapping
        public bool ConvertToCustomFile { get; set; }

        public int? CustomerTenantNumber { get; set; }

        public DateTime? MainCarriageFinalDestinationETA { get; set; }
        public DateTime? MainCarriageFinalDestinationATA { get; set; }
        public DateTime? DepartureArrivalFromDate { get; set; }
        public DateTime? DepartureArrivalToDate { get; set; }

        public string FirstPickupLocation { get; set; }
        public string FinalDeliveryLocation { get; set; }
        public bool IsImporterShipment { get; set; }
        public bool IsUpdatedByChampAnalyzer { get; set; }
        public bool IsUpdatedByGLSHKAnalyzer { get; set; }
        public bool IsUpdatedByINTTRAAnalyzer { get; set; }

        public bool IsCreatedFromCustomerOverview { get; set; }

        public string DeclarationXMLData { get; set; }
        public bool IsImporterApprovalRequired { get; set; }
        public bool SendUpdatesToAgentEnabled { get; set; }
        public bool UpdateSendUpdatesToAgentEnabledField { get; set; }
        public bool DocsSentToAgent { get; set; }
        public string VersionApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string DocumentsApprovedByUserName { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public bool IsNewARInvoiceBlocked { get; set; }
        public string ShipmentAddtionalDataXML { get; set; }
        //public bool IsMappingXSDFields { get; set; }
        //public bool StopConcurrencyValidating { get; set; }
        public string OriginShipmentId { get; set; }
        public bool ProrateReceivables { get; set; }
        public bool ReleasesPackagesAddedOnTheShipment { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FreightRelease { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? TerminalAvailable { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ISFNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ISFDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ITNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ITDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DocumentsClosingDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OBLTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ENSNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ENSDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TruckerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AssignedToTruckerDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AssginedtoCustomsAgentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AssginedToCustomsAgentDate { get; set; }


        #region WarehouseLeg
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegWarehouseId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegTerminalCode { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegExpectedEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegActualEntryDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegExpectedReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegActualReleaseDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegLastFreeDate { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegRemarks { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressAddress1 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressAddress2 { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCity { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCountryName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressCountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressPhoneNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegAddressFaxNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WarehouseLegReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string WarehouseLegTerminalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegEntryDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegReleaseDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegVGMCutOffDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? WarehouseLegCutOffDate { get; set; }
        #endregion 

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? RegistryDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAssembly { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastSharedEventId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastSharedEventLocation { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastSharedEventNotes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastSharedEventDate { get; set; }

        public string LastSharedEventName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? GrossWeightPerTon { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public double? GrossWeightPerStorageDays { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstOperationalCloseDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstAccountingCloseDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OldStatusValue { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AMSClosingDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByPartner { get; set; }

        public string SplitFromShipmentNo { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRASIStatusCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRASIStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? INTTRASIStatusDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRASIError { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmergencyContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRAContractNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRAInstructions { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRAComments { get; set; }

        public int? INTTRADocumentQTY { get; set; }
        public bool SIHasAttachList { get; set; }
        public bool INTTRAIsFreighted { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRADocumentTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRABookingTransStatusCode { get; set; }
        public string INTTRABookingTransStatusName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRABookingStatusCode { get; set; }
        public string INTTRABookingStatusName { get; set; }
        
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? INTTRALastEBbookingSendDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRABookingError { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string INTTRALastBookingResponse { get; set; }

        public string INTTRABookingResponse_Voyage { get; set; }
        public DateTime INTTRABookingResponse_POLDate { get; set; }
        public string INTTRABookingResponse_POFPort { get; set; }
        public string INTTRABookingResponse_POFPortCode { get; set; }
        public string INTTRABookingResponse_POFCCode { get; set; }
        public string INTTRABookingResponse_POFCName { get; set; }
        public DateTime INTTRABookingResponse_PODDate { get; set; }
        public string INTTRABookingResponse_PODPort { get; set; }
        public string INTTRABookingResponse_PODPortCode { get; set; }
        public string INTTRABookingResponse_PODCCode { get; set; }
        public string INTTRABookingResponse_PODCName { get; set; }
        public string INTTRABookingResponse_ShippingLine { get; set; }
        public string INTTRABookingResponse_Vessel { get; set; }
        public string INTTRABookingResponse_VesselId { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastFinalDestination { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string From { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string To { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Origin { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstPickupETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstPickupETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? INTTRALastStatusDate { get; set; }

        public bool IsPaymentRequired { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentRequestXML { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? PaymentDateTime { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify1Reference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notify2Reference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperNotExporterReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeNotImporterReference { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForwardingPartnerId { get; set; }

        public bool IsSharedLogisticsMoneyTabEnabled { get; set; }
        public bool IsSharedLogisticsMainCarrierVisible { get; set; }
        public bool IsSharedLogisticsPickDelvCarrierVisible { get; set; }
        public bool IsSharedLogisticsAgentVisible { get; set; }
        public bool IsSharedLogisticsShipperVisible { get; set; }
        public bool IsSharedLogisticsConsigneeVisible { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstPickupATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstPickupATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryETA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryETD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryATA { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FinalDeliveryATD { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DeclarationWCOXml { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProjectNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ContainerLastStatusDate { get; set; }

        public bool ShipmentContanisDangerousGoods { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BasicFreightId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DestinationPortChargesId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DestinationHaulageChargesId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AdditionalChargesId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightPayerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreightPayerAddressId { get; set; }

        public bool HasContainerException { get; set; }

        public DateTime? FirstARInvoiceApprovalDate { get; set; }


        //ShipmentComputedFields
        public bool IsMissingDocuments { get; set; }
        public DateTime? LastDocumentDateTime { get; set; }
        public int MissingDocumentsCount { get; set; }
        public string MissingDocumentsNames { get; set; }
        public bool IsRequestedDocuments { get; set; }
        public int RequestedDocumentsCount { get; set; }
        public int NumberOfHouses { get; set; }
        public bool IsDigitalSignRequired { get; set; }
        public bool IsDepositionRequired { get; set; }
        public string ImporterDepositionRequestDetails { get; set; }
        public bool IsShipmentComputedFieldChange { get; set; }
        public bool IsShipmentAdditionalCloudDataChange { get; set; }
        public bool IsStatusChange { get; set; }


        public string PackagesTypesNames { get; set; }
        public string PackagesTypesPrintAs { get; set; }
        public string ContainersNumbers { get; set; }
        public string ARInvoices { get; set; }
        public bool ConvertShipmentToLCL { get; set; }
        public bool ConvertShipmentToFCL { get; set; }

        public bool ConvertShipmentToLTL { get; set; }
        public bool ConvertShipmentToFTL { get; set; }
        public string HousesNumbers { get; set; }

        public bool ShipmentDirectionConverted { get; set; }
        public bool ShipmentConvertedNewNumber { get; set; }
        public bool FromCountryIsEC { get; set; }
        public bool ToCountryIsEC { get; set; }
        public bool PackagesDeleted { get; set; }
        public string MasterCreatedFromHouseId { get; set; }
        public bool IsDeletingAllPayables { get; set; }
        public double? NotInvoicedReceivablesAmount { get; set; }
        public string CreatedByPartner { get; set; }

        // Fields of Champ analyzer Concurrency
        public string FWBStatusCode_Original { get; set; }
        public DateTime? FWBStatusDate_Original { get; set; }
        public string FHLStatusCode_Original { get; set; }
        public DateTime? FHLStatusDate_Original { get; set; }
        public string CarrierLastStatusCode_Original { get; set; }
        public DateTime? CarrierLastStatusDate_Original { get; set; }
        public string MainCarriageToPortId_Original { get; set; }
        public int? NumberOfPackages_Original { get; set; }
        public double? GrossWeight_Original { get; set; }
        public double? ChargeableWeight_Original { get; set; }
        public string GrossWeightUnitCode_Original { get; set; }
        public DateTime? MainCarriageATD_Original { get; set; }
        public DateTime? MainCarriageETD_Original { get; set; }
        public DateTime? MainCarriageSTD_Original { get; set; }
        public DateTime? Transshipment1ATD_Original { get; set; }
        public DateTime? Transshipment1ETD_Original { get; set; }
        public DateTime? Transshipment1STD_Original { get; set; }
        public DateTime? Transshipment2ATD_Original { get; set; }
        public DateTime? Transshipment2ETD_Original { get; set; }
        public DateTime? Transshipment2STD_Original { get; set; }
        public DateTime? Transshipment3ATD_Original { get; set; }
        public DateTime? Transshipment3ETD_Original { get; set; }
        public DateTime? Transshipment3STD_Original { get; set; }        
        public DateTime? MainCarriageATA_Original { get; set; }
        public DateTime? MainCarriageETA_Original { get; set; }
        public DateTime? MainCarriageSTA_Original { get; set; }
        public DateTime? Transshipment1ATA_Original { get; set; }
        public DateTime? Transshipment1ETA_Original { get; set; }
        public DateTime? Transshipment1STA_Original { get; set; }
        public DateTime? Transshipment2ATA_Original { get; set; }
        public DateTime? Transshipment2ETA_Original { get; set; }
        public DateTime? Transshipment2STA_Original { get; set; }
        public DateTime? Transshipment3ATA_Original { get; set; }
        public DateTime? Transshipment3ETA_Original { get; set; }
        public DateTime? Transshipment3STA_Original { get; set; }        
        public string INTTRABookingStatusCode_Original { get; set; }
        public string BookingConfirmedBy_Original { get; set; }
        public string MAN_FromPortId_Original { get; set; }
        public string FIN_PortId_Original { get; set; }
        public string TR3_ToPortId_Original { get; set; }
        public string TR2_ToPortId_Original { get; set; }
        public string TR1_ToPortId_Original { get; set; }
        public string BookingConfNumber_Original { get; set; }
        public string MAN_CarrierNumber_Original { get; set; }

        public bool IsUserIDNumberRequired { get; set; }
        public DateTime? UserIdNumberUpdateDate { get; set; }
        public string UserIdNumberXMLData { get; set; }
        public string UserIdNumber { get; set; }
        public string WarehouseReleasesIds { get; set; }
        public int? WarehouseStorageFreeDays { get; set; }
        public bool IsDeclarationApprovalRequest { get; set; }
        public bool CreatedFromDigital { get; set; }

        public string DocumentFilingIds { get; set; }

        

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SLAC { get; set; }
        
        public string MasterProjectNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentSubTypeId { get; set; }
        public string ShipmentSubTypeName { get; set; }
        public bool IsUpdateWarehouseLegData { get; set; }
        public bool IsUpdateEntityException { get; set; }
        public string MasterHousesNumbers { get; set; }
        public string HousesDescriptionofGoods { get; set; }
        public DateTime? BookingConfirmationSentDate { get; set; }
        public DateTime? PreAlertSentDate { get; set; }
        public DateTime? DeliveryNoticeSentDate { get; set; }
        public DateTime? ExpectedArrivalNoticeSentDate { get; set; }
        public DateTime? ArrivalNoticeSentDate { get; set; }
        public DateTime? T1ReceivedDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool ChargeStorage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ChargeStorageCurrencyId { get; set; }
        public string ChargeStorageCurrencyCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WeightMeasurementCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string WeightRoundingCode { get; set; }

        private List<ShipmentStoragePricingPM> shipmentStoragePricings;
        [Include]
        [Association("shipmentStoragePricingShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentStoragePricingPM> ShipmentStoragePricings
        {
            get
            {
                if (this.shipmentStoragePricings == null)
                {
                    shipmentStoragePricings = new List<ShipmentStoragePricingPM>();
                }
                return this.shipmentStoragePricings;
            }
            set
            {
                if (value != null)
                {
                    shipmentStoragePricings = value;
                }
            }
        }

        public List<TransshipmentLeg> MainCarriageLegs { get; set; }
        public bool IsCFSWarehouse { get; set; }
        public bool IsCFSWarehouseChanged { get; set; }
        public string ViewSharedDocuments { get; set; }
        public bool IsAccrualsApproved { get; set; }
        public DateTime? AccrualsApprovalDate { get; set; }
        public bool IsExternalAPI { get; set; }
        public double? HousesOpenPayablesInLocal { get; set; }
        public double? HousesOpenPayablesInProfit { get; set; }
        public double? HousesACCTPayablesInLocal { get; set; }
        public double? HousesACCTPayablesInProfit { get; set; }
        public double? HousesOpenReceivablesInLocal { get; set; }
        public double? HousesOpenReceivablesInProfit { get; set; }
        public double? HousesACCTReceivablesInLocal { get; set; }
        public double? HousesACCTReceivablesInProfit { get; set; }
        public string ExternalStatuses { get; set; }
        public bool IsGroupageHousesUpdated { get; set; }

        public string PreForwardingTransportModeId { get; set; }
        public string PreForwardingFromPortId { get; set; }
        public string PreForwardingToPortId { get; set; }
        public string PreForwardingCarrierId { get; set; }
        public string PreForwardingCarrierNumber { get; set; }
        public string PreForwardingCarrierName { get; set; }
        public string PreForwardingCarrierCode { get; set; }
        public string PreForwardingFromPortCode { get; set; }
        public string PreForwardingFromPortName { get; set; }
        public string PreForwardingFromPortCountryCode { get; set; }
        public string PreForwardingFromPortCountryName { get; set; }
        public string PreForwardingToPortCode { get; set; }
        public string PreForwardingToPortName { get; set; }
        public string PreForwardingToPortCountryCode { get; set; }
        public string PreForwardingToPortCountryName { get; set; }
        public DateTime? PreForwardingETD { get; set; }
        public DateTime? PreForwardingATD { get; set; }
        public DateTime? PreForwardingETA { get; set; }
        public DateTime? PreForwardingATA { get; set; }
        public string PreForwardingCarrierWebSite { get; set; }
        public string PreForwardingVesselId { get; set; }
        public string PreForwardingVesselName { get; set; }
        public bool HasPreForwarding { get; set; }
        public DateTime? PreForwardingATD_Original { get; set; }
        public DateTime? PreForwardingETD_Original { get; set; }
        public DateTime? PreForwardingATA_Original { get; set; }
        public DateTime? PreForwardingETA_Original { get; set; }

        public string OnForwardingTransportModeId { get; set; }
        public string OnForwardingFromPortId { get; set; }
        public string OnForwardingToPortId { get; set; }
        public string OnForwardingCarrierId { get; set; }
        public string OnForwardingCarrierNumber { get; set; }
        public string OnForwardingCarrierName { get; set; }
        public string OnForwardingCarrierCode { get; set; }
        public string OnForwardingFromPortCode { get; set; }
        public string OnForwardingFromPortName { get; set; }
        public string OnForwardingFromPortCountryCode { get; set; }
        public string OnForwardingFromPortCountryName { get; set; }
        public string OnForwardingToPortCode { get; set; }
        public string OnForwardingToPortName { get; set; }
        public string OnForwardingToPortCountryCode { get; set; }
        public string OnForwardingToPortCountryName { get; set; }
        public DateTime? OnForwardingETD { get; set; }
        public DateTime? OnForwardingATD { get; set; }
        public DateTime? OnForwardingETA { get; set; }
        public DateTime? OnForwardingATA { get; set; }
        public string OnForwardingCarrierWebSite { get; set; }
        public string OnForwardingVesselId { get; set; }
        public string OnForwardingVesselName { get; set; }
        public string OnForwardingAdditionalTransportModeCode { get; set; }
        public bool HasOnForwarding { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SplitOnForwarding { get; set; }
        public DateTime? OnForwardingATD_Original { get; set; }
        public DateTime? OnForwardingETD_Original { get; set; }
        public DateTime? OnForwardingATA_Original { get; set; }
        public DateTime? OnForwardingETA_Original { get; set; }
        public string OriginPreCarriageFromPortId { get; set; }
        public string OriginOnCarriageToPortId { get; set; }
        public string OriginPreCarriageToPortId { get; set; }
        public string OriginOnCarriageFromPortId { get; set; }
        public string TotalTax { get; set; }
        public string WarehouseLegLocalName { get; set; }
        public string WarehouseLegEnglishName { get; set; }
        public bool IsHTSMissing { get; set; }

        private List<ShipmentProductItemPM> shipmentProductItems;
        [Include]
        [Association("ShipmentProductItemShipment", "Id", "ShipmentId")]
        [Composition]
        public virtual List<ShipmentProductItemPM> ShipmentProductItems
        {
            get
            {

                if (this.shipmentProductItems == null)
                {
                    shipmentProductItems = new List<ShipmentProductItemPM>();
                }
                return this.shipmentProductItems;
            }
            set
            {
                if (value != null)
                {
                    shipmentProductItems = value;
                }
            }
        }
    }

    public class TransshipmentLeg
    {
        public string Id { get; set; }
        public int LegIndex { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string CarrierId { get; set; }
        public string CarrierNumber { get; set; }
        public string VesselId { get; set; }        
        public string MasterNumber { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }        
    }
}
