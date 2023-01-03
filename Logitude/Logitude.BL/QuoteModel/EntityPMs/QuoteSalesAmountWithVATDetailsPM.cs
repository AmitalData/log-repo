using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteSalesAmountWithVATDetailsPM
    {
        [Key]
        public string CurrencyCode { get; set; }
        public double? SubtotalAmount { get; set; }
        public double? VATAmount { get; set; }
        public double? TotalAmount { get; set; }
        public string VATPercentage { get; set; }
    }
}
