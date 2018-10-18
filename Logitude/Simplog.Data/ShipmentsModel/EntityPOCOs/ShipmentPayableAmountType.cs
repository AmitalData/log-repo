using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPayableAmountType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<ShipmentPayable> ShipmentPayables { get; set; }
    }
}