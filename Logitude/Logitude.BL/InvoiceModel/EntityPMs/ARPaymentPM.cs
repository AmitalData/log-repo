using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ARPaymentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public string PaymentNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string InvoiceNumbers { get; set; }       
        public double? AmountInPaymentCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BranchId { get; set; }
        public string BranchName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BillToId { get; set; }
        public string ARAccountId { get; set; }
        public string StatusCode { get; set; }
        public bool IsClosed { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentCurrencyId { get; set; }
        
        public string PaidBy { get; set; }
        public string AccountingPaymentMethodId { get; set; }
        public string AccountingPaymentMethodCode { get; set; }
        public string DebitAccountId { get; set; }
        public string PrintNotes { get; set; }
        public string InternalNotes { get; set; }
        public double? PaymentCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BillToAddressId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? RegisterDate { get; set; }
        public string SearchFields { get; set; }
        public string BillToName { get; set; }
        public string BillToLocalName { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string CreatedByUserName { get; set; }
        public string StatusName { get; set; }
        public string AccountingPaymentMethodName { get; set; }
        public string ARAccountName { get; set; }
        public string DebitAccountName { get; set; }
                
        public double? OpenAmount { get; set; }
        public double? OpenAmountInLocalCurrency { get; set; }

        public string ChequeOrPaymentRef { get; set; }
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public string Account { get; set; }
        public DateTime? ValueDate { get; set; }
        //public string InvoiceId { get; set; } // used to mark that this payment came from an invoice and needs to connected to it;
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreditCardTypeId { get; set; }

        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public bool ReadyForTransfer { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string TransmissionError { get; set; }
        public string SATTransferStatusName { get; set; }
        public DateTime? SATApprovalDate { get; set; }

        private List<ARPaymentInvoicePM> paymentInvoices;
        [Include]
        [Composition]
        [Association("ARPaymentARPaymentInvoices", "Id", "ARPaymentId")]
        public virtual List<ARPaymentInvoicePM> PaymentInvoices
        {
            get
            {
                if (paymentInvoices == null)
                {
                    paymentInvoices = new List<ARPaymentInvoicePM>();
                }

                return this.paymentInvoices;
            }
            set
            {
                if (value != null)
                {
                    paymentInvoices = value;
                }
            }
        }


        //Dummy Fields
        public bool SetVoided { get; set; }
        public bool SetApproved { get; set; }
        public bool SetCancelApproval { get; set; }
        public bool HasInvoicesErrors { get; set; }
        public bool SetReTransfer { get; set; }
        public bool SetReSendQBO { get; set; }

        public string BankAccountId { get; set; }
        public string CashbookId { get; set; }
        public string BillToPartnerTypeId { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string SATXML { get; set; }
        public string SATTransferStatusCode { get; set; }

        public string BankAccountLiteId { get; set; }
        public string BankAccountName { get; set; }
        public string MetodoPagoCode { get; set; }
        public string TipoCadenaPago { get; set; }
        public string CertPago { get; set; }
        public string CadPago { get; set; }
        public string SelloPago { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstApproveDate { get; set; }
        public bool IsFullAccounting { get; set; }
        public string GLAccountId { get; set; }
        public string GLAccountRecoMethodCode { get; set; }

        public DateTime? FechaPago { get; set; }
        public bool IsExternalEntity { get; set; }
        public string JournalNumber { get; set; }
        public string JournalId { get; set; }
        public string CreatedByPartner { get; set; }


        private List<LedgerTransactionPM> invoicesLedgerTransactions;
        public virtual List<LedgerTransactionPM> InvoicesLedgerTransactions
        {
            get
            {
                if (invoicesLedgerTransactions == null)
                {
                    invoicesLedgerTransactions = new List<LedgerTransactionPM>();
                }

                return invoicesLedgerTransactions;
            }
            set
            {
                if (value != null)
                {
                    invoicesLedgerTransactions = value;
                }
            }
        }

        private List<ARPaymentChequeReplicaPM> paymentChequeReplicas;
        [Include]
        [Composition]
        [Association("ARPaymentARPaymentChequeReplicas", "Id", "PaymentId")]
        public virtual List<ARPaymentChequeReplicaPM> ARPaymentChequeReplicas
        {
            get
            {
                if (paymentChequeReplicas == null)
                {
                    paymentChequeReplicas = new List<ARPaymentChequeReplicaPM>();
                }

                return this.paymentChequeReplicas;
            }
            set
            {
                if (value != null)
                {
                    paymentChequeReplicas = value;
                }
            }
        }

        public bool IsPaymentNumberManuallySet { get; set; }
    }
}
