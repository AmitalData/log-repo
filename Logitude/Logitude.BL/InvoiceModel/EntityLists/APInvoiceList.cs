using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APInvoiceList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InternalNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string VendorId { get; set; }
        public string VATNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }
        public string InternalNotes { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string MainEntityReference { get; set; }
        public string MainEntityId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string APInvoiceTypeCode { get; set; }
        public string VendorName { get; set; }
        public string VendorLocalName { get; set; }
        public string VendorCode { get; set; }
        public string PaymentTermName { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string StatusName { get; set; }
        public string CreatedByUserName { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string UpdatedByUserName { get; set; }
        public string APInvoiceTypeName { get; set; }
        public double? AmountDue { get; set; }
        public double? AmountPaid { get; set; }
        public string SearchFields { get; set; }
        public double? RefundAmount { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string Description { get; set; }
        public string CreditAccount { get; set; }
        public int TransferTries { get; set; }
        public string TransferError { get; set; }
        public bool IsTransferStarted { get; set; }
        public string TransferStatusCode { get; set; }
        public string TransferStatusName { get; set; }
        public string AccountingExternalCode { get; set; }
        public bool ReadyForTransfer { get; set; }
        public string PaymentTermExternalId { get; set; }
        public bool IsDueDateColorRed { get; set; }
        public bool IsMultipleEntities { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public string ApprovedByUserName { get; set; }
        public DateTime? OperationalDate { get; set; }
        public string VendorGLAccountId { get; set; }
        public DateTime? AccountingDate { get; set; }
        public bool IsExternalEntity { get; set; }
        public bool IsGeneralInvoice { get; set; }
        public DateTime? FirstApproveDate { get; set; }
        public string VendorCity { get; set; }
        public string VendorCountry { get; set; }
        public string CreatedByPartner { get; set; }
    }
}
