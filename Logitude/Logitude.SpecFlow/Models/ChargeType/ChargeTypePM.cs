namespace Logitude.SpecFlow.Models.ChargeType
{
    public class ChargeTypePM
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string ChargesGroupCode { get; set; }
        public string MeasurementId { get; set; }
        public string ChargesGroupId { get; set; }
        public int Tenant { get; set; }
    }
}