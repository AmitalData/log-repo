using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class MappedShipmentDirections
    {
        [Key] 
        [Column("Tenant", Order = 1)]
        public int Tenant { get; set; } 

        [Key]
        [ForeignKey("Direction")]
        [Column("ShipmentDirectionId", Order = 2)]
        public string ShipmentDirectionId { get; set; }

        public virtual Direction Direction { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}
