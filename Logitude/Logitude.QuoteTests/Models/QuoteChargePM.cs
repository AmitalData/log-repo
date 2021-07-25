

namespace Logitude.QuoteTests.Models
{
    public class QuoteChargePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string QuoteId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string CostCurrencyId { get; set; }
        public string SaleCurrencyId { get; set; }
        public double? CostExchangeRate { get; set; }
        public double? SaleExchangeRate { get; set; }
        public string CostMeasurementId { get; set; }
        public string SaleMeasurementId { get; set; }

    }
}
