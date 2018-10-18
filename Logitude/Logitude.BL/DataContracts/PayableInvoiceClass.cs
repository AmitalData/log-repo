using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class PayableInvoiceClass
    {
        [Key]
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string StatusName { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public string LocalCurrencyId { get; set; }
        public string LocalCurrencyCode { get; set; }        
        public double? GrandTotalInInvoiceCurrency { get; set; }
        public double? GrandTotalInLocalCurrency { get; set; }
    }
}