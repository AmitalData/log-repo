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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class SupplierInvoiceItem
    {
	 string dbms;

        [Key]
        [ForeignKey("SupplierInvoice")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual SupplierInvoice SupplierInvoice { get; set; }
     [Key]
        [ForeignKey("SupplierInvoice")]
        [Column("CounterKey" ,Order = 2)]
	    public int CounterKey { get; set; }
     [Key]
        [Column("LineNumber" ,Order = 3)]
	    public int LineNumber { get; set; }
        [Column("ItemCode")]
	    public string ItemCode { get; set; }
        [ForeignKey("OriginCountry")]
        [Column("OriginCountryCode")]
	    public string OriginCountryCode { get; set; }
	      
        public virtual CustomsCountry OriginCountry { get; set; }
        [Column("ClassificationCode")]
	    public string ClassificationCode { get; set; }
        [Column("DangerousClassificationCode")]
	    public string DangerousClassificationCode { get; set; }
        [ForeignKey("DangerousGoodsPackingReq")]
        [Column("DangerousPackingGroupTypeCode")]
	    public string DangerousPackingGroupTypeCode { get; set; }
	      
        public virtual DangerousGoodsPackingReq DangerousGoodsPackingReq { get; set; }
        [Column("ItemPrice")]
	    public decimal? ItemPrice { get; set; }
        [Column("NonCustomsItemPrice")]
	    public decimal? NonCustomsItemPrice { get; set; }
        [Column("WholeSaleItemPrice")]
	    public decimal? WholeSaleItemPrice { get; set; }
        [Column("ManufactureIdentifier")]
	    public string ManufactureIdentifier { get; set; }
        [ForeignKey("CustomsBookType")]
        [Column("CustomsBookTypeCode")]
	    public string CustomsBookTypeCode { get; set; }
	      
        public virtual CustomsBookType CustomsBookType { get; set; }
        [Column("TaxExemptCode")]
	    public string TaxExemptCode { get; set; }
        [Column("OptionalTamaPercentage")]
	    public decimal? OptionalTamaPercentage { get; set; }
        [ForeignKey("SalesTaxExemptionType")]
        [Column("SalesTaxExemptionTypeCode")]
	    public string SalesTaxExemptionTypeCode { get; set; }
	      
        public virtual SalesTaxExemptionType SalesTaxExemptionType { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
        [ForeignKey("ItemPriceCurrency")]
        [Column("ItemPriceCurrencyCode")]
	    public string ItemPriceCurrencyCode { get; set; }
	      
        public virtual CurrencyType ItemPriceCurrency { get; set; }
        [ForeignKey("NonCustomsItemPriceCurrency")]
        [Column("NonCustomsItemPriceCurCode")]
	    public string NonCustomsItemPriceCurCode { get; set; }
	      
        public virtual CurrencyType NonCustomsItemPriceCurrency { get; set; }
        [ForeignKey("WholeSaleItemPriceCurrency")]
        [Column("WholeSaleItemPriceCurrencyCode")]
	    public string WholeSaleItemPriceCurrencyCode { get; set; }
	      
        public virtual CurrencyType WholeSaleItemPriceCurrency { get; set; }
        [Column("ActualInvoiceLines")]
	    public string ActualInvoiceLines { get; set; }
        [ForeignKey("TradeAgreement")]
        [Column("TradeAgreementCode")]
	    public string TradeAgreementCode { get; set; }
	      
        public virtual TradeAgreement TradeAgreement { get; set; }
        [Column("StatisticQuantity")]
	    public decimal? StatisticQuantity { get; set; }
        [Column("InvoiceQuantity")]
	    public decimal? InvoiceQuantity { get; set; }
        [Column("AdditionalQuantity")]
	    public decimal? AdditionalQuantity { get; set; }
        [ForeignKey("InvoiceMeasurmentUnit")]
        [Column("InvoiceQuantityType")]
	    public string InvoiceQuantityType { get; set; }
	      
        public virtual MeasurmentUnit InvoiceMeasurmentUnit { get; set; }
        [ForeignKey("StatisticMeasurmentUnit")]
        [Column("StatisticQuantityType")]
	    public string StatisticQuantityType { get; set; }
	      
        public virtual MeasurmentUnit StatisticMeasurmentUnit { get; set; }
        [ForeignKey("AdditionalMeasurmentUnit")]
        [Column("AdditionalQuantityType")]
	    public string AdditionalQuantityType { get; set; }
	      
        public virtual MeasurmentUnit AdditionalMeasurmentUnit { get; set; }
        [Column("PreferenceDocumentNumber")]
	    public string PreferenceDocumentNumber { get; set; }
        [Column("ItemDescription")]
	    public string ItemDescription { get; set; }
        [ForeignKey("CertificatesStatus")]
        [Column("CertificatesStatusCode")]
	    public string CertificatesStatusCode { get; set; }
	      
        public virtual CertificatesStatus CertificatesStatus { get; set; }
        [Column("IsUsed")]
	    public bool IsUsed { get; set; }
        [Column("DeferredCustomsTax")]
	    public decimal? DeferredCustomsTax { get; set; }
        [Column("DeferredPurchaseTax")]
	    public decimal? DeferredPurchaseTax { get; set; }
        [Column("VehicleStatus")]
	    public bool VehicleStatus { get; set; }
        [Column("ItemAdditionalStatus")]
	    public bool ItemAdditionalStatus { get; set; }
        [Column("ItemHash")]
	    public string ItemHash { get; set; }
        [Column("ParentLineNumber")]
	    public int? ParentLineNumber { get; set; }
        [Column("NotForAccumaltion")]
	    public bool NotForAccumaltion { get; set; }
        [Column("IsParent")]
	    public bool IsParent { get; set; }
        [Column("UnfInvoiceLine")]
	    public int? UnfInvoiceLine { get; set; }
        [Column("OrderByLineNo")]
	    public string OrderByLineNo { get; set; }
        [Column("LastCopyFromOrderNo")]
	    public string LastCopyFromOrderNo { get; set; }
        [Column("ClasifiedRemarks")]
	    public string ClasifiedRemarks { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("MarksAndNumbers")]
	    public string MarksAndNumbers { get; set; }
        [Column("PackageQuantity")]
	    public int? PackageQuantity { get; set; }
        [Column("Weight")]
	    public decimal? Weight { get; set; }
        [Column("OcrHeight")]
	    public decimal OcrHeight { get; set; }
        [Column("OcrTop")]
	    public decimal OcrTop { get; set; }
        [Column("OcrPageNumber")]
	    public decimal OcrPageNumber { get; set; }
        [ForeignKey("ClassificationType")]
        [Column("ClassificationTypeCode")]
	    public string ClassificationTypeCode { get; set; }
	      
        public virtual ClassificationType ClassificationType { get; set; }
        [ForeignKey("TransactionNatureType")]
        [Column("TransactionNatureCode")]
	    public string TransactionNatureCode { get; set; }
	      
        public virtual TransactionNatureType TransactionNatureType { get; set; }
        [ForeignKey("ClaimReasonType")]
        [Column("ClaimReasonCode")]
	    public string ClaimReasonCode { get; set; }
	      
        public virtual ClaimReasonType ClaimReasonType { get; set; }
        [Column("ItemFOBAmountForeign")]
	    public decimal? ItemFOBAmountForeign { get; set; }
        [Column("ItemFOBAmountNIS")]
	    public decimal? ItemFOBAmountNIS { get; set; }
    }
}
	 