namespace Logitude.AccountingTests.Models
{
    public class APInvoiceLinePM
    {
        public int Tenant { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeName { get; set; }
        public string Description { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ForiegnCurrencyAmount { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string VatTypeId { get; set; }
        public string VatTypeName { get; set; }
        public double? VatPercentage { get; set; }
        public string EntityId { get; set; }
    }
}
