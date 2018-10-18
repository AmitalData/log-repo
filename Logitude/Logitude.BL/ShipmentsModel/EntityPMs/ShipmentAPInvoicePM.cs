using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentAPInvoicePM
    {
        [Key]
        public string Id { get; set; }
        public string ShipmentId { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string InvoiceTypeCode { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? GrandTotalInLocalCurrency { get; set; }
        public double? GrandTotalInProfitCurrency { get; set; }
        public double? GrandTotalInInvoiceCurrency { get; set; }
    }
}