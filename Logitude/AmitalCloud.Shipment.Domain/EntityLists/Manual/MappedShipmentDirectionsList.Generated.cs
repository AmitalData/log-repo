using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class MappedShipmentDirectionsList
    {
        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ShipmentDirectionId { get; set; }  
        public DateTime UpdateDateTime { get; set; }
    }
}
