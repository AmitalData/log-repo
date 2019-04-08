using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARPayment
    {
        [Key]

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PaymentNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string BranchId { get; set; }
        public string BillToId { get; set; }
        public string ARAccountId { get; set; }
        public string StatusCode { get; set; }
        public bool IsClosed { get; set; }
        public string PaymentCurrencyId { get; set; }
        public double? AmountInPaymentCurrency { get; set; }
        public string PaidBy { get; set; }
        public string AccountingPaymentMethodId { get; set; }
        public string DebitAccountId { get; set; }
        public string PrintNotes { get; set; }
        public string InternalNotes { get; set; }
        public double? PaymentCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string BillToAddressId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string SearchFields { get; set; }
        public double? OpenAmount { get; set; }
        public string ChequeOrPaymentRef { get; set; }
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public string Account { get; set; }
        public DateTime? ValueDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string CreditCardTypeId { get; set; }
        public string BankAccountId { get; set; }
        public string CashbookId { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipmentNumber { get; set; }

        public string SATXML { get; set; }
        public string SATAdditionalFieldsXML { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string TransmissionError { get; set; }

        public string TipoCadenaPago { get; set; }
        public string CertPago { get; set; }
        public string CadPago { get; set; }
        public string SelloPago { get; set; }
        public DateTime? SATApprovalDate { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }

        public string BankAccountLiteId { get; set; }
        [ForeignKey("BankAccountLiteId")]
        public virtual BankAccountLite BankAccountLite { get; set; }

        [ForeignKey("SATPaymentMethodCode")]
        public virtual SATPaymentMethod SATPaymentMethod { get; set; }


        [ForeignKey("TransferStatusCode")]
        public virtual ARPaymentTransferStatus TransferStatus { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public User UpdatedByUser { get; set; }

        [ForeignKey("PaymentMethodId")]
        public virtual AccountingPaymentMethod AccountingPaymentMethod { get; set; }

        [ForeignKey("StatusCode")]
        public virtual ARPaymentStatus Status { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("PrintByUserId")]
        public virtual User PrintByUser { get; set; }

        [ForeignKey("LocalCurrencyId")]
        public virtual Currency LocalCurrency { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("BillToId")]
        public virtual Card BillToCard { get; set; }

        [ForeignKey("ARAccountId")]
        public virtual Account ARAccount { get; set; }

        [ForeignKey("DebitAccountId")]
        public virtual Account DebitAccount { get; set; }

        [ForeignKey("PaymentCurrencyId")]
        public virtual Currency PaymentCurrency { get; set; }

        [ForeignKey("BillToAddressId")]
        public virtual Address BillToAddress { get; set; }

        [ForeignKey("CreditCardTypeId")]
        public virtual CreditCardType CreditCardType { get; set; }


        public string SATTransferStatusCode { get; set; }
        [ForeignKey("SATTransferStatusCode")]
        public virtual SATTransferStatus SATTransferStatus { get; set; }


        public string MetodoPagoCode { get; set; }
        [ForeignKey("MetodoPagoCode")]
        public virtual MetodoPago MetodoPago { get; set; }

        [ForeignKey("ApprovedByUserId")]
        public virtual User ApprovedByUser { get; set; }
        public DateTime? FirstApproveDate { get; set; }
        public bool IsFullAccounting { get; set; }
        public bool IsExternalEntity { get; set; }
    }
}