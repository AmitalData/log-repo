using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ContainerDiscrepancyPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime DiscrepancyDate { get; set; }
        public string Discrepancy { get; set; }
        public string SearchFields { get; set; }
        public string ShipmentId { get; set; }
        public string ContainerId { get; set; }

    }
}
