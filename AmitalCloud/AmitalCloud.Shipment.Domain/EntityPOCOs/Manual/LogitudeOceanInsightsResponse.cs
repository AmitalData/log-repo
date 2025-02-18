using System;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class LogitudeOceanInsightsResponse
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SCACCode { get; set; }
        public string ContainerNumber { get; set; }
        public string CarrierName { get; set; }
        public DateTime FirstResponseDate { get; set; }
        public DateTime LastResponseDate { get; set; }

    }
}
