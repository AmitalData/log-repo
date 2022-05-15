using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerTrackingRequestPM
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int Tenant { get; set; }
        public string Provider { get; set; }
        public string ContainerNumber { get; set; }
        public string Master { get; set; }
        public string RequestId { get; set; }
        public string SearchFields { get; set; }
    }
}