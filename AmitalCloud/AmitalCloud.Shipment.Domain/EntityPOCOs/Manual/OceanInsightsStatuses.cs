using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class OceanInsightsStatuses
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OceanInsightsRequestId { get; set; }
        public string ContentDocumentId { get; set; }
        public string CommunicationLogId { get; set; }
        public DateTime CreateDate { get; set; }
		public string XML { get; set; }

		[ForeignKey("CommunicationLogId")]
        public virtual CommunicationLog CommunicationLog { get; set; }

        [ForeignKey("OceanInsightsRequestId")]
        public virtual OceanInsightsRequest OceanInsightsRequest { get; set; }

        [ForeignKey("ContentDocumentId")]
        public virtual Document Document { get; set; }
     
    }
}
