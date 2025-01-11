using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentPayableLineStatusList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}