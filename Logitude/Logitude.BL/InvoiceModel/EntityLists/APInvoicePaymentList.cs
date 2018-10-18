using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APInvoicePaymentList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string APInvoiceId { get; set; }
        public string APPaymentId { get; set; }
        public string ForeignCurrencyId { get; set; }
        public double? ForeignAmount { get; set; }
        public double? LocalAmount { get; set; }
    }
}