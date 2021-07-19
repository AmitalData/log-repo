namespace Logitude.QuoteTests.Models
{
    public class QuotePackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Weight { get; set; }
        public double? Width { get; set; }
        public int? Quantity { get; set; }
        public string QuoteId { get; set; }
        public int ChangeSetOp { get; set; }
    }
}