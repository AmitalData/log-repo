using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARInvoiceAnalytic
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string InvoiceNumber { get; set; }        
        public DateTime? InvoiceDate { get; set; }
        public string PaymentTermId { get; set; }
        public string BankAccountLiteId { get; set; }
        public string DraftNumber { get; set; }
        public string SalesmanUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsConstituentInvoice { get; set; }
        public bool Intercompany { get; set; }
        public bool IsConsolidationInvoice { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string PrepaidCollectId { get; set; }
        public string BillToId { get; set; }
        public string ARInvoiceTypeCode { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public bool IsClosed { get; set; }
        public string ProfitCurrencyId { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MainEntityReference { get; set; }
        public string VatNumber { get; set; }
        public string BillToAddressId { get; set; }
        public double? AmountDueInLocalCurrency { get; set; }
        public double? AmountDueInProfitCurrency { get; set; }
        public string BranchId { get; set; }
        public string PrintByUserId { get; set; }
        public string TransferError { get; set; }
        public string RegionalTaxId { get; set; }
        public bool IsTransferStarted { get; set; }
        public bool Sent { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsInvoiceNumberManuallySet { get; set; }
        public bool IsPrinted { get; set; }
        public string TransferStatusCode { get; set; }
        public string PartnerId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? PrintDate { get; set; }
        public string ApprovedByUserId { get; set; }
        public DateTime? OperationalDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? RegionalTaxPercentage { get; set; }
    }
}


