using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class ShipmentPayableAmountType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<ShipmentPayable> ShipmentPayables { get; set; }
    }
}