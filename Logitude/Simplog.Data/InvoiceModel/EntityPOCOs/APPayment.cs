using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APPayment
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PaymentNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintedByUserId { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string VendorId { get; set; }
        public string StatusCode { get; set; }
        public bool IsClosed { get; set; }
        public string PaymentMethodId { get; set; }
        public string AccountingPaymentMethodId { get; set; }
        public string PrintNotes { get; set; }
        public string InternalNotes { get; set; }
        public string PaymentCurrencyId { get; set; }
        public double? AmountInPaymentCurrency { get; set; }
        public double? PaymentCurrencyExchangeRate { get; set; }
        public DateTime? PaymentCurrencyExchangeRateDate { get; set; }
        public string VendorAddressId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public double? OpenAmount { get; set; }
        public string ChequeOrPaymentRef { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public string Account { get; set; }
        public string SearchFields { get; set; }
        public string BranchId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string CreditCardTypeId { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferError { get; set; }

        public Decimal? TaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }

        [ForeignKey("ApprovedByUserId")]
        public User ApprovedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public User UpdatedByUser { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
        [ForeignKey("PaymentMethodId")]
        public virtual APPaymentMethod PaymentMethod { get; set; }
        [ForeignKey("AccountingPaymentMethodId")]
        public virtual AccountingPaymentMethod AccountingPaymentMethod { get; set; }
        [ForeignKey("StatusCode")]
        public virtual APPaymentStatus Status { get; set; }
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("PrintedByUserId")]
        public virtual User PrintedByUser { get; set; }
        [ForeignKey("LocalCurrencyId")]
        public virtual Currency LocalCurrency { get; set; }
        [ForeignKey("VendorId")]
        public virtual Card VendorCard { get; set; }
        [ForeignKey("PaymentCurrencyId")]
        public virtual Currency PaymentCurrency { get; set; }
        [ForeignKey("VendorAddressId")]
        public virtual Address VendorAddress { get; set; }
        [ForeignKey("CreditCardTypeId")]
        public virtual CreditCardType CreditCardType { get; set; }
        [ForeignKey("TransferStatusCode")]
        public virtual APPaymentTransferStatus TransferStatus { get; set; }

        public string BankAccountId { get; set; }

        public DateTime? FirstApproveDate { get; set; }
     
    }
}