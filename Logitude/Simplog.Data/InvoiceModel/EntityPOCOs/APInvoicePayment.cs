using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APInvoicePayment
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? LocalAmount { get; set; }
        public double? ForeignAmount { get; set; }
        public double? PaymentAmount { get; set; }
        public double? ExchangeRate { get; set; }

        [ForeignKey("APInvoiceId")]
        public virtual APInvoice APInvoice { get; set; }
        public string APInvoiceId { get; set; }

        [ForeignKey("APPaymentId")]
        public virtual APPayment APPayment { get; set; }
        public string APPaymentId { get; set; }

        [ForeignKey("ForeignCurrencyId")]
        public virtual Currency ForeignCurrency { get; set; }
        public string ForeignCurrencyId { get; set; }

    }
}