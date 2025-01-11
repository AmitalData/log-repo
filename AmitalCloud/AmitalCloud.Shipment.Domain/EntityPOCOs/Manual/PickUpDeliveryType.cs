using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Shipment.Domain.EntityPOCOs
{
    public class PickUpDeliveryType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<ShipmentPickUpDelivery> ShipmentPickUpDeliveries { get; set; }

    }
}