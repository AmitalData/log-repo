using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class ARInvoicePayment
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? LocalAmount { get; set; }
        public double? ForeignAmount { get; set; }
        public double? PaymentAmount { get; set; }
        public double? ExchangeRate { get; set; }

        [ForeignKey("ARPaymentId")]
        public virtual ARPayment ARPayment { get; set; }
        public string ARPaymentId { get; set; }

        [ForeignKey("ARInvoiceId")]
        public virtual ARInvoice ARInvoice { get; set; }
        public string ARInvoiceId { get; set; }

        [ForeignKey("ForeignCurrencyId")]
        public virtual Currency ForeignCurrency { get; set; }
        public string ForeignCurrencyId { get; set; }

    }
}