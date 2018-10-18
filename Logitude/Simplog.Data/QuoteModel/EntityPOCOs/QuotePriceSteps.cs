using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.QuoteModel.EntityPOCOs
{
    public class QuotePriceSteps
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? SaleUnitPrice { get; set; }
        public double? Step { get; set; }
        public double? MarkupValue { get; set; }

        public string QuoteId { get; set; }
        [ForeignKey("QuoteId")]
        public Quote Quote { get; set; }  

        public string QuoteChargeId { get; set; }
        [ForeignKey("QuoteChargeId")]
        public QuoteCharge QuoteCharge { get; set; }                   
    }
}