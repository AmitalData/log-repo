using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentCustomerType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool ShowInLOV { get; set; }        
    }
}