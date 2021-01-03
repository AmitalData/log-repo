namespace Logitude.SecurityTests.Models.Shipment
{
    public class ShipmentPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string shipmentType { get; set; }
        public string NewConcurrencyGUID { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string BasicFreightId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string FreightPayerAddressId { get; set; }
        public string FreightPayerId { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string MainCarriageFromPortId { get; set; }
    }
}