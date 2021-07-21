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
   
    public class SupplierInvoice
    {
	 string dbms;

        [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationId" ,Order = 1)]
	    public string DeclarationId { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
     [Key]
        [Column("InvoiceCounterKey" ,Order = 2)]
	    public int InvoiceCounterKey { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
        [Column("InvoiceNumber")]
	    public string InvoiceNumber { get; set; }
        [Column("IssueDate")]
	    public DateTime? IssueDate { get; set; }
        [ForeignKey("InvoiceType")]
        [Column("AccountTypeCode")]
	    public string AccountTypeCode { get; set; }
	      
        public virtual InvoiceType InvoiceType { get; set; }
        [Column("IsPreference")]
	    public bool IsPreference { get; set; }
        [ForeignKey("TradeAgreement")]
        [Column("PreferenceDocumentTypeCode")]
	    public string PreferenceDocumentTypeCode { get; set; }
	      
        public virtual TradeAgreement TradeAgreement { get; set; }
        [ForeignKey("PaymentType")]
        [Column("PaymentTypeCode")]
	    public string PaymentTypeCode { get; set; }
	      
        public virtual PaymentType PaymentType { get; set; }
        [ForeignKey("CurrencyType")]
        [Column("InvoiceCurrencyTypeCode")]
	    public string InvoiceCurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType CurrencyType { get; set; }
        [Column("InvoiceAmount")]
	    public decimal? InvoiceAmount { get; set; }
        [ForeignKey("PayedCurrencyType")]
        [Column("ActualPayedCurrencyTypeCode")]
	    public string ActualPayedCurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType PayedCurrencyType { get; set; }
        [Column("ActualPayedAmount")]
	    public decimal? ActualPayedAmount { get; set; }
        [ForeignKey("Vendor")]
        [Column("VendorId")]
	    public string VendorId { get; set; }
	      
        public virtual CustomsVendor Vendor { get; set; }
        [ForeignKey("TermsOfSaleType")]
        [Column("IncotermCode")]
	    public string IncotermCode { get; set; }
	      
        public virtual TermsOfSaleType TermsOfSaleType { get; set; }
        [ForeignKey("IssueCountry")]
        [Column("IssueCountryCode")]
	    public string IssueCountryCode { get; set; }
	      
        public virtual CustomsCountry IssueCountry { get; set; }
        [ForeignKey("PaymentTerm")]
        [Column("PaymentTermsCode")]
	    public string PaymentTermsCode { get; set; }
	      
        public virtual CustomsPaymentTerm PaymentTerm { get; set; }
        [Column("TotalFreightInFreightCurrency")]
	    public decimal? TotalFreightInFreightCurrency { get; set; }
        [Column("TotalFreightInNIS")]
	    public decimal? TotalFreightInNIS { get; set; }
        [Column("ExchangeRate")]
	    public decimal? ExchangeRate { get; set; }
        [ForeignKey("InsruanceCurrencyType")]
        [Column("InsruanceCurrencyTypeCode")]
	    public string InsruanceCurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType InsruanceCurrencyType { get; set; }
        [Column("InsuranceAmount")]
	    public decimal? InsuranceAmount { get; set; }
        [Column("InsruancePercentage")]
	    public decimal? InsruancePercentage { get; set; }
        [ForeignKey("FreightCurrencyType")]
        [Column("FreightCurrencyTypeCode")]
	    public string FreightCurrencyTypeCode { get; set; }
	      
        public virtual CurrencyType FreightCurrencyType { get; set; }
        [Column("IsAccumalated")]
	    public bool IsAccumalated { get; set; }
        [ForeignKey("AccumalationState")]
        [Column("AccumalationStateCode")]
	    public string AccumalationStateCode { get; set; }
	      
        public virtual AccumalationState AccumalationState { get; set; }
        [Column("UnfInvoiceCounterKey")]
	    public string UnfInvoiceCounterKey { get; set; }
        [Column("VendorComissionPercentage")]
	    public decimal? VendorComissionPercentage { get; set; }
        [Column("InvoiceAmountInUSD")]
	    public decimal? InvoiceAmountInUSD { get; set; }
        [Column("ChangeInSupplierInvoice")]
	    public string ChangeInSupplierInvoice { get; set; }
        [Column("BuyerName")]
	    public string BuyerName { get; set; }
        [Column("BuyerAddress")]
	    public string BuyerAddress { get; set; }
        [ForeignKey("BuyerCountry")]
        [Column("BuyerCountryCode")]
	    public string BuyerCountryCode { get; set; }
	      
        public virtual CustomsCountry BuyerCountry { get; set; }
        [ForeignKey("BuyerRole")]
        [Column("BuyerRoleCode")]
	    public string BuyerRoleCode { get; set; }
	      
        public virtual BuyerRoleType BuyerRole { get; set; }
        [ForeignKey("PartyRelationship")]
        [Column("PartyRelationshipCode")]
	    public string PartyRelationshipCode { get; set; }
	      
        public virtual PartyRelationshipType PartyRelationship { get; set; }
        [Column("ItemFOBAmountForeign")]
	    public decimal? ItemFOBAmountForeign { get; set; }
        [Column("ItemFOBAmountNIS")]
	    public decimal? ItemFOBAmountNIS { get; set; }
    }
}
	 