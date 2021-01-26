using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ARPaymentList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PaymentNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime? PrintDate { get; set; }
        public string PrintByUserId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string BillToId { get; set; }
        public string ARAccountId { get; set; }
        public string StatusCode { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsClosed { get; set; }
        public string PaymentCurrencyId { get; set; }
        public double? AmountInPaymentCurrency { get; set; }
        public string PaidBy { get; set; }
        public string AccountingPaymentMethodCode { get; set; }
        public string AccountingPaymentMethodId { get; set; }
        public string DebitAccountId { get; set; }
        public string PrintNotes { get; set; }
        public string InternalNotes { get; set; }
        public double? PaymentCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string BillToAddressId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string SearchFields { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string InvoiceNumbers { get; set; }
        public string BillToName { get; set; }
        public string BillToLocalName { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public string CreatedByUserName { get; set; }
        public string StatusName { get; set; }
        public string AccountingPaymentMethodName { get; set; }
        public string ARAccountName { get; set; }
        public string CreditAccountName { get; set; }
        public double? OpenAmount { get; set; }
        public double? OpenAmountInLocalCurrency { get; set; }
        public string ChequeOrPaymentRef { get; set; }
        public string Bank { get; set; }
        public string BankBranch { get; set; }
        public string Account { get; set; }
        public DateTime? ValueDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreditCardTypeId { get; set; }
        public string BankAccountId { get; set; }
        public string CashbookId { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public bool ReadyForTransfer { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShipmentNumber { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string SATTransferStatusCode { get; set; }
        public string SATTransferStatusName { get; set; }
        public string TransmissionError { get; set; }
        public string BankAccountLiteId { get; set; }
        public string BankAccountName { get; set; }
        public DateTime? AccountingCancelationDate { get; set; }
        public string CancelationNotes { get; set; }
        public string VoidedByJournalNumber { get; set; }

        public string MetodoPagoCode { get; set; }
        public string TipoCadenaPago { get; set; }
        public string CertPago { get; set; }
        public string CadPago { get; set; }
        public string SelloPago { get; set; }
        public DateTime? SATApprovalDate { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? FirstApproveDate { get; set; }
        public bool IsFullAccounting { get; set; }
        public DateTime? FechaPago { get; set; }

        public string CreatedByPartner { get; set; }
        public bool IsPaymentNumberManuallySet { get; set; }

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
        public string PartnerId { get; set; }
    }
}
