using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
    public class APInvoiceTotalVAT
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
        public double? ProfitCurrencyVATAmount { get; set; }
        public double? ProfitVatableAmount { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }

        [ForeignKey("APInvoiceId")]
        public virtual APInvoice APInvoice { get; set; }
    }
}