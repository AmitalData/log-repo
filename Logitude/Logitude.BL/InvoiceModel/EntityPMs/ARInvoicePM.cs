using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ARInvoicePM
    {
        [Key]
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

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BillToId { get; set; }
        public string BillToName { get; set; }
        public string BillToLocalName { get; set; }
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

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BillToAddressId { get; set; }
        public string VatNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? InvoiceDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DueDate { get; set; }

        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string PrintByUserName { get; set; }
        public string IssuedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
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

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchId { get; set; }
        public string BranchName { get; set; }

        public bool IsPrinted { get; set; }
        public string ConnectedEntityReferences { get; set; }
        public CustomFieldClass Field1 { get; set; }
        public CustomFieldClass Field2 { get; set; }
        public CustomFieldClass Field3 { get; set; }
        public CustomFieldClass Field4 { get; set; }
        public CustomFieldClass Field5 { get; set; }
        public CustomFieldClass Field6 { get; set; }
        public CustomFieldClass Field7 { get; set; }
        public CustomFieldClass Field8 { get; set; }
        public CustomFieldClass Field9 { get; set; }
        public CustomFieldClass Field10 { get; set; }

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

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerRef { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? OperationalDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DateForVATInterest { get; set; }
        public bool SplitJournalByCurrency { get; set; }
        public bool IsExternalEntity { get; set; }
        public string SATXML { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }

        public string SATTransferStatusCode { get; set; }
        public string SATInvoiceStatusCode { get; set; }
        public string SATTransferStatusName { get; set; }
        public string SATInvoiceStatusName { get; set; }
        public bool Intercompany { get; set; }
        public DateTime? SATApprovalDate { get; set; }
        public bool IsShowAmountLocalCurrencyColumnInSharedLogistics { get; set; }

        

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankAccountLiteId { get; set; }

        private List<ARInvoiceLinePM> invoiceLines;
        [Include]
        [Composition]
        [Association("InvoiceInvoiceLines", "Id", "ARInvoiceId")]
        public virtual List<ARInvoiceLinePM> InvoiceLines
        {
            get
            {
                if (invoiceLines == null)
                {
                    invoiceLines = new List<ARInvoiceLinePM>();
                }

                return this.invoiceLines;
            }
            set
            {
                if (value != null)
                {
                    invoiceLines = value;
                }
            }
        }

        List<ARInvoiceEntityPM> invoiceEntities;
        [Include]
        //[Composition]
        [Association("InvoiceInvoiceEntities", "Id", "ARInvoiceId")]
        public virtual List<ARInvoiceEntityPM> InvoiceEntities
        {
            get
            {
                if (invoiceEntities == null)
                {
                    invoiceEntities = new List<ARInvoiceEntityPM>();
                }

                return this.invoiceEntities;
            }
            set
            {
                if (value != null)
                {
                    invoiceEntities = value;
                }
            }
        }

        List<ARInvoicePaymentPM> invoicePayments;
        [Include]
        [Composition]
        [Association("InvoiceInvoicePayments", "Id", "ARInvoiceId")]
        public virtual List<ARInvoicePaymentPM> InvoicePayments
        {
            get
            {
                if (invoicePayments == null)
                {
                    invoicePayments = new List<ARInvoicePaymentPM>();
                }

                return this.invoicePayments;
            }
            set
            {
                if (value != null)
                {
                    invoicePayments = value;
                }
            }
        }

        List<ARInvoiceTransferHistoryPM> invoiceTransfers;
        [Include]
        [Association("ARInvoiceTransfers", "Id", "ARInvoiceId")]
        public virtual List<ARInvoiceTransferHistoryPM> InvoiceTransfers
        {
            get
            {
                if (invoiceTransfers == null)
                {
                    invoiceTransfers = new List<ARInvoiceTransferHistoryPM>();
                }

                return this.invoiceTransfers;
            }
            set
            {
                if (value != null)
                {
                    invoiceTransfers = value;
                }
            }
        }

        private List<ConstituentPM> constituentInvoices;
        [Include]
        [Composition]
        [Association("ConstituentPMARInvoice", "Id", "ConsolidationInvoiceId")]
        public List<ConstituentPM> ConstituentInvoices
        {
            get
            {
                if (constituentInvoices == null) { constituentInvoices = new List<ConstituentPM>(); }
                return constituentInvoices;
            }

            set
            {
                if (value != null) { constituentInvoices = value; }
            }
        }

        private List<ARInvoiceTotalVATPM> totalVATs;
        [Include]
        [Composition]
        [Association("ARInvoiceTotalVATARInvoice", "Id", "ARInvoiceId")]
        public virtual List<ARInvoiceTotalVATPM> TotalVATs
        {
            get
            {
                if (totalVATs == null)
                {
                    totalVATs = new List<ARInvoiceTotalVATPM>();
                }

                return this.totalVATs;
            }

            set
            {
                if (value != null)
                {
                    totalVATs = value;
                }
            }
        }

        //Dummy Fields
        public bool SetVoided { get; set; }
        public bool SetAsSent { get; set; }
        public bool SetApproved { get; set; }
        public bool SetReTransfer { get; set; }
        public bool SetCancelDraft { get; set; }
        public bool IsExternalAPI { get; set; }
        public bool SetReSendQBO { get; set; }

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
        public string CreditARInvoice { get; set; }
        public Decimal? TotalAmountForTaxReport { get; set; }
        public Decimal? TotaVatableAmountForTaxReport { get; set; }
        public Decimal TotalVAT { get; set; }

        public bool IsFullAccounting { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ConcurrencyGUID { get; set; }
        public string NewConcurrencyGUID { get; set; }
        public string ARInvoiceStockId { get; set; }
        public bool IsInvoiceNumberFromStock { get; set; }
        public string DocumentFilingId { get; set; }
        public string BatchTaskExecutionId { get; set; }

        public bool IsCreatingConsolidation { get; set; }
        public bool IsFromConsolidationBatch { get; set; }


        public string BillToCity { get; set; }
        public string BillToCountry { get; set; }
    }
}