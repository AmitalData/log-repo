using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class FullAccountingSettingList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string DeductionFileNumber  { get; set; }
       [DataMember]
       public string ConsolidationVAT  { get; set; }
       [DataMember]
       public string DefaultVATTypeId  { get; set; }
       [DataMember]
       public string VATInputsGLAccountId  { get; set; }
       [DataMember]
       public string AutomaticReconcileMethodId  { get; set; }
       [DataMember]
       public string ExchangeRateDiffGLAccountId  { get; set; }
       [DataMember]
       public string RevenueExpenseGLAccountId  { get; set; }

       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string VATOutputGLAccountId  { get; set; }
       [DataMember]
       public string CustomerControlAccountId  { get; set; }
       [DataMember]
       public string CustomerControlAccountName  { get; set; }
       [DataMember]
       public string CustomerControlAccountNumber  { get; set; }
       [DataMember]
       public string VendorControlAccountId  { get; set; }
       [DataMember]
       public string VendorControlAccountName  { get; set; }
       [DataMember]
       public string VendorControlAccountNumber  { get; set; }
       [DataMember]
       public string FileControlAccountId  { get; set; }
       [DataMember]
       public string FileControlAccountName  { get; set; }
       [DataMember]
       public string FileControlAccountNumber  { get; set; }
       [DataMember]
       public string OceanExportJobControlAccountId  { get; set; }
       [DataMember]
       public string OceanExportJobControlAccountName  { get; set; }
       [DataMember]
       public string OceanExportJobControlAccountNumber  { get; set; }
       [DataMember]
       public string AirExportJobControlAccountId  { get; set; }
       [DataMember]
       public string AirExportJobControlAccountName  { get; set; }
       [DataMember]
       public string AirExportJobControlAccountNumber  { get; set; }
       [DataMember]
       public string OceanImportJobControlAccountId  { get; set; }
       [DataMember]
       public string OceanImportJobControlAccountName  { get; set; }
       [DataMember]
       public string OceanImportJobControlAccountNumber  { get; set; }
       [DataMember]
       public string AirImportJobControlAccountId  { get; set; }
       [DataMember]
       public string AirImportJobControlAccountName  { get; set; }
       [DataMember]
       public string AirImportJobControlAccountNumber  { get; set; }
       [DataMember]
       public string ExternalReconciliationDefault  { get; set; }
       [DataMember]
       public string TaxWithholdingGLAccountId  { get; set; }
       [DataMember]
       public decimal? DefaultTaxWithholdPercentage  { get; set; }
       [DataMember]
       public string CustomsGLAccountId  { get; set; }
       [DataMember]
       public string DefaultDifferencesGLAccountId  { get; set; }
       [DataMember]
       public string DefaultExternalDiffGLAccountId  { get; set; }
       [DataMember]
       public string SoftwareVersion  { get; set; }
       [DataMember]
       public bool IsPaymentChequesActivated  { get; set; }
       [DataMember]
       public int? GLAccounterCounterLength  { get; set; }
       [DataMember]
       public string PaymentChequesLogoId  { get; set; }
       [DataMember]
       public int? NumberOfAgingMonths  { get; set; }
       [DataMember]
       public bool AllowMultiRatesInInvoiceLines  { get; set; }
       [DataMember]
       public int? NumberofPeriods  { get; set; }
       [DataMember]
       public string FirstPeriodsMonths  { get; set; }
       [DataMember]
       public string SecondPeriodsMonths  { get; set; }
       [DataMember]
       public string ThirdsPeriodsMonths  { get; set; }
       [DataMember]
       public bool IsSecurityLevelActivated  { get; set; }
       [DataMember]
       public bool CreateRevaluationJournal  { get; set; }
       [DataMember]
       public string TaxInstitutionGLAccountId  { get; set; }
       [DataMember]
       public string HSM  { get; set; }
       [DataMember]
       public string HSMtoken  { get; set; }
       [DataMember]
       public string HSMaddress  { get; set; }
       [DataMember]
       public bool AllowEditingExchangeRate  { get; set; }
   }

}
	 