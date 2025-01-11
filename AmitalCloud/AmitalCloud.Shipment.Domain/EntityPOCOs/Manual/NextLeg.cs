using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class NextLeg
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<Shipment> Shipments { get; set; }
    }
}