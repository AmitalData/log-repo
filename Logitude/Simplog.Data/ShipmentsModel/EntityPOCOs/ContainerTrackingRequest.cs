using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainerTrackingRequest
    {
       
        [Key]
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int Tenant { get; set; }
        public string Provider { get; set; }
        public string ContainerNumber { get; set; }
        public string Master { get; set; }
        public string RequestId { get; set; }
        public string ContainerId { get; set; }
        public string ShipmentId { get; set; }
        public string SearchFields { get; set; }
        public string CarrierCode { get; set; }
        public string ScacCode { get; set; }
        public string Status { get; set; }
        public bool IsSimulate { get; set; }


    }
}