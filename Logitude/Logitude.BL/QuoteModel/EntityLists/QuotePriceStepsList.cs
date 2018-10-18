using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.QuoteModel.EntityLists
{
    public class QuotePriceStepsList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? CostUnitPrice { get; set; }
        public double? SaleUnitPrice { get; set; }
        public double? Step { get; set; }
        public double? MarkupValue { get; set; }
        public string QuoteId { get; set; }
        public string QuoteChargeId { get; set; }
    }
}