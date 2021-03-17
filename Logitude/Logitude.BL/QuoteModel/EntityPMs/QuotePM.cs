using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class QuotePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string ConcurrencyGUID { get; set; }
        public string QuoteTemplateId { get; set; }
        public int LastVersionNumber { get; set; }
        public string QuoteLevel { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string QuoteNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentTypeId { get; set; }

        public bool IsCustomerSet { get; set; }
        public string BaseShipmentNumber { get; set; }

        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string SaleCurrencyCode { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Subject { get; set; }

        public bool IsSubjectEdited { get; set; }

        #region Partners
        public string QuoteCustomerTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNote { get; set; }

        public string FreelancerId { get; set; }
        public string FreelancerAddressId { get; set; }
        public string FreelancerContactId { get; set; }
        public string FreelancerName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipperId { get; set; }
        public string ShipperContactId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public string ShipperName { get; set; }
        public string ShipperNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConsigneeId { get; set; }
        public string ConsigneeContactId { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeNote { get; set; }

        //dummy
        public string ShipperMainAddressId { get; set; }
        public string ShipperPickAddressId { get; set; }
        public string ConsigneeMainAddressId { get; set; }
        public string ConsigneePickAddressId { get; set; }
        #endregion

        public string CustomerRankName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BusinessUnitId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromPortId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ToPortId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IncotermId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserId { get; set; }

        public string SalesmanName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime OpenDate { get; set; }

        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }

        public double? ChargeableWeight { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? PickupDeliveryChargeableWeight { get; set; }
        public double? GrossWeight { get; set; }
        public double? GrossWeightInKG { get; set; }
        public double? GrossWeightPerTon { get; set; }
        public double? VolumeInCBM { get; set; } 
        public bool IsRefreshQuoteFollowUps { get; set; } 
        public bool IsRefreshFollowUp { get; set; }
        public byte[] LastModified { get; set; }
        public bool IsClosed { get; set; }
        public bool IsFixedPrice { get; set; }
        public double? TotalReceivablesAmount { get; set; }

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

        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string PickupDeliveryCWeightUnitCode { get; set; }

        public string DimensionsUnitCode { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? Volume { get; set; }
        public int? NumberOfPackages { get; set; }
        public int? NumberOfContainers { get; set; }
        public double? Ratio { get; set; }
        public double? PickupDeliveryRatio { get; set; }

        public double? DimFactor { get; set; }
        public string VolumeUnitCode { get; set; }
        public bool IsDangerous { get; set; }
        public int? ExpirationDays { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? StartDate { get; set; }

        public bool IsFreightBySteps { get; set; }
        public bool TotalPerContainer { get; set; }
        public string FromPort { get; set; }
        public string FromPortName { get; set; }
        public string FromPortCountry { get; set; }

        public string ToPort { get; set; }
        public string ToPortName { get; set; }
        public string ToPortCountry { get; set; }

        public string TotalContainers { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DepartmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchId { get; set; }

        public string DepartmentName { get; set; }
        public string BranchName { get; set; }

        public string PackageType1Id { get; set; }
        public string PackageType2Id { get; set; }
        public string PackageType3Id { get; set; }
        public string PackageType4Id { get; set; }
        public string PackageType5Id { get; set; }

        public int? PackageType1Quantity { get; set; }
        public int? PackageType2Quantity { get; set; }
        public int? PackageType3Quantity { get; set; }
        public int? PackageType4Quantity { get; set; }
        public int? PackageType5Quantity { get; set; }

        public string QuoteTypeCode { get; set; }
        public string QuoteTypeName { get; set; }

        public bool IsByKG { get; set; }
        public bool IsByContainer { get; set; }

        public string MainCarriageCarrierId { get; set; }

        public bool IsCancelled { get; set; }

        public string ActionType { get; set; }
        public string EventNote { get; set; }

        public string SaleCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }

        public string SearchFields { get; set; }

        public string FromPartnerId { get; set; }
        public string ToPartnerId { get; set; }
        public string FromPartnerAddressId { get; set; }
        public string ToPartnerAddressId { get; set; }

        public bool IncludePickUp { get; set; }
        public bool IncludeDelivery { get; set; }
        public string PickUpAddressId { get; set; }
        public string PickUpAddress { get; set; }
        public string DeliveryAddressId { get; set; }
        public string DeliveryAddress { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromAddressCountryId { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }

        public int? UsageCount { get; set; }
        public DateTime? LastUsageDate { get; set; }

        public string QuoteClosingReasonCode { get; set; }
        public string QuoteClosingReasonId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string StageId { get; set; }
        public string StageName { get; set; }
        public int? StageMaxDays { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? StageDueDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RatingCode { get; set; }
        public string RatingName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastActivityTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastActivitySubject { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastActivityDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NextActivityTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NextActivitySubject { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? NextActivityDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OpportunityId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsAutomaticallyClosed { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? AutomaticallyCloseDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? AutomaticallyCloseDays { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ProductCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastStageDate { get; set; }

        public double? TEU { get; set; }
        public bool IsChargesByVAT { get; set; }

        //dummy
        public bool IsCopy { get; set; }
        public string ToCountryId { get; set; }
        public string FromCountryId { get; set; }
        public bool FromCountryIsEC { get; set; }
        public bool ToCountryIsEC { get; set; }
        public string FromPartnerName { get; set; }
        public string ToPartnerName { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public bool IsPotentialShipper { get; set; }
        public bool IsPotentialConsignee { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }
        public string FromCountryCode { get; set; }
        public string FromCountryName { get; set; }
        public string ToCountryCode { get; set; }
        public string ToCountryName { get; set; }
        public bool ConvertToFCL { get; set; }
        public bool ConvertToLCL { get; set; }

        public bool IsQuoteDataExternal { get; set; }
        public bool IsQuoteDocumentExternal { get; set; }

        public string ExternalEntityNumber { get; set; }
        public string TransitTime { get; set; }
        public string DepartureFrequency { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }

        public string ETDLabel { get; set; }
        public string ETALabel { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentId { get; set; }

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

        public string MoveTypeId { get; set; }

        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrencyId { get; set; }

        #region Summary Fields
        public double? EstimateProfit { get; set; }
        public bool EstimateProfitEdited { get; set; }

        public double? CostTotalAmountInLocalCurrency { get; set; }
        public double? SaleTotalAmountInLocalCurrency { get; set; }
        public double? CostTotalAmountInSaleCurrency { get; set; }
        public double? SaleTotalAmountInSaleCurrency { get; set; }
        public double? EstimateProfitInSaleCurrency { get; set; }
        public bool IsHybrid { get; set; }
        public double? TotalSaleIncludingVATAmountInSaleCurrency { get; set; }
        public double? TotalSaleIncludingVATAmountInLocalCurrency { get; set; }
        #endregion

        public bool IsSaleCurrencySameAsCost { get; set; }
        public bool IsMultiCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NotifyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NotifyAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NotifyContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NotifyName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string NotifyNote { get; set; }

        public string NotifyAddress1 { get; set; }
        public string NotifyAddress2 { get; set; }
        public string NotifyZipCode { get; set; }
        public string NotifyStateId { get; set; }
        public string NotifyCountryId { get; set; }
        public string NotifyCity { get; set; }

        public bool DontExportQuotationsToIntegratedSystem { get; set; }
        public string QuotationSections { get; set; }
        public string SameOrFixed { get; set; }

        public bool GrossWeightEdited { get; set; }
        public bool ChargeableWeightEdited { get; set; }

        public string QuoteHTMLDocumentId { get; set; }
        public string QuoteVersion { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? NumberOfFollowUps { get; set; }


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

        public string CountryForStatisticsId { get; set; }

        private List<QuoteChargePM> quoteCharges;
        [Include]
        [Association("QuoteQuoteCharge", "Id", "QuoteId")]
        [Composition]
        public virtual List<QuoteChargePM> QuoteCharges
        {
            get
            {

                if (this.quoteCharges == null)
                {
                    quoteCharges = new List<QuoteChargePM>();
                }
                return this.quoteCharges;
            }
            set
            {
                if (value != null)
                {
                    quoteCharges = value;
                }
            }
        }

        private List<QuoteFollowUpPM> followUps;
        [Include]
        [Composition]
        [Association("FollowUpQuote", "Id", "QuoteId")]
        public virtual List<QuoteFollowUpPM> FollowUps
        {
            get
            {
                if (followUps == null)
                {
                    followUps = new List<QuoteFollowUpPM>();
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


        private List<QuoteDocumentVersionPM> quoteDocumentVersions;
        [Include]
        [Association("QuoteTemplateQuoteDocumentVersion", "Id", "QuoteId")]
        [Composition]
        public virtual List<QuoteDocumentVersionPM> QuoteDocumentVersions
        {
            get
            {
                if (quoteDocumentVersions == null)
                {
                    quoteDocumentVersions = new List<QuoteDocumentVersionPM>();
                }

                return this.quoteDocumentVersions;
            }
            set
            {
                if (value != null)
                {
                    quoteDocumentVersions = value;
                }
            }
        }

        private List<QuoteCostChargePM> quoteCostCharge;
        public List<QuoteCostChargePM> QuoteCostCharges
        {
            get
            {
                if (quoteCostCharge == null)
                {
                    quoteCostCharge = new List<QuoteCostChargePM>();
                }

                return quoteCostCharge;
            }

            set
            {
                if (value != null)
                {
                    quoteCostCharge = value;
                }
            }
        }

        private List<QuoteSaleChargePM> quotationSaleCharges;
        public List<QuoteSaleChargePM> QuotationSaleCharges
        {
            get
            {
                if (quotationSaleCharges == null)
                {
                    quotationSaleCharges = new List<QuoteSaleChargePM>();
                }

                return quotationSaleCharges;
            }

            set
            {
                if (value != null)
                {
                    quotationSaleCharges = value;
                }
            }
        }


        private List<QuoteSaleChargePM> quoteSaleCharges;
        public List<QuoteSaleChargePM> QuoteSaleCharges
        {
            get
            {
                if (quoteSaleCharges == null)
                {
                    quoteSaleCharges = new List<QuoteSaleChargePM>();
                }

                return quoteSaleCharges;
            }

            set
            {
                if (value != null)
                {
                    quoteSaleCharges = value;
                }
            }
        }

        public bool MarkFollowUpsAsDone { get; set; }
        public bool IsCopyExchangeRates { get; set; }

        private List<QuotePackagePM> quotePackagePM;
        [Include]
        [Composition]
        [Association("QuotePackagePMQuote", "Id", "QuoteId")]
        public virtual List<QuotePackagePM> QuotePackages
        {
            get
            {
                if (quotePackagePM == null)
                {
                    quotePackagePM = new List<QuotePackagePM>();
                }

                return this.quotePackagePM;
            }
            set
            {
                if (value != null)
                {
                    quotePackagePM = value;
                }
            }
        }

        public string SalesTotalAmounts { get; set; }

        private List<QuoteSalesTotalPM> quoteSalesTotals;
        public List<QuoteSalesTotalPM> QuoteSalesTotals
        {
            get
            {
                if (quoteSalesTotals == null)
                {
                    quoteSalesTotals = new List<QuoteSalesTotalPM>();
                }

                return quoteSalesTotals;
            }

            set
            {
                if (value != null)
                {
                    quoteSalesTotals = value;
                }
            }
        }

        private List<QuoteVATsTotalPM> totalVATPerQuote;
        public List<QuoteVATsTotalPM> TotalVATPerQuote
        {
            get
            {
                if (totalVATPerQuote == null)
                {
                    totalVATPerQuote = new List<QuoteVATsTotalPM>();
                }

                return totalVATPerQuote;
            }

            set
            {
                if (value != null)
                {
                    totalVATPerQuote = value;
                }
            }
        }

        private List<QuoteTotalVATPM> totalVATs;
        [Include]
        [Composition]
        [Association("QuoteTotalVATPMQuote", "Id", "QuoteId")]
        public virtual List<QuoteTotalVATPM> TotalVATs
        {
            get
            {
                if (totalVATs == null)
                {
                    totalVATs = new List<QuoteTotalVATPM>();
                }

                return this.totalVATs;
            }

            set
            {
                if (value != null)
                {
                    totalVATs = value;
                }
            }
        }


        public DateTime? RequestDate { get; set; }

        public bool IsCreatedFromTicket { get; set; }
        public DateTime? TicketCreateDate { get; set; }
        public double? EstimatedProfitInLocal { get; set; }
        public double? EstimatedProfitInProfit { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitExchangeRate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentSubTypeId { get; set; }
        public string ShipmentSubTypeName { get; set; }
        public string PickupCity { get; set; }
        public string PickupCountryId { get; set; }
        public string PickupZipCode { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryCountryId { get; set; }
        public string DeliveryZipCode { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? PickupDeliveryVolumetricWeight { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegionalTaxId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? RegionalTaxPercentage { get; set; }

        public bool DescriptionRightToLeft { get; set; }

        public double? PercentForeignChargesLocal { get; set; }
    }
}
