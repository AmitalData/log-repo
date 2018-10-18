using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class PickUpDeliveryFromToType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<ShipmentPickUpDelivery> FromShipmentPickUpDeliveries { get; set; }
        //public List<ShipmentPickUpDelivery> ToShipmentPickUpDeliveries { get; set; }

    }
}