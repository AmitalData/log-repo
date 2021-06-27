using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ContainerList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipmentPackagesId { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public string CarrierName { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public string Master { get; set; }
        public string MainCarriageVesselId { get; set; }
        public string VesselName { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string ShipmentId { get; set; }
        public string EmptyPickupLocation { get; set; }
        public DateTime? EstimatedEmptyPickupDate { get; set; }
        public DateTime? ActualEmptyPickupDate { get; set; }
        public DateTime? EstimatedGateInDate { get; set; }
        public DateTime? ActualGateInDate { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public string CurrentLocation { get; set; }
        public bool HasContainerException { get; set; }

    }
}
