using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APPaymentList
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
        public string LocalCurrencyCode { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string VendorId { get; set; }
        public string StatusCode { get; set; }
        public bool IsClosed { get; set; }
        public string PaymentMethodCode { get; set; }
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
        public string SearchFields { get; set; }
        public string Account { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string ExternalAccountingEntityId { get; set; }
        public string CreatedByUserName { get; set; }
        public string VendorName { get; set; }
        public string VendorLocalName { get; set; }
        public string StatusName { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string CreditCardTypeId { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferError { get; set; }
        public string TransferStatusName { get; set; }
        public bool ReadyForTransfer { get; set; }
        public Decimal? TaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
        public string AccountingPaymentMethodId { get; set; }
        public string BankAccountId { get; set; }
        public DateTime? FirstApproveDate { get; set; }

        public string VendorBankAddress { get; set; }
        public string VendorBankName { get; set; }
        public string VendorBankAccountNumber { get; set; }
        public string VendorSwift { get; set; }
        public string VendorIBANNumber { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public double? ExternalPaymentAmount { get; set; }
        public DateTime? ExternalPaymentDate { get; set; }
        public string ExternalPaymentNotes { get; set; }
        public string VendorCode { get; set; }
        public string ConnectedInvoicesNumbers { get; set; }
        public string MasavInterfaceId { get; set; }
    }
}