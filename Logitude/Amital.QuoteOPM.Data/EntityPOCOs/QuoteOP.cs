using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOP
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("QuoteOPTemplate")]
        [Column("QuoteTemplateId")]
	    public string QuoteTemplateId { get; set; }
	      
        public virtual QuoteOPTemplate QuoteOPTemplate { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [Column("LastVersionNumber")]
	    public int LastVersionNumber { get; set; }
        [ForeignKey("FreelancerCard")]
        [Column("FreelancerId")]
	    public string FreelancerId { get; set; }
	      
        public virtual Card FreelancerCard { get; set; }
        [ForeignKey("FreelancerAdress")]
        [Column("FreelancerAddressId")]
	    public string FreelancerAddressId { get; set; }
	      
        public virtual Address FreelancerAdress { get; set; }
        [ForeignKey("FreelancerContact")]
        [Column("FreelancerContactId")]
	    public string FreelancerContactId { get; set; }
	      
        public virtual Contact FreelancerContact { get; set; }
        [Column("LastModified")]
	    public string LastModified { get; set; }
        [Column("Field1")]
	    public string Field1 { get; set; }
        [Column("Field2")]
	    public string Field2 { get; set; }
        [Column("Field3")]
	    public string Field3 { get; set; }
        [Column("Field4")]
	    public string Field4 { get; set; }
        [Column("Field5")]
	    public string Field5 { get; set; }
        [Column("Field6")]
	    public string Field6 { get; set; }
        [Column("Field7")]
	    public string Field7 { get; set; }
        [Column("Field8")]
	    public string Field8 { get; set; }
        [Column("Field9")]
	    public string Field9 { get; set; }
        [Column("Field10")]
	    public string Field10 { get; set; }
        [Column("IsByKG")]
	    public bool IsByKG { get; set; }
        [Column("IsByContainer")]
	    public bool IsByContainer { get; set; }
        [Column("EstimateProfitEdited")]
	    public bool EstimateProfitEdited { get; set; }
        [ForeignKey("PickUpAddressEntity")]
        [Column("FromAddressId")]
	    public string FromAddressId { get; set; }
	      
        public virtual Address PickUpAddressEntity { get; set; }
        [ForeignKey("DeliveryAddressEntity")]
        [Column("ToAddressId")]
	    public string ToAddressId { get; set; }
	      
        public virtual Address DeliveryAddressEntity { get; set; }
        [Column("OpportunityId")]
	    public string OpportunityId { get; set; }
        [Column("MinimumFreightCost")]
	    public double? MinimumFreightCost { get; set; }
        [Column("MinimumFreightSale")]
	    public double? MinimumFreightSale { get; set; }
        [Column("LastStageDate")]
	    public DateTime? LastStageDate { get; set; }
        [Column("AgentReference1")]
	    public string AgentReference1 { get; set; }
        [Column("AgentReference2")]
	    public string AgentReference2 { get; set; }
        [Column("IsSaleCurrencySameAsCost")]
	    public bool IsSaleCurrencySameAsCost { get; set; }
        [Column("EstimateProfit")]
	    public double? EstimateProfit { get; set; }
        [Column("IsFixedPrice")]
	    public bool IsFixedPrice { get; set; }
        [ForeignKey("CustomerContact")]
        [Column("CustomerContactId")]
	    public string CustomerContactId { get; set; }
	      
        public virtual Contact CustomerContact { get; set; }
        [Column("ShipperName")]
	    public string ShipperName { get; set; }
        [Column("ConsigneeName")]
	    public string ConsigneeName { get; set; }
        [Column("DeliveryAddress")]
	    public string DeliveryAddress { get; set; }
        [Column("PickUpAddress")]
	    public string PickUpAddress { get; set; }
        [ForeignKey("SaleCurrency")]
        [Column("SaleCurrencyId")]
	    public string SaleCurrencyId { get; set; }
	      
        public virtual Currency SaleCurrency { get; set; }
        [Column("ExchangeRate")]
	    public double? ExchangeRate { get; set; }
        [Column("CustomerName")]
	    public string CustomerName { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("QuoteNumber")]
	    public string QuoteNumber { get; set; }
        [Column("MainCarriageCarrierId")]
	    public string MainCarriageCarrierId { get; set; }
        [ForeignKey("Direction")]
        [Column("DirectionId")]
	    public string DirectionId { get; set; }
	      
        public virtual Direction Direction { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("Department")]
        [Column("DepartmentId")]
	    public string DepartmentId { get; set; }
	      
        public virtual Department Department { get; set; }
        [ForeignKey("Branch")]
        [Column("BranchId")]
	    public string BranchId { get; set; }
	      
        public virtual Branch Branch { get; set; }
        [ForeignKey("ShipmentType")]
        [Column("ShipmentTypeId")]
	    public string ShipmentTypeId { get; set; }
	      
        public virtual ShipmentType ShipmentType { get; set; }
        [ForeignKey("QuoteOPCustomerType")]
        [Column("QuoteCustomerTypeCode")]
	    public string QuoteCustomerTypeCode { get; set; }
	      
        public virtual QuoteOPCustomerType QuoteOPCustomerType { get; set; }
        [ForeignKey("CustomerCard")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card CustomerCard { get; set; }
        [ForeignKey("ShipperCard")]
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
	      
        public virtual Card ShipperCard { get; set; }
        [ForeignKey("ShipperContact")]
        [Column("ShipperContactId")]
	    public string ShipperContactId { get; set; }
	      
        public virtual Contact ShipperContact { get; set; }
        [Column("ShipperReference1")]
	    public string ShipperReference1 { get; set; }
        [Column("ShipperReference2")]
	    public string ShipperReference2 { get; set; }
        [ForeignKey("ConsigneeCard")]
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
	      
        public virtual Card ConsigneeCard { get; set; }
        [ForeignKey("ConsigneeContact")]
        [Column("ConsigneeContactId")]
	    public string ConsigneeContactId { get; set; }
	      
        public virtual Contact ConsigneeContact { get; set; }
        [Column("ConsigneeReference1")]
	    public string ConsigneeReference1 { get; set; }
        [Column("ConsigneeReference2")]
	    public string ConsigneeReference2 { get; set; }
        [Column("FromPortId")]
	    public string FromPortId { get; set; }
        [Column("ToPortId")]
	    public string ToPortId { get; set; }
        [Column("IncotermId")]
	    public string IncotermId { get; set; }
        [ForeignKey("SalesmanUser")]
        [Column("SalesmanUserId")]
	    public string SalesmanUserId { get; set; }
	      
        public virtual User SalesmanUser { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("OpenDate")]
	    public DateTime OpenDate { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("DescriptionOfGoods")]
	    public string DescriptionOfGoods { get; set; }
        [Column("ChargeableWeight")]
	    public double? ChargeableWeight { get; set; }
        [Column("GrossWeight")]
	    public double? GrossWeight { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("DimensionsUnitCode")]
	    public string DimensionsUnitCode { get; set; }
        [Column("Volume")]
	    public double? Volume { get; set; }
        [Column("Ratio")]
	    public double? Ratio { get; set; }
        [Column("NumberOfPackages")]
	    public int? NumberOfPackages { get; set; }
        [Column("NumberOfContainers")]
	    public int? NumberOfContainers { get; set; }
        [Column("VolumeUnitCode")]
	    public string VolumeUnitCode { get; set; }
        [Column("IsDangerous")]
	    public bool IsDangerous { get; set; }
        [Column("ExpirationDays")]
	    public int? ExpirationDays { get; set; }
        [Column("ExpirationDate")]
	    public DateTime? ExpirationDate { get; set; }
        [Column("IsFreightBySteps")]
	    public bool IsFreightBySteps { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("PackageType1")]
        [Column("PackageType1Id")]
	    public string PackageType1Id { get; set; }
	      
        public virtual PackageType PackageType1 { get; set; }
        [ForeignKey("PackageType2")]
        [Column("PackageType2Id")]
	    public string PackageType2Id { get; set; }
	      
        public virtual PackageType PackageType2 { get; set; }
        [ForeignKey("PackageType3")]
        [Column("PackageType3Id")]
	    public string PackageType3Id { get; set; }
	      
        public virtual PackageType PackageType3 { get; set; }
        [ForeignKey("PackageType4")]
        [Column("PackageType4Id")]
	    public string PackageType4Id { get; set; }
	      
        public virtual PackageType PackageType4 { get; set; }
        [ForeignKey("PackageType5")]
        [Column("PackageType5Id")]
	    public string PackageType5Id { get; set; }
	      
        public virtual PackageType PackageType5 { get; set; }
        [Column("PackageType1Quantity")]
	    public int? PackageType1Quantity { get; set; }
        [Column("PackageType2Quantity")]
	    public int? PackageType2Quantity { get; set; }
        [Column("PackageType3Quantity")]
	    public int? PackageType3Quantity { get; set; }
        [Column("PackageType4Quantity")]
	    public int? PackageType4Quantity { get; set; }
        [Column("PackageType5Quantity")]
	    public int? PackageType5Quantity { get; set; }
        [ForeignKey("QuoteOPType")]
        [Column("QuoteTypeCode")]
	    public string QuoteTypeCode { get; set; }
	      
        public virtual QuoteOPType QuoteOPType { get; set; }
        [Column("GrossWeightUnitCode")]
	    public string GrossWeightUnitCode { get; set; }
        [Column("ChargeableWeightUnitCode")]
	    public string ChargeableWeightUnitCode { get; set; }
        [Column("VolumetricWeight")]
	    public double? VolumetricWeight { get; set; }
        [ForeignKey("FromPartnerCard")]
        [Column("FromPartnerId")]
	    public string FromPartnerId { get; set; }
	      
        public virtual Card FromPartnerCard { get; set; }
        [ForeignKey("ToPartnerCard")]
        [Column("ToPartnerId")]
	    public string ToPartnerId { get; set; }
	      
        public virtual Card ToPartnerCard { get; set; }
        [ForeignKey("FromPartnerAddress")]
        [Column("FromPartnerAddressId")]
	    public string FromPartnerAddressId { get; set; }
	      
        public virtual Address FromPartnerAddress { get; set; }
        [ForeignKey("ToPartnerAddress")]
        [Column("ToPartnerAddressId")]
	    public string ToPartnerAddressId { get; set; }
	      
        public virtual Address ToPartnerAddress { get; set; }
        [Column("FromAddressCity")]
	    public string FromAddressCity { get; set; }
        [ForeignKey("FromAddressCountry")]
        [Column("FromAddressCountryId")]
	    public string FromAddressCountryId { get; set; }
	      
        public virtual Country FromAddressCountry { get; set; }
        [Column("FromAddressZipCode")]
	    public string FromAddressZipCode { get; set; }
        [Column("ToAddressCity")]
	    public string ToAddressCity { get; set; }
        [ForeignKey("ToAddressCountry")]
        [Column("ToAddressCountryId")]
	    public string ToAddressCountryId { get; set; }
	      
        public virtual Country ToAddressCountry { get; set; }
        [Column("ToAddressZipCode")]
	    public string ToAddressZipCode { get; set; }
        [Column("DimFactor")]
	    public double? DimFactor { get; set; }
        [Column("IncludePickUp")]
	    public bool IncludePickUp { get; set; }
        [Column("IncludeDelivery")]
	    public bool IncludeDelivery { get; set; }
        [Column("QuoteClosingReasonCode")]
	    public string QuoteClosingReasonCode { get; set; }
        [Column("SentDate")]
	    public DateTime? SentDate { get; set; }
        [Column("AcceptedDate")]
	    public DateTime? AcceptedDate { get; set; }
        [Column("DeclinedDate")]
	    public DateTime? DeclinedDate { get; set; }
        [Column("UsageCount")]
	    public int? UsageCount { get; set; }
        [Column("LastUsageDate")]
	    public DateTime? LastUsageDate { get; set; }
        [ForeignKey("BusinessUnit")]
        [Column("BusinessUnitId")]
	    public string BusinessUnitId { get; set; }
	      
        public virtual BusinessUnit BusinessUnit { get; set; }
        [Column("CustomerReference1")]
	    public string CustomerReference1 { get; set; }
        [Column("CustomerReference2")]
	    public string CustomerReference2 { get; set; }
        [Column("Subject")]
	    public string Subject { get; set; }
        [Column("IsSubjectEdited")]
	    public bool IsSubjectEdited { get; set; }
        [ForeignKey("Stage")]
        [Column("StageId")]
	    public string StageId { get; set; }
	      
        public virtual QuoteOPStage Stage { get; set; }
        [Column("StageDueDate")]
	    public DateTime? StageDueDate { get; set; }
        [ForeignKey("Rating")]
        [Column("RatingCode")]
	    public string RatingCode { get; set; }
	      
        public virtual QuoteOPRating Rating { get; set; }
        [Column("LastActivityDate")]
	    public DateTime? LastActivityDate { get; set; }
        [Column("LastActivitySubject")]
	    public string LastActivitySubject { get; set; }
        [Column("LastActivityTypeCode")]
	    public string LastActivityTypeCode { get; set; }
        [Column("NextActivityDate")]
	    public DateTime? NextActivityDate { get; set; }
        [Column("NextActivitySubject")]
	    public string NextActivitySubject { get; set; }
        [Column("NextActivityTypeCode")]
	    public string NextActivityTypeCode { get; set; }
        [Column("IsAutomaticallyClosed")]
	    public bool IsAutomaticallyClosed { get; set; }
        [Column("AutomaticallyCloseDate")]
	    public DateTime? AutomaticallyCloseDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("AutomaticallyCloseDays")]
	    public int? AutomaticallyCloseDays { get; set; }
        [Column("ProductCode")]
	    public string ProductCode { get; set; }
        [Column("TransitTime")]
	    public string TransitTime { get; set; }
        [Column("DepartureFrequency")]
	    public string DepartureFrequency { get; set; }
        [Column("ETD")]
	    public DateTime? ETD { get; set; }
        [Column("ETA")]
	    public DateTime? ETA { get; set; }
        [ForeignKey("AgentCard")]
        [Column("AgentId")]
	    public string AgentId { get; set; }
	      
        public virtual Card AgentCard { get; set; }
        [ForeignKey("AgentAddress")]
        [Column("AgentAddressId")]
	    public string AgentAddressId { get; set; }
	      
        public virtual Address AgentAddress { get; set; }
        [ForeignKey("AgentContact")]
        [Column("AgentContactId")]
	    public string AgentContactId { get; set; }
	      
        public virtual Contact AgentContact { get; set; }
        [ForeignKey("MoveType")]
        [Column("MoveTypeId")]
	    public string MoveTypeId { get; set; }
	      
        public virtual MoveType MoveType { get; set; }
        [Column("TEU")]
	    public double? TEU { get; set; }
        [Column("ValueOfGoods")]
	    public double? ValueOfGoods { get; set; }
        [ForeignKey("ValueOfGoodsCurrency")]
        [Column("ValueOfGoodsCurrencyId")]
	    public string ValueOfGoodsCurrencyId { get; set; }
	      
        public virtual Currency ValueOfGoodsCurrency { get; set; }
        [Column("IsChargesByVAT")]
	    public bool IsChargesByVAT { get; set; }
        [Column("IsQuoteDataExternal")]
	    public bool IsQuoteDataExternal { get; set; }
        [Column("IsQuoteDocumentExternal")]
	    public bool IsQuoteDocumentExternal { get; set; }
        [Column("TotalPerContainer")]
	    public bool TotalPerContainer { get; set; }
        [Column("QuotationSections")]
	    public string QuotationSections { get; set; }
        [Column("GrossWeightInKG")]
	    public double? GrossWeightInKG { get; set; }
        [Column("GrossWeightPerTon")]
	    public double? GrossWeightPerTon { get; set; }
        [ForeignKey("NotifyCard")]
        [Column("NotifyId")]
	    public string NotifyId { get; set; }
	      
        public virtual Card NotifyCard { get; set; }
        [ForeignKey("NotifyAddress")]
        [Column("NotifyAddressId")]
	    public string NotifyAddressId { get; set; }
	      
        public virtual Address NotifyAddress { get; set; }
        [ForeignKey("NotifyContact")]
        [Column("NotifyContactId")]
	    public string NotifyContactId { get; set; }
	      
        public virtual Contact NotifyContact { get; set; }
        [Column("NumberOfFollowUps")]
	    public int? NumberOfFollowUps { get; set; }
        [Column("GrossWeightEdited")]
	    public bool GrossWeightEdited { get; set; }
        [Column("ChargeableWeightEdited")]
	    public bool ChargeableWeightEdited { get; set; }
        [Column("ChargeableWeightInKG")]
	    public double? ChargeableWeightInKG { get; set; }
        [Column("VolumeInCBM")]
	    public double? VolumeInCBM { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("Field11")]
	    public string Field11 { get; set; }
        [Column("Field12")]
	    public string Field12 { get; set; }
        [Column("Field13")]
	    public string Field13 { get; set; }
        [Column("Field14")]
	    public string Field14 { get; set; }
        [Column("Field15")]
	    public string Field15 { get; set; }
        [Column("Field16")]
	    public string Field16 { get; set; }
        [Column("Field17")]
	    public string Field17 { get; set; }
        [Column("Field18")]
	    public string Field18 { get; set; }
        [Column("Field19")]
	    public string Field19 { get; set; }
        [Column("Field20")]
	    public string Field20 { get; set; }
        [Column("RequestDate")]
	    public DateTime? RequestDate { get; set; }
        [Column("EstimatedProfitInLocal")]
	    public double? EstimatedProfitInLocal { get; set; }
        [Column("EstimatedProfitInProfit")]
	    public double? EstimatedProfitInProfit { get; set; }
        [ForeignKey("ProfitCurrency")]
        [Column("ProfitCurrencyId")]
	    public string ProfitCurrencyId { get; set; }
	      
        public virtual Currency ProfitCurrency { get; set; }
        [Column("ProfitExchangeRate")]
	    public double? ProfitExchangeRate { get; set; }
        [ForeignKey("CountryForStatistics")]
        [Column("CountryForStatisticsId")]
	    public string CountryForStatisticsId { get; set; }
	      
        public virtual Country CountryForStatistics { get; set; }
        [ForeignKey("QuoteHTMLDocument")]
        [Column("QuoteHTMLDocumentId")]
	    public string QuoteHTMLDocumentId { get; set; }
	      
        public virtual Document QuoteHTMLDocument { get; set; }
        [ForeignKey("QuoteOPClosingReason")]
        [Column("QuoteClosingReasonId")]
	    public string QuoteClosingReasonId { get; set; }
	      
        public virtual QuoteOPClosingReason QuoteOPClosingReason { get; set; }
        [ForeignKey("ShipmentSubType")]
        [Column("ShipmentSubTypeId")]
	    public string ShipmentSubTypeId { get; set; }
	      
        public virtual ShipmentSubType ShipmentSubType { get; set; }
        [Column("PickupDeliveryRatio")]
	    public double? PickupDeliveryRatio { get; set; }
        [Column("PickupDeliveryChargeableWeight")]
	    public double? PickupDeliveryChargeableWeight { get; set; }
        [Column("PickupDeliveryVolumetricWeight")]
	    public double? PickupDeliveryVolumetricWeight { get; set; }
        [Column("PickupDeliveryCWeightUnitCode")]
	    public string PickupDeliveryCWeightUnitCode { get; set; }
        [ForeignKey("RegionalTax")]
        [Column("RegionalTaxId")]
	    public string RegionalTaxId { get; set; }
	      
        public virtual VatType RegionalTax { get; set; }
        [Column("RegionalTaxPercentage")]
	    public double? RegionalTaxPercentage { get; set; }
        [Column("DescriptionRightToLeft")]
	    public bool DescriptionRightToLeft { get; set; }
        [Column("AutomaticLastUpdateDate")]
	    public DateTime? AutomaticLastUpdateDate { get; set; }
        [Column("IsMultiCurrency")]
	    public bool IsMultiCurrency { get; set; }
        [Column("SpecialServiceId")]
	    public string SpecialServiceId { get; set; }
    }
}
	 