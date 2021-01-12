using System;
using System.Collections.Generic;

namespace RestClientApplication
{
    public class ARInvoice
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string AutoCreditByARInvoiceId { get; set; }
        public string AutoCreditByARInvoiceNumber { get; set; }

        public string AutoCreditedByARInvoiceId { get; set; }
        public string AutoCreditedByARInvoiceNumber { get; set; }

        public string InvoiceNumber { get; set; }
        public string ARInvoiceTypeCode { get; set; }
        
        public string MainEntityStatus { get; set; }

        public string BillToId { get; set; }
        public string BillToName { get; set; }
        public string BillToType { get; set; }
        public string BillToPartnerTypeId { get; set; }
        public string BillToCode { get; set; }
        public string BillToAccountManagerName { get; set; }

        public bool BillToIsCreditLimitEnabled { get; set; }
        public double? BillToCreditLimitAmount { get; set; }
        public double? BillToCreditLimitOpenBalance { get; set; }
        public double? BillToCreditLimitWarningPercentage { get; set; }
        public double? BillToCreditLimitActualAmount { get; set; }
        public double? BillToCreditLimitActualBalance { get; set; }
        public bool BillToBlockNewInvoiceCreation { get; set; }
        public bool HasCreditLimitOverrideFeature { get; set; }

        public string BillToAddressId { get; set; }
        public string VatNumber { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string PrintByUserName { get; set; }
        public string IssuedByUserId { get; set; }

        public string InvoiceCurrencyId { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public bool IsAutoCredit { get; set; }
        public bool IsCancelled { get; set; }
        public string CancelledByARInvoiceId { get; set; }
        public string InternalNotes { get; set; }
        public string PrintNotes { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        
        public string PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PrepaidCollectId { get; set; }
        public string DraftNumber { get; set; }
        public bool IsInvoiceNumberManuallySet { get; set; }
        public bool Sent { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string EventNote { get; set; }
        public string SearchFields { get; set; }

        public string MainEntityId { get; set; }
        public string MainEntityReference { get; set; }
        public double? AmountDue { get; set; }

        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string Description { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public DateTime? ExpectedPaymentDate { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string BranchId { get; set; }
        public bool IsPrinted { get; set; }
        public string ConnectedEntityReferences { get; set; }


        public string DebitAccount { get; set; }       
        public string ReportUrl { get; set; }        
        public string PaymentTermExternalId { get; set; }
        public bool IsConstituentInvoice { get; set; }
        public bool IsConsolidationInvoice { get; set; }
        public string ConsolidationInvoiceId { get; set; }
        public string ConsolidationInvoiceNumber { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public string AccountingExternalCode { get; set; }
        public string AccountingExternalName { get; set; }
        public bool ReadyForTransfer { get; set; }
        public bool IsTransferStatusSetManually { get; set; }
        public bool IsBillToAllowConsolidation { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public string ApprovedByUserName { get; set; }

        public string CreditedByARInvoiceId { get; set; }

        public string ExternalAccountingEntityId { get; set; }

        public string CustomerRef { get; set; }

        public DateTime? OperationalDate { get; set; }

        public DateTime? DateForVATInterest { get; set; }
        public bool SplitJournalByCurrency { get; set; }
        public bool IsExternalEntity { get; set; }
        public string SATXML { get; set; }

        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }

        public string SATTransferStatusCode { get; set; }
        public string SATInvoiceStatusCode { get; set; }
        public string SATTransferStatusName { get; set; }
        public string SATInvoiceStatusName { get; set; }
        public bool Intercompany { get; set; }

        public string BankAccountLiteId { get; set; }

        private List<ARInvoiceLinePM> invoiceLines;
        
        //Dummy Fields
        public bool SetVoided { get; set; }
        public bool SetAsSent { get; set; }
        public bool SetApproved { get; set; }
        public bool SetReTransfer { get; set; }
        public bool SetCancelDraft { get; set; }
        public bool IsExternalAPI { get; set; }

        // Full Accounting Fields 
        public string JournalId { get; set; }
        public string JournalNumber {get;set;}
        public bool IsGeneralInvoice { get; set; }

        public string SATPaymentMethodCode { get; set; }
        public string TransmissionError { get; set; }
        public bool IsCustomsChargesOnly { get; set; }
        public string RelatedInvoice { get; set; }
        public string SATAdditionalFieldsXML { get; set; }
        public string MetodoPagoCode { get; set; }
        public string UsoCFDICode { get; set; }
        public bool IsDraft { get; set; }

        public bool IsMultiCurrency { get; set; }

        public Decimal? TotalAmountForTaxReport { get; set; }
        public Decimal? TotaVatableAmountForTaxReport { get; set; }
        public Decimal? TotalVAT { get; set; }


    }
}