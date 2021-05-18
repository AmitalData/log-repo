using System;
using System.Collections.Generic;
namespace Logitude.ShipmentTests.Models.Accounting.ARInvoice
{
    public class ARInvoicePM
    {
        public string Id { get; set; }
        public string ShipmentsNumbers { get; set; }
        public int Tenant { get; set; }
        public string BranchId { get; set; }
        public string PartnerId { get; set; }//
        public string BillToId { get; set; }//
        public string IssuedByUserId { get; set; }
        public string ARInvoiceTypeCode { get; set; }
        public string VATNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public double? ProfitCurrencyExchangeRate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? AmountDue { get; set; }
        public double? InvoiceExpectedAmount { get; set; }
        public double? AmountInInvoiceCurrency { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public double? AmountInProfitCurrency { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public double? SubTotalInInvoiceCurrency { get; set; }
        public double? SubTotalInLocalCurrency { get; set; }
        public bool IsInvoiceNumberManuallySet { get; set; }
        public List<ARInvoiceLinePM> InvoiceLines { get; set; }
    }
}
