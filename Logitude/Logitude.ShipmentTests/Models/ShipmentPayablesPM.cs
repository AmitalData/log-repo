namespace Logitude.ShipmentTests.Models
{
    public class ShipmentPayablesPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ShipmentPayableLineStatusCode { get; set; }
        public double? UnitPrice { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
    }
}