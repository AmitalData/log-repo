using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainerDiscrepancy
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public string ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        public string ContainerId { get; set; }
        [ForeignKey("ContainerId")]
        public virtual Container Container { get; set; }
        public DateTime DiscrepancyDate { get; set; }
        public string Discrepancy { get; set; }


    }
}
