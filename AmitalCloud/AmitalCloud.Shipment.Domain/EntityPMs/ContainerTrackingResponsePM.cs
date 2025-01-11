using Logitude.Server.Tools;
using System;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ContainerTrackingResponsePM : EntityPM
    {
        public string Id { get; set; }

        public int Tenant { get; set; }

        public DateTime CreateDate { get; set; }
        public string SearchFields { get; set; }
        public string CommunicationLogId { get; set; }
        public string ContainerTrackingRequestId { get; set; }

    }
}