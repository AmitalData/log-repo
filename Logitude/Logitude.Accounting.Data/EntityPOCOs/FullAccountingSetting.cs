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
   
    public class FullAccountingSetting
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("DeductionFileNumber")]
	    public string DeductionFileNumber { get; set; }
        [Column("ConsolidationVAT")]
	    public string ConsolidationVAT { get; set; }
        [ForeignKey("DefaultVATType")]
        [Column("DefaultVATTypeId")]
	    public string DefaultVATTypeId { get; set; }
	      
        public virtual VatType DefaultVATType { get; set; }
        [ForeignKey("VATInputsGLAccont")]
        [Column("VATInputsGLAccountId")]
	    public string VATInputsGLAccountId { get; set; }
	      
        public virtual GLAccount VATInputsGLAccont { get; set; }
        [ForeignKey("AutomaticReconcileMethod")]
        [Column("AutomaticReconcileMethodId")]
	    public string AutomaticReconcileMethodId { get; set; }
	      
        public virtual AutomaticReconcileMethod AutomaticReconcileMethod { get; set; }
        [ForeignKey("ExchangeRateDiffGLAccount")]
        [Column("ExchangeRateDiffGLAccountId")]
	    public string ExchangeRateDiffGLAccountId { get; set; }
	      
        public virtual GLAccount ExchangeRateDiffGLAccount { get; set; }
        [ForeignKey("RevenueExpenseGLAcount")]
        [Column("RevenueExpenseGLAccountId")]
	    public string RevenueExpenseGLAccountId { get; set; }
	      
        public virtual GLAccount RevenueExpenseGLAcount { get; set; }
     [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [ForeignKey("VATOutputGLAccount")]
        [Column("VATOutputGLAccountId")]
	    public string VATOutputGLAccountId { get; set; }
	      
        public virtual GLAccount VATOutputGLAccount { get; set; }
        [ForeignKey("CustomerControlAccount")]
        [Column("CustomerControlAccountId")]
	    public string CustomerControlAccountId { get; set; }
	      
        public virtual GLAccount CustomerControlAccount { get; set; }
        [ForeignKey("VendorControlAccount")]
        [Column("VendorControlAccountId")]
	    public string VendorControlAccountId { get; set; }
	      
        public virtual GLAccount VendorControlAccount { get; set; }
        [ForeignKey("FileControlAccount")]
        [Column("FileControlAccountId")]
	    public string FileControlAccountId { get; set; }
	      
        public virtual GLAccount FileControlAccount { get; set; }
        [ForeignKey("OceanExportJobControlAccount")]
        [Column("OceanExportJobControlAccountId")]
	    public string OceanExportJobControlAccountId { get; set; }
	      
        public virtual GLAccount OceanExportJobControlAccount { get; set; }
        [ForeignKey("AirExportJobControlAccount")]
        [Column("AirExportJobControlAccountId")]
	    public string AirExportJobControlAccountId { get; set; }
	      
        public virtual GLAccount AirExportJobControlAccount { get; set; }
        [ForeignKey("OceanImportJobControlAccount")]
        [Column("OceanImportJobControlAccountId")]
	    public string OceanImportJobControlAccountId { get; set; }
	      
        public virtual GLAccount OceanImportJobControlAccount { get; set; }
        [ForeignKey("AirImportJobControlAccount")]
        [Column("AirImportJobControlAccountId")]
	    public string AirImportJobControlAccountId { get; set; }
	      
        public virtual GLAccount AirImportJobControlAccount { get; set; }
        [ForeignKey("AutomaticExternalRconcilMthod")]
        [Column("ExternalReconciliationDefault")]
	    public string ExternalReconciliationDefault { get; set; }
	      
        public virtual AutomaticExternalRconcilMthod AutomaticExternalRconcilMthod { get; set; }
        [ForeignKey("TaxWithholdingGLAccount")]
        [Column("TaxWithholdingGLAccountId")]
	    public string TaxWithholdingGLAccountId { get; set; }
	      
        public virtual GLAccount TaxWithholdingGLAccount { get; set; }
        [Column("DefaultTaxWithholdPercentage")]
	    public decimal? DefaultTaxWithholdPercentage { get; set; }
        [ForeignKey("CustomsGLAccount")]
        [Column("CustomsGLAccountId")]
	    public string CustomsGLAccountId { get; set; }
	      
        public virtual GLAccount CustomsGLAccount { get; set; }
        [ForeignKey("DefaultDifferencesGLAccount")]
        [Column("DefaultDifferencesGLAccountId")]
	    public string DefaultDifferencesGLAccountId { get; set; }
	      
        public virtual GLAccount DefaultDifferencesGLAccount { get; set; }
        [ForeignKey("DefaultExternalDiffGLAccount")]
        [Column("DefaultExternalDiffGLAccountId")]
	    public string DefaultExternalDiffGLAccountId { get; set; }
	      
        public virtual GLAccount DefaultExternalDiffGLAccount { get; set; }
        [Column("SoftwareVersion")]
	    public string SoftwareVersion { get; set; }
        [Column("IsPaymentChequesActivated")]
	    public bool IsPaymentChequesActivated { get; set; }
        [Column("GLAccounterCounterLength")]
	    public int? GLAccounterCounterLength { get; set; }
        [Column("PaymentChequesLogoId")]
	    public string PaymentChequesLogoId { get; set; }
        [Column("NumberOfAgingMonths")]
	    public int? NumberOfAgingMonths { get; set; }
        [Column("AllowMultiRatesInInvoiceLines")]
	    public bool AllowMultiRatesInInvoiceLines { get; set; }
        [Column("NumberofPeriods")]
	    public int? NumberofPeriods { get; set; }
        [Column("FirstPeriodsMonths")]
	    public string FirstPeriodsMonths { get; set; }
        [Column("SecondPeriodsMonths")]
	    public string SecondPeriodsMonths { get; set; }
        [Column("ThirdsPeriodsMonths")]
	    public string ThirdsPeriodsMonths { get; set; }
        [Column("IsSecurityLevelActivated")]
	    public bool IsSecurityLevelActivated { get; set; }
        [Column("VATreportEveryTwoMonths")]
	    public bool VATreportEveryTwoMonths { get; set; }
        [Column("CreateRevaluationJournal")]
	    public bool CreateRevaluationJournal { get; set; }
        [ForeignKey("TaxInstitutionGLAccount")]
        [Column("TaxInstitutionGLAccountId")]
	    public string TaxInstitutionGLAccountId { get; set; }
	      
        public virtual GLAccount TaxInstitutionGLAccount { get; set; }
        [Column("HSM")]
	    public int? HSM { get; set; }
        [Column("HSMtoken")]
	    public string HSMtoken { get; set; }
        [Column("HSMaddress")]
	    public string HSMaddress { get; set; }
        [Column("AllowEditingExchangeRate")]
	    public bool AllowEditingExchangeRate { get; set; }
        [Column("AmountForConfirmationNumber")]
	    public int? AmountForConfirmationNumber { get; set; }
        [Column("NumberingByChartOfAccount")]
	    public bool NumberingByChartOfAccount { get; set; }
        [Column("OppositeAccountNumber")]
	    public bool OppositeAccountNumber { get; set; }
    }
}
	 