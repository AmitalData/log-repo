using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerTrackingResponsePM
    {
        public string Id { get; set; }

        public int Tenant { get; set; }

        public DateTime CreateDate { get; set; }
        public string SearchFields { get; set; }
        public string CommunicationLogId { get; set; }
        public string ContainerTrackingRequestId { get; set; }

    }
}