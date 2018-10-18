using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ARInvoicePaymentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ARPaymentId { get; set; }
        public string ARInvoiceId { get; set; }
        public double? LocalAmount { get; set; }
        public double? ForeignAmount { get; set; }
        public double? PaymentAmount { get; set; }
        public string ForeignCurrencyId { get; set; }
        public string ForeignCurrencyCode { get; set; }
        public double? ExchangeRate { get; set; }
        public string PaymentNumber { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
