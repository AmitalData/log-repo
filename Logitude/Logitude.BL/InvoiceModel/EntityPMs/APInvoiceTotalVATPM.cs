using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class APInvoiceTotalVATPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string APInvoiceId { get; set; }
        public string VatTypeId { get; set; }
        public double? VatPercent { get; set; }
        public string VatTypeName { get; set; }
        public string VatTypeCell { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public double InvoiceCurrencyVatableAmount { get; set; }
        public double LocalVatableAmount { get; set; }
        public double? ProfitVatableAmount { get; set; }
        public double InvoiceCurrencyVATAmount { get; set; }
        public double LocalVATAmount { get; set; }
        public double? ProfitCurrencyVATAmount { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}