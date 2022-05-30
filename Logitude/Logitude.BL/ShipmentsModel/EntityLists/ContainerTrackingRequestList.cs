using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ContainerTrackingRequestList
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int Tenant { get; set; }
        public string Provider { get; set; }
        public string ContainerNumber { get; set; }
        public string Master { get; set; }
        public string RequestId { get; set; }
        public string ContainerId { get; set; }
        public string ShipmentId { get; set; }
        public string CarrierCode { get; set; }
        public string Status { get; set; }
        public string SearchFields { get; set; }
        public bool IsSimulate { get; set; }
    }
}