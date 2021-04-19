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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class GLAccount
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("InternalNumber")]
	    public string InternalNumber { get; set; }
        [ForeignKey("GLAccountType")]
        [Column("AccountTypeCode")]
	    public string AccountTypeCode { get; set; }
	      
        public virtual GLAccountType GLAccountType { get; set; }
        [Column("DisplayNumber")]
	    public string DisplayNumber { get; set; }
        [Column("LocalName")]
	    public string LocalName { get; set; }
        [Column("EnglishName")]
	    public string EnglishName { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("IsMultiCurrency")]
	    public bool? IsMultiCurrency { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [ForeignKey("RevenueExpense")]
        [Column("RevenueExpenseType")]
	    public string RevenueExpenseType { get; set; }
	      
        public virtual RevenueExpenseType RevenueExpense { get; set; }
        [Column("IsControlAccount")]
	    public bool? IsControlAccount { get; set; }
        [ForeignKey("ChartOfAccount")]
        [Column("ChartOfAccountsId")]
	    public string ChartOfAccountsId { get; set; }
	      
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        [Column("Inactive")]
	    public bool? Inactive { get; set; }
        [ForeignKey("ChartOfAccountsType")]
        [Column("ChartOfAccountsTypeCode")]
	    public string ChartOfAccountsTypeCode { get; set; }
	      
        public virtual ChartOfAccountsType ChartOfAccountsType { get; set; }
        [ForeignKey("ReconcileMethod")]
        [Column("ReconcileMethodCode")]
	    public string ReconcileMethodCode { get; set; }
	      
        public virtual ReconcileMethod ReconcileMethod { get; set; }
        [ForeignKey("ControlAccount")]
        [Column("ControlAccountId")]
	    public string ControlAccountId { get; set; }
	      
        public virtual GLAccount ControlAccount { get; set; }
        [ForeignKey("AutomaticReconcile")]
        [Column("AutomaticReconcileId")]
	    public string AutomaticReconcileId { get; set; }
	      
        public virtual AutomaticReconcileMethod AutomaticReconcile { get; set; }
        [Column("PreviousEnglishName")]
	    public string PreviousEnglishName { get; set; }
        [Column("PreviousEnglishNameChangeDate")]
	    public DateTime? PreviousEnglishNameChangeDate { get; set; }
        [Column("PreviousLocalName")]
	    public string PreviousLocalName { get; set; }
        [Column("PreviousLocalNameChangeDate")]
	    public DateTime? PreviousLocalNameChangeDate { get; set; }
        [Column("PreviousNumber")]
	    public string PreviousNumber { get; set; }
        [Column("PreviousNumberChangeDate")]
	    public DateTime? PreviousNumberChangeDate { get; set; }
        [ForeignKey("PreviousChartOfAccount")]
        [Column("PreviousChartOfAccountsId")]
	    public string PreviousChartOfAccountsId { get; set; }
	      
        public virtual ChartOfAccount PreviousChartOfAccount { get; set; }
     
	    public DateTime? PreviousChartOfAccountsChangeDate { get; set; }
        [Column("CustomerGLAccountId")]
	    public string CustomerGLAccountId { get; set; }
        [Column("RevaluationEnabled")]
	    public bool? RevaluationEnabled { get; set; }
        [Column("ParentAccountId")]
	    public string ParentAccountId { get; set; }
        [ForeignKey("Category1")]
        [Column("Category1Id")]
	    public string Category1Id { get; set; }
	      
        public virtual Category1 Category1 { get; set; }
        [ForeignKey("Category2")]
        [Column("Category2Id")]
	    public string Category2Id { get; set; }
	      
        public virtual Category2 Category2 { get; set; }
        [ForeignKey("Category3")]
        [Column("Category3Id")]
	    public string Category3Id { get; set; }
	      
        public virtual Category3 Category3 { get; set; }
        [ForeignKey("Category4")]
        [Column("Category4Id")]
	    public string Category4Id { get; set; }
	      
        public virtual Category4 Category4 { get; set; }
        [ForeignKey("Category5")]
        [Column("Category5Id")]
	    public string Category5Id { get; set; }
	      
        public virtual Category5 Category5 { get; set; }
        [Column("IsVATExempt")]
	    public bool? IsVATExempt { get; set; }
        [ForeignKey("WithholdingTaxDeductionType")]
        [Column("DeductionFileTypeId")]
	    public string DeductionFileTypeId { get; set; }
	      
        public virtual WithholdingTaxDeductionType WithholdingTaxDeductionType { get; set; }
        [Column("DeductionFileNumber")]
	    public string DeductionFileNumber { get; set; }
        [ForeignKey("TaxWithholdingAssessOffice")]
        [Column("AssessingOfficeCode")]
	    public string AssessingOfficeCode { get; set; }
	      
        public virtual TaxWithholdingAssessOffice TaxWithholdingAssessOffice { get; set; }
        [Column("Occupation")]
	    public string Occupation { get; set; }
        [ForeignKey("AccountingCompanyType")]
        [Column("DeductionTypeId")]
	    public string DeductionTypeId { get; set; }
	      
        public virtual AccountingCompanyType AccountingCompanyType { get; set; }
        [Column("ConsolidationVat")]
	    public string ConsolidationVat { get; set; }
        [Column("IsEquipmentVendor")]
	    public bool IsEquipmentVendor { get; set; }
        [Column("ExcludeFromDeductionReport")]
	    public bool ExcludeFromDeductionReport { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [Column("AllowEditChequePayToName")]
	    public bool AllowEditChequePayToName { get; set; }
        [Column("ActiveForInterest")]
	    public bool ActiveForInterest { get; set; }
        [Column("InterestCalculationStartDate")]
	    public DateTime? InterestCalculationStartDate { get; set; }
        [Column("ActiveForInterestCreditInvoice")]
	    public bool ActiveForInterestCreditInvoice { get; set; }
        [Column("InterestCreditLimit")]
	    public decimal? InterestCreditLimit { get; set; }
        [Column("NameForPrintingCheques")]
	    public string NameForPrintingCheques { get; set; }
        [Column("Smallcashbook")]
	    public bool Smallcashbook { get; set; }
        [Column("MinimumInterestInvoiceBilling")]
	    public int? MinimumInterestInvoiceBilling { get; set; }
        [Column("ReportingAsAnotherDocument")]
	    public bool ReportingAsAnotherDocument { get; set; }
        [Column("CreditAllotmentPercentage")]
	    public decimal? CreditAllotmentPercentage { get; set; }
    }
}
	 