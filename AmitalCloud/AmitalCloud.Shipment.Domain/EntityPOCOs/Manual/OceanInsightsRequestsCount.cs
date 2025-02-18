using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class OceanInsightsRequestsCount
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string Type { get; set; }
        public string OceanInsigntId { get; set; }
        public string BLNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerSubscriptionId { get; set; }

    }
}
