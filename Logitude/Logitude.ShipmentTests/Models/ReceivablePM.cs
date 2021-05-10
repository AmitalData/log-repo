using Logitude.ShipmentTests.Models.Accounting;

namespace Logitude.ShipmentTests.Models
{
    public class ReceivablePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ChargesTypeId { get; set; }
        public string ChargesTypeCode { get; set; }
        public string ChargesTypeName { get; set; }
        public string ShipmentReceivableLineStatusCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public double? Rate { get; set; }
        public double? TotalAmountLocal { get; set; }
        public double? TotalAmount { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

    }
}