using System;
using System.Collections.Generic;

namespace Logitude.AccountingTests.Models
{
    public class APInvoicePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string BranchId { get; set; }
        public string VendorId { get; set; }
        public string VATNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentTermId { get; set; }
        public DateTime? DueDate { get; set; }
        public double? InvoiceCurrencyExchangeRate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string ProfitCurrencyId { get; set; }
        public string LocalCurrencyId { get; set; }
        public double? AmountDue { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }

        public List<APInvoiceLinePM> InvoiceLines { get; set; }
    }
}
