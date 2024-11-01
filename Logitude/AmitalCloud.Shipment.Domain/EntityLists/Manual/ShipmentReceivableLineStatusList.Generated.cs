using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentReceivableLineStatusList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}