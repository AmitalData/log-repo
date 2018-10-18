using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ARInvoiceLineList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARInvoiceId { get; set; }       
        public string ChargesTypeId { get; set; }        
        public string CurrencyId { get; set; }        
        public double? Amount { get; set; }
        public double? AmountLocalCurrency { get; set; }       
        public double? AmountInvoiceCurrency { get; set; }                     
        public string VatTypeId { get; set; }
        public string Notes { get; set; }        
        public double? Rate { get; set; }
        public string ReceivableId { get; set; }
        public string CreditAccount { get; set; }  
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public DateTime? ExchangeRateDate { get; set; }
    }
}
