using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.DataContracts;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class APInvoicePaymentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string APInvoiceId { get; set; }
        public string APPaymentId { get; set; }
        public string ForeignCurrencyId { get; set; }
        public double? ForeignAmount { get; set; }
        public double? LocalAmount { get; set; }
        public string PaymentNumber { get; set; }
        public double? PaymentAmount { get; set; }
        public double? ExchangeRate { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
