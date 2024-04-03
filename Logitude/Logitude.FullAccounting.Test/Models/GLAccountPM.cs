
using System;
using System.Collections.Generic;


namespace Logitude.FullAccounting.Test.Models
{


    public class GLAccountPM
    {
        public int MyProperty { get; set; }

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InternalNumber { get; set; }
        public string AccountTypeCode { get; set; }
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string SearchFields { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string CurrencyId { get; set; }
        public string RevenueExpenseType { get; set; }
        public bool? IsControlAccount { get; set; }
        public string ChartOfAccountsId { get; set; }
        public bool? Inactive { get; set; }
        public string AccountTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string RevenueExpenseName { get; set; }
        public string ChartOfAccountsName { get; set; }
        public string ChartOfAccountsTypeCode { get; set; }
        public string ChartOfAccountsTypeName { get; set; }
        public string CurrencyCode { get; set; }
        public string ReconcileMethodCode { get; set; }
        public string ReconcileMethodName { get; set; }
        public string ControlAccountId { get; set; }
        public string ControlAccountName { get; set; }
        public string ControlAccountNumber { get; set; }
        public string ActiveStatusName { get; set; }
        public string OldCurrencyId { get; set; }
        public bool OldIsMultiCurrency { get; set; }
        public string AutomaticReconcileId { get; set; }
        public string AutomaticReconcileName { get; set; }
        public string PreviousEnglishName { get; set; }
        public DateTime? PreviousEnglishNameChangeDate { get; set; }
        public string PreviousLocalName { get; set; }
        public DateTime? PreviousLocalNameChangeDate { get; set; }
        public string PreviousNumber { get; set; }
        public DateTime? PreviousNumberChangeDate { get; set; }
        public string PreviousChartOfAccountsId { get; set; }
        public DateTime? PreviousChartOfAccountsChangeDate { get; set; }
        public string CustomerGLAccountId { get; set; }
        public string CustomerGLAccountName { get; set; }
        public string CustomerGLAccountNumber { get; set; }
        public decimal? BalanceInLocalCurrency { get; set; }
        public bool? RevaluationEnabled { get; set; }
        public string ParentAccountId { get; set; }
        public string ParentAccountName { get; set; }
        public string ParentAccountNumber { get; set; }
        public string CustomerGLAccountInternalNumber { get; set; }
        public string Category1Id { get; set; }
        public string Category1Name { get; set; }
        public string Category2Id { get; set; }
        public string Category2Name { get; set; }
        public string Category3Id { get; set; }
        public string Category3Name { get; set; }
        public string Category4Id { get; set; }
        public string Category4Name { get; set; }
        public string Category5Id { get; set; }
        public string Category5Name { get; set; }
        public bool? IsVATExempt { get; set; }
        public string ChartOfAccountsCode { get; set; }
        public string CustomerCode { get; set; }
        public string ParentAccountByCurrency { get; set; }
        public string VatNumber { get; set; }
        public string PaymentTermId { get; set; }
        public string CollectorId { get; set; }
        public string SalesmanUserId { get; set; }
        public string NewGLAccountCardId { get; set; }
        public decimal? LocalBalanceInDue { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string CurrencySign { get; set; }
        public string ConnectedItems { get; set; }
        public string Type { get; set; }
        public string DeductionFileTypeId { get; set; }
        public string DeductionFileNumber { get; set; }
        public string AssessingOfficeCode { get; set; }
        public string Occupation { get; set; }
        public string DeductionTypeId { get; set; }
        public string ConsolidationVat { get; set; }
        public string CardId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string UpdatedByLocalName { get; set; }
        public string CardCode { get; set; }
        public string PartnerTypeId { get; set; }
        public bool AllowEditChequePayToName { get; set; }
        public bool ActiveForInterest { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
        public bool ActiveForInterestCreditInvoice { get; set; }
       
        public decimal? InterestCreditLimit { get; set; }
        public string NameForPrintingCheques { get; set; }
        public bool Smallcashbook { get; set; }
        public int? MinimumInterestInvoiceBilling { get; set; }
        public bool IsSplitted { get; set; }
        public string SalesmanName { get; set; }
        public string CollectorName { get; set; }
        public string SplitCurrencyAccount { get; set; }
        public string ParentName { get; set; }

    }

}
