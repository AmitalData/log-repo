using System;

namespace Logitude.BL.ShipmentsModel.DigitalModels
{
    public class ShipmentAdditionalFields
    {
        public string Id { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public DateTime? FirstPickupATD { get; set; }
        public DateTime? FirstPickupATA { get; set; }
        public DateTime? LastDeliveryATD { get; set; }
        public DateTime? LastDeliveryATA { get; set; }
    }
}