using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class CustomsTransferLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }       
        public string CustomsTransferHeaderId { get; set; }
        public string SearchFields { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        
        public virtual CustomsTransferHeader CustomsTransferHeader { get; set; }
    }
}
