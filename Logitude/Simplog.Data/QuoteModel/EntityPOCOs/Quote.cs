using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class Quote
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteNumber { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentTypeId { get; set; }
        public string QuoteTemplateId { get; set; }
        public string ConcurrencyGUID { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public int LastVersionNumber { get; set; }
        public string Subject { get; set; }
        public bool IsSubjectEdited { get; set; }
        public bool TotalPerContainer { get; set; }

        #region Partners
        public string ConsigneeNotImporterId { get; set; }
        public string ConsigneeNotImporterAddressId { get; set; }
        public string ConsigneeNotImporterContactId { get; set; }
        public string ConsigneeNotImporterReference { get; set; }
        public virtual Address ConsigneeNotImporterAddress { get; set; }
        public virtual Contact ConsigneeNotImporterContact { get; set; }
        public virtual Card ConsigneeNotImporterCard { get; set; }

        public string ShipperNotExporterId { get; set; }
        public string ShipperNotExporterAddressId { get; set; }
        public string ShipperNotExporterContactId { get; set; }
        public string ShipperNotExporterReference { get; set; }
        public virtual Address ShipperNotExporterAddress { get; set; }
        public virtual Contact ShipperNotExporterContact { get; set; }
        public virtual Card ShipperNotExporterCard { get; set; }


        public string QuoteCustomerTypeCode { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerReference1 { get; set; }
        public string CustomerReference2 { get; set; }
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperContactId { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public string ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeContactId { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }

        public string FreelancerId { get; set; }
        public string FreelancerAddressId { get; set; }
        public string FreelancerContactId { get; set; }

        #endregion

        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string IncotermId { get; set; }
        public string SalesmanUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime OpenDate { get; set; }
        public string Notes { get; set; }
        public string DescriptionOfGoods { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? ChargeableWeightInKG { get; set; }

        public double? PickupDeliveryChargeableWeight { get; set; }
        public double? VolumeInCBM { get; set; }
        public double? GrossWeight { get; set; }
        public byte[] LastModified { get; set; }
        public bool IsClosed { get; set; }
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
        public string DepartmentId { get; set; }
        public string BranchId { get; set; }
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
        public bool IsByKG { get; set; }
        public bool IsByContainer { get; set; }
        public string QuoteTypeCode { get; set; }
        public double? EstimateProfit { get; set; }
        public bool EstimateProfitEdited { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public bool IsCancelled { get; set; }

        public string SearchFields { get; set; }
        public string PickUpAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public bool IncludePickUp { get; set; }
        public bool IncludeDelivery { get; set; }
        public string FromAddressId { get; set; }
        public string ToAddressId { get; set; }
        public string FromAddressCountryId { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string FromPartnerId { get; set; }
        public string ToPartnerId { get; set; }
        public string FromPartnerAddressId { get; set; }
        public string ToPartnerAddressId { get; set; }
        public DateTime? StageDueDate { get; set; }
        public string LastActivityTypeCode { get; set; }
        public string LastActivitySubject { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string NextActivityTypeCode { get; set; }
        public string NextActivitySubject { get; set; }
        public DateTime? NextActivityDate { get; set; }
        public string OpportunityId { get; set; }
        public bool IsAutomaticallyClosed { get; set; }
        public DateTime? AutomaticallyCloseDate { get; set; }
        public int? AutomaticallyCloseDays { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ProductCode { get; set; }
        public double? MinimumFreightCost { get; set; }
        public double? MinimumFreightSale { get; set; }
        public string TransitTime { get; set; }
        public string DepartureFrequency { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? LastStageDate { get; set; }
        public string AgentId { get; set; }
        public string AgentAddressId { get; set; }
        public string AgentContactId { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public double? TEU { get; set; }

        public string SaleCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        public bool IsFixedPrice { get; set; }
        public bool IsSaleCurrencySameAsCost { get; set; }
        public bool IsMultiCurrency { get; set; }

        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrencyId { get; set; }
        public bool IsChargesByVAT { get; set; }

        public bool IsQuoteDataExternal { get; set; }
        public bool IsQuoteDocumentExternal { get; set; }

        public string QuotationSections { get; set; }

        public double? GrossWeightInKG { get; set; }
        public double? GrossWeightPerTon { get; set; }

        public string NotifyId { get; set; }
        public string NotifyAddressId { get; set; }
        public string NotifyContactId { get; set; }
        public int? NumberOfFollowUps { get; set; }
        public string NotifyReference1 { get; set; }
        public string NotifyReference2 { get; set; }
        public string QuoteHTMLDocumentId { get; set; }

        public virtual Card NotifyCard { get; set; }
        public virtual Contact NotifyContact { get; set; }
        public virtual Address NotifyAddress { get; set; }

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

        public string CountryForStatisticsId { get; set; }

        [ForeignKey("AgentId")]
        public virtual Card AgentCard { get; set; }

        [ForeignKey("AgentAddressId")]
        public virtual Address AgentAddress { get; set; }

        [ForeignKey("AgentContactId")]
        public virtual Contact AgentContact { get; set; }

        public string UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        public string BusinessUnitId { get; set; }
        [ForeignKey("BusinessUnitId")]
        public virtual BusinessUnit BusinessUnit { get; set; }

        public string RatingCode { get; set; }
        [ForeignKey("RatingCode")]
        public virtual QuoteRating Rating { get; set; }

        public string StageId { get; set; }
        [ForeignKey("StageId")]
        public virtual QuoteStage Stage { get; set; }

        public string QuoteClosingReasonCode { get; set; }
        public string QuoteClosingReasonId { get; set; }
        [ForeignKey("QuoteClosingReasonId")]
        public virtual QuoteClosingReason QuoteClosingReason { get; set; }

        public int? UsageCount { get; set; }
        public DateTime? LastUsageDate { get; set; }

        [ForeignKey("FromAddressId")]
        public virtual Address PickUpAddressEntity { get; set; }

        [ForeignKey("ToAddressId")]
        public virtual Address DeliveryAddressEntity { get; set; }

        [ForeignKey("FromAddressCountryId")]
        public virtual Country FromAddressCountry { get; set; }

        [ForeignKey("ToAddressCountryId")]
        public virtual Country ToAddressCountry { get; set; }

        [ForeignKey("SaleCurrencyId")]
        public virtual Currency SaleCurrency { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Card CustomerCard { get; set; }

        [ForeignKey("CustomerContactId")]
        public virtual Contact CustomerContact { get; set; }

        [ForeignKey("MainCarriageCarrierId")]
        public virtual Card MainCarriageCarrierCard { get; set; }

        [ForeignKey("QuoteTypeCode")]
        public virtual QuoteType QuoteType { get; set; }

        [ForeignKey("PackageType1Id")]
        public virtual PackageType PackageType1 { get; set; }

        [ForeignKey("PackageType2Id")]
        public virtual PackageType PackageType2 { get; set; }

        [ForeignKey("PackageType3Id")]
        public virtual PackageType PackageType3 { get; set; }

        [ForeignKey("PackageType4Id")]
        public virtual PackageType PackageType4 { get; set; }

        [ForeignKey("PackageType5Id")]
        public virtual PackageType PackageType5 { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("FromPartnerId")]
        public virtual Card FromPartnerCard { get; set; }

        [ForeignKey("ToPartnerId")]
        public virtual Card ToPartnerCard { get; set; }

        [ForeignKey("FromPartnerAddressId")]
        public virtual Address FromPartnerAddress { get; set; }

        [ForeignKey("ToPartnerAddressId")]
        public virtual Address ToPartnerAddress { get; set; }

        [ForeignKey("FreelancerId")]
        public virtual Card FreelancerCard { get; set; }

        [ForeignKey("FreelancerAddressId")]
        public virtual Address FreelancerAdress { get; set; }

        [ForeignKey("FreelancerContactId")]
        public virtual Contact FreelancerContact { get; set; }

        [ForeignKey("QuoteTemplateId")]
        public virtual QuoteTemplate QuoteTemplate { get; set; }

        [ForeignKey("TransportModeId")]
        public virtual TransportMode TransportMode { get; set; }

        [ForeignKey("DirectionId")]
        public virtual Direction Direction { get; set; }

        [ForeignKey("ShipperId")]
        public virtual Card ShipperCard { get; set; }

        [ForeignKey("ShipperContactId")]
        public virtual Contact ShipperContact { get; set; }

        [ForeignKey("ConsigneeId")]
        public virtual Card ConsigneeCard { get; set; }

        [ForeignKey("ConsigneeContactId")]
        public virtual Contact ConsigneeContact { get; set; }

        [ForeignKey("SalesmanUserId")]
        public virtual User SalesmanUser { get; set; }

        [ForeignKey("FromPortId")]
        public virtual Port FromPort { get; set; }

        [ForeignKey("ToPortId")]
        public virtual Port ToPort { get; set; }

        [ForeignKey("IncotermId")]
        public virtual Incoterm Incoterm { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ShipmentTypeId")]
        public virtual ShipmentType ShipmentType { get; set; }

        [ForeignKey("QuoteCustomerTypeCode")]
        public virtual QuoteCustomerType QuoteCustomerType { get; set; }

        public string MoveTypeId { get; set; }

        [ForeignKey("MoveTypeId")]
        public virtual MoveType MoveType { get; set; }

        [ForeignKey("ValueOfGoodsCurrencyId")]
        public virtual Currency ValueOfGoodsCurrency { get; set; }

        public bool GrossWeightEdited { get; set; }
        public bool ChargeableWeightEdited { get; set; }

        [ForeignKey("QuoteHTMLDocumentId")]
        public virtual Document QuoteHTMLDocument { get; set; }

        [ForeignKey("CountryForStatisticsId")]
        public virtual Country CountryForStatistics { get; set; }

        public DateTime? RequestDate { get; set; }
        public double? EstimatedProfitInLocal { get; set; }
        public double? EstimatedProfitInProfit { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitExchangeRate { get; set; }
        [ForeignKey("ProfitCurrencyId")]
        public virtual Currency ProfitCurrency { get; set; }

        public string ShipmentSubTypeId { get; set; }
        public virtual ShipmentSubType ShipmentSubType { get; set; }
        public double? PickupDeliveryVolumetricWeight { get; set; }
        public string RegionalTaxId { get; set; }
        [ForeignKey("RegionalTaxId")]
        public virtual VatType RegionalTax { get; set; }
        public double? RegionalTaxPercentage { get; set; }
        public bool DescriptionRightToLeft { get; set; }
        public int? PackagesQuantity { get; set; }

        public string InlandDomesticFromZipCode { get; set; }
        public string InlandDomesticToZipCode { get; set; }
        public string InlandDomesticFromCity { get; set; }
        public string InlandDomesticToCity { get; set; }
        public string InlandDomesticFromCountryId { get; set; }
        public string InlandDomesticToCountryId { get; set; }
        public string InlandDomesticFromTypeCode { get; set; }
        public string InlandDomesticToTypeCode { get; set; }
        public string MainCarriageFromPortAddress { get; set; }
        public string MainCarriageToPortAddress { get; set; }

        [ForeignKey("InlandDomesticFromTypeCode")]
        public PickUpDeliveryFromToType InlandDomesticFromType { get; set; }

        [ForeignKey("InlandDomesticToTypeCode")]
        public PickUpDeliveryFromToType InlandDomesticToType { get; set; }

        [ForeignKey("InlandDomesticFromCountryId")]
        public virtual Country InlandDomesticFromCountry { get; set; }

        [ForeignKey("InlandDomesticToCountryId")]
        public virtual Country InlandDomesticToCountry { get; set; }


        public string SpecialServicesTypeId { get; set; }
        public bool IncludeInsurance { get; set; }
        public bool IsStackable { get; set; }
        public bool IncludeImportDutyCharges { get; set; }
        public double? InsuranceValue { get; set; }
        [ForeignKey("SpecialServicesTypeId")]
        public virtual SpecialServicesType SpecialServicesType { get; set; }

        public string ValidByTypeCode { get; set; }
        [ForeignKey("ValidByTypeCode")]
        public virtual ValidByType ValidByType { get; set; }
        public bool? ConnectedToOpportunity { get; set; }

        public string QuoteClosingReasonNotes { get; set; }
    }
}