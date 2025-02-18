using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class OceanInsightsStatusLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OceanInsigntRequestId { get; set; }
        public string XML { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
