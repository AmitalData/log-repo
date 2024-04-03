using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainerTrackingResponse
    {
       
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public DateTime CreateDate { get; set; }
        public string SearchFields { get; set; }
        public string CommunicationLogId { get; set; }
        public string ContainerTrackingRequestId { get; set; }

        [ForeignKey("ContainerTrackingRequestId")]
        public virtual ContainerTrackingRequest ContainerTrackingRequest { get; set; }
        
    }
}