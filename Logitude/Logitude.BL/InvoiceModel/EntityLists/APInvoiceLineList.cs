using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APInvoiceLineList
    {
        [Key]
        public string APInvoiceId { get; set; }
        [Key]
        public int LineNumber { get; set; }
        public int Tenant { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string Notes { get; set; }
        public string ChargesTypeId { get; set; }
        public string VatTypeId { get; set; }
        public string ChargesTypeName { get; set; }
        public string VatTypeName { get; set; }
        public string EntityId { get; set; }
        public string EntityPayableId { get; set; }
        public double? RefundAmount { get; set; }
        public string ForiegnCurrencyId { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? ForiegnExchangeRate { get; set; }
        public string DebitAccount { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
    }
}