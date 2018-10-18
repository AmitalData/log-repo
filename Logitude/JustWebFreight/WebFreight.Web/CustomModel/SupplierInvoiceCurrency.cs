using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel
{
    public class SupplierInvoiceCurrency
    {
        [Key]
        public string Id { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}