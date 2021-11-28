using System;

namespace Logitude.BL.ShipmentsModel.DigitalModels
{
    public class ShipmentAdditionalFields
    {
        public string ShipmentId { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public DateTime? FirstPickupATD { get; set; }
        public DateTime? FirstPickupATA { get; set; }
        public DateTime? LastDeliveryATD { get; set; }
        public DateTime? LastDeliveryATA { get; set; }
        public string ShipmentOrdersType { get; set; }
        public string NumberOfOrderPackages { get; set; }

        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public string Transshipment1FromPortName { get; set; }
        public string Transshipment1FromPortCountryCode { get; set; }
        public string Transshipment1ToPortName { get; set; }
        public string Transshipment1ToPortCountryCode { get; set; }

        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment2FromPortCountryCode { get; set; }
        public string Transshipment2ToPortName { get; set; }
        public string Transshipment2ToPortCountryCode { get; set; }

        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Transshipment3FromPortCountryCode { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCountryCode { get; set; }
    }
}