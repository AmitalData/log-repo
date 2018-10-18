using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ARInvoiceTotalVATList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string VatTypeId { get; set; }
        public double? InvoiceCurrencyVATAmount { get; set; }
        public double? LocalVATAmount { get; set; }
        public double? InvoiceCurrencyVatableAmount { get; set; }
        public double? LocalVatableAmount { get; set; }
        public double? VATPercent { get; set; }
        public string ARInvoiceId { get; set; }

    }
}