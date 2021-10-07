using System;
namespace Logitude.FullAccounting.Test.Models
{

	public class FullAccountingSettingPM 
   {

       public int Tenant { get; set; }
       public string DeductionFileNumber { get; set; }
		public string ConsolidationVAT { get; set; }
		public string DefaultVATTypeId { get; set; }
		public string VATInputsGLAccountId { get; set; }
		public string AutomaticReconcileMethodId { get; set; }
		public string ExchangeRateDiffGLAccountId { get; set; }
		public string RevenueExpenseGLAccountId { get; set; }
		public string Id { get; set; }
		public string VATOutputGLAccountId { get; set; }
		public string CustomerControlAccountId { get; set; }
		public string CustomerControlAccountName { get; set; }
		public string CustomerControlAccountNumber { get; set; }
		public string VendorControlAccountId { get; set; }
		public string VendorControlAccountName { get; set; }
		public string VendorControlAccountNumber { get; set; }
		public string FileControlAccountId { get; set; }
		public string FileControlAccountName { get; set; }
		public string FileControlAccountNumber { get; set; }
		public string OceanExportJobControlAccountId { get; set; }
		public string OceanExportJobControlAccountName { get; set; }
		public string OceanExportJobControlAccountNumber { get; set; }
		public string AirExportJobControlAccountId { get; set; }
		public string AirExportJobControlAccountName { get; set; }
		public string AirExportJobControlAccountNumber { get; set; }
		public string OceanImportJobControlAccountId { get; set; }
		public string OceanImportJobControlAccountName { get; set; }
		public string OceanImportJobControlAccountNumber { get; set; }
		public string AirImportJobControlAccountId { get; set; }
		public string AirImportJobControlAccountName { get; set; }
		public string AirImportJobControlAccountNumber { get; set; }
		public string TenantPaymentTermId { get; set; }
		public DateTime? AccountingActivationDate { get; set; }
		public bool AccountingActivated { get; set; }
		public string ExternalReconciliationDefault { get; set; }
		public string TaxWithholdingGLAccountId { get; set; }
		public decimal? DefaultTaxWithholdPercentage { get; set; }
		public string CustomsGLAccountId { get; set; }
		public string DefaultDifferencesGLAccountId { get; set; }
		public string DefaultExternalDiffGLAccountId { get; set; }
		public string SoftwareVersion { get; set; }
		public bool IsPaymentChequesActivated { get; set; }
		public int? GLAccounterCounterLength { get; set; }
		public string PaymentChequesLogoId { get; set; }
		public int? NumberOfAgingMonths { get; set; }
		public bool AllowMultiRatesInInvoiceLines { get; set; }
		public int? NumberofPeriods { get; set; }
		public string FirstPeriodsMonths { get; set; }
		public string SecondPeriodsMonths { get; set; }
		public string ThirdsPeriodsMonths { get; set; }
		public bool IsSecurityLevelActivated { get; set; }
		public bool VATreportEveryTwoMonths { get; set; }
	}
   
}
	 