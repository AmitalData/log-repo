using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class ShipmentReceivableLineStatus
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

 
    }
}