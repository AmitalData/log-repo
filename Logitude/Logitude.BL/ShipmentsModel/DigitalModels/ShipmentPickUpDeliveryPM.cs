using System;

namespace Logitude.BL.ShipmentsModel.DigitalModels
{
    public class ShipmentPickUpDeliveryPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string PickUpDeliveryTypeCode { get; set; }
        public string PickUpDeliveryNumber { get; set; }
    }
}
