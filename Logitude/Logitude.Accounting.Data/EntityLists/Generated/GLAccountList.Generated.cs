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
   public partial class GLAccountList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string InternalNumber  { get; set; }
       [DataMember]
       public string AccountTypeCode  { get; set; }
       [DataMember]
       public string DisplayNumber  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
       [DataMember]
       public string EnglishName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool? IsMultiCurrency  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public string PaymentTerms { get; set; }
       [DataMember]
       public string RevenueExpenseType  { get; set; }
       [DataMember]
       public bool? IsControlAccount  { get; set; }
       [DataMember]
       public string ChartOfAccountsId  { get; set; }
       [DataMember]
       public bool? Inactive  { get; set; }
       [DataMember]
       public string AccountTypeName  { get; set; }
       [DataMember]
       public string CurrencyName  { get; set; }
       [DataMember]
       public string RevenueExpenseName  { get; set; }
       [DataMember]
       public string ChartOfAccountsName  { get; set; }
       [DataMember]
       public string ChartOfAccountsTypeCode  { get; set; }
       [DataMember]
       public string ChartOfAccountsTypeName  { get; set; }
       [DataMember]
       public string CurrencyCode  { get; set; }
       [DataMember]
       public string ReconcileMethodCode  { get; set; }
       [DataMember]
       public string ReconcileMethodName  { get; set; }
       [DataMember]
       public string ControlAccountId  { get; set; }
       [DataMember]
       public string ControlAccountName  { get; set; }
       [DataMember]
       public string ControlAccountNumber  { get; set; }
       [DataMember]
       public string ActiveStatusName  { get; set; }
       [DataMember]
       public string AutomaticReconcileId  { get; set; }
       [DataMember]
       public string AutomaticReconcileName  { get; set; }
       [DataMember]
       public string PreviousEnglishName  { get; set; }
       [DataMember]
       public DateTime? PreviousEnglishNameChangeDate  { get; set; }
       [DataMember]
       public string PreviousLocalName  { get; set; }
       [DataMember]
       public DateTime? PreviousLocalNameChangeDate  { get; set; }
       [DataMember]
       public string PreviousNumber  { get; set; }
       [DataMember]
       public DateTime? PreviousNumberChangeDate  { get; set; }
       [DataMember]
       public string PreviousChartOfAccountsId  { get; set; }
       [DataMember]
       public DateTime? PreviousChartOfAccountsChangeDate  { get; set; }
       [DataMember]
       public string CustomerGLAccountId  { get; set; }
       [DataMember]
       public string CustomerGLAccountName  { get; set; }
       [DataMember]
       public string CustomerGLAccountNumber  { get; set; }
       [DataMember]
       public decimal? BalanceInLocalCurrency  { get; set; }
       [DataMember]
       public bool? RevaluationEnabled  { get; set; }
       [DataMember]
       public string ParentAccountId  { get; set; }
       [DataMember]
       public string ParentAccountName  { get; set; }
       [DataMember]
       public string ParentAccountNumber  { get; set; }
       [DataMember]
       public string Category1Id  { get; set; }
       [DataMember]
       public string Category1Name  { get; set; }
       [DataMember]
       public string Category2Id  { get; set; }
       [DataMember]
       public string Category2Name  { get; set; }
       [DataMember]
       public string Category3Id  { get; set; }
       [DataMember]
       public string Category3Name  { get; set; }
       [DataMember]
       public string Category4Id  { get; set; }
       [DataMember]
       public string Category4Name  { get; set; }
       [DataMember]
       public string Category5Id  { get; set; }
       [DataMember]
       public string Category5Name  { get; set; }
       [DataMember]
       public bool? IsVATExempt  { get; set; }
       [DataMember]
       public DateTime? LastActivityDate  { get; set; }
       [DataMember]
       public string LastActivityTypeName  { get; set; }
       [DataMember]
       public string LastActivityByUserName  { get; set; }
       [DataMember]
       public string VatNumber  { get; set; }
       [DataMember]
       public string PaymentTermId  { get; set; }
       [DataMember]
       public string SalesmanUserId  { get; set; }
       [DataMember]
       public string NewGLAccountCardId  { get; set; }
       [DataMember]
       public decimal? LocalBalanceInDue  { get; set; }
       [DataMember]
       public DateTime? NextDueDate  { get; set; }
       [DataMember]
       public string CurrencySign  { get; set; }
       [DataMember]
       public string DeductionFileTypeId  { get; set; }
       [DataMember]
       public string DeductionFileNumber  { get; set; }
       [DataMember]
       public string AssessingOfficeCode  { get; set; }
       [DataMember]
       public string Occupation  { get; set; }
       [DataMember]
       public string DeductionTypeId  { get; set; }
       [DataMember]
       public string ConsolidationVat  { get; set; }
       [DataMember]
       public bool IsEquipmentVendor  { get; set; }
       [DataMember]
       public bool ExcludeFromDeductionReport  { get; set; }
       [DataMember]
       public string Parent  { get; set; }
       [DataMember]
       public string DeductionTypeName  { get; set; }
       [DataMember]
       public string DeductionFileTypeCode  { get; set; }
       [DataMember]
       public string DeductionFileTypeName  { get; set; }
       [DataMember]
       public string AssessingOfficeName  { get; set; }
       [DataMember]
       public string DeductionTypeEnglishName  { get; set; }
       [DataMember]
       public decimal? TotalOpenChequesInLocalCur  { get; set; }
       [DataMember]
       public string AutomaticReconcileLocalName  { get; set; }
       [DataMember]
       public string ReconcileMethodLocalName  { get; set; }
       [DataMember]
       public decimal? TotFutureOpenChequesInLocalCur  { get; set; }
       [DataMember]
       public string CardId  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string CreatedByLocalName  { get; set; }
       [DataMember]
       public string UpdatedByLocalName  { get; set; }
       [DataMember]
       public bool AllowEditChequePayToName  { get; set; }
       [DataMember]
       public bool ActiveForInterest  { get; set; }
       [DataMember]
       public DateTime? InterestCalculationStartDate  { get; set; }
       [DataMember]
       public bool ActiveForInterestCreditInvoice  { get; set; }
       [DataMember]
       public decimal? InterestCreditLimit  { get; set; }
        [DataMember]
        public decimal? InterestOpenBalance { get; set; }
       [DataMember]
       public string NameForPrintingCheques  { get; set; }
       [DataMember]
       public bool Smallcashbook  { get; set; }
       [DataMember]
       public int? MinimumInterestInvoiceBilling  { get; set; }
       [DataMember]
       public string SalesmanName  { get; set; }
       [DataMember]
       public string CollectorName  { get; set; }
       [DataMember]
       public string SplitCurrencyAccount  { get; set; }
       [DataMember]
       public string ParentName  { get; set; }
       [DataMember]
       public string ParentCurrencyId  { get; set; }
       [DataMember]
       public bool ReportingAsAnotherDocument  { get; set; }
       [DataMember]
       public decimal? CreditAllotmentPercentage  { get; set; }
       [DataMember]
       public string Category1LocalName  { get; set; }
       [DataMember]
       public string Category2LocalName  { get; set; }
       [DataMember]
       public string Category3LocalName  { get; set; }
       [DataMember]
       public string Category4LocalName  { get; set; }
       [DataMember]
       public string Category5LocalName  { get; set; }
       [DataMember]
       public string RelatedGLAccount  { get; set; }
       [DataMember]
       public string ChartOfAccountsEnglishName  { get; set; }
       [DataMember]
       public string ChartOfAccountsTypeEnglishName  { get; set; }
       [DataMember]
       public string ChartOfAccountsTypeLocalName  { get; set; }
       [DataMember]
       public string ChartOfAccountsLocalName  { get; set; }
       [DataMember]
       public string CardsDataId  { get; set; }
       [DataMember]
       public string PaymentTermName  { get; set; }
       [DataMember]
       public decimal? Period0  { get; set; }
       [DataMember]
       public decimal? Period1  { get; set; }
       [DataMember]
       public decimal? Period2  { get; set; }
       [DataMember]
       public decimal? Period3  { get; set; }
       [DataMember]
       public decimal? Period4  { get; set; }
       [DataMember]
       public decimal? Period5  { get; set; }
       [DataMember]
       public decimal? PeriodPast  { get; set; }
       [DataMember]
       public decimal? PeriodFuture  { get; set; }
       [DataMember]
       public int? TotalOpenTransactions  { get; set; }
       [DataMember]
       public string LastReconciledBy  { get; set; }
       [DataMember]
       public string CardCollectorId { get; set; }
       [DataMember]
       public DateTime? LastReconcileDate  { get; set; }
       [DataMember]
       public double? CreditLimit  { get; set; }
       [DataMember]
       public string PaymentTerm  { get; set; }
       [DataMember]
       public decimal? TotalOpenShipments  { get; set; }
       [DataMember]
       public string Phone  { get; set; }
       [DataMember]
       public string Salesman  { get; set; }
       [DataMember]
       public string Collector  { get; set; }
       [DataMember]
       public DateTime? FollowupDate  { get; set; }
       [DataMember]
       public string FollowupNotes  { get; set; }
       [DataMember]
       public decimal? CalculatedAgingPeriod1  { get; set; }
       [DataMember]
       public decimal? CalculatedAgingPeriod2  { get; set; }
       [DataMember]
       public decimal? CalculatedAgingPeriod3  { get; set; }
       [DataMember]
       public string FirstPeriodsMonths  { get; set; }
       [DataMember]
       public string SecondPeriodsMonths  { get; set; }
       [DataMember]
       public string ThirdPeriodsMonths  { get; set; }
       [DataMember]
       public double? InsuredCreditLimit  { get; set; }
       [DataMember]
       public decimal? PostponedChequesCommission  { get; set; }
       [DataMember]
       public decimal? BalanceInForeignCurrency  { get; set; }
       [DataMember]
       public decimal? ForeignBalanceInDue  { get; set; }

        [DataMember]
        public bool IsSecurityLevelsEnabled { get; set; }
        [DataMember]
        public int? ChartOfAccountSecurityLevel { get; set; }
        [DataMember]
        public bool Access { get; set; }
        [DataMember]
        public bool IsMainGLAccount { get; set; }
        [DataMember]
        public bool IsSplitGLAccout { get; set; }
        [DataMember]
        public decimal? Obligo { get; set; }
        [DataMember]
        public double? CreditUsed { get; set; }
        [DataMember]
        public double? InsuredCreditPercentage { get; set; }
        [DataMember]
        public string DateFormat { get; set; }
        [DataMember]
       public string ContactId  { get; set; }
        [DataMember]
       public string ContactName  { get; set; }
       [DataMember]
       public string ContactEmail  { get; set; }
       [DataMember]
       public string ContactPhone  { get; set; }
    }

}
	 