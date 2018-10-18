using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class APInvoiceTotalVATList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double InvoiceCurrencyVATAmount { get; set; }
        public double LocalVATAmount { get; set; }
        public double InvoiceCurrencyVatableAmount { get; set; }
        public double LocalVatableAmount { get; set; }
        public double VatPercent { get; set; }
        public string VatTypeId { get; set; }
        public string APInvoiceId { get; set; }

        public string VatTypeName { get; set; }
    }
}