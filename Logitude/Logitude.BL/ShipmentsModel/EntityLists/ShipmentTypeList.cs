using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentTypeList
    {
        [Key]
        public string Id { get; set; }        
        public string Name { get; set; }
        public string TransportModeName { get; set; }
        public string TransportModeId { get; set; }
        public string SearchFields { get; set; }
         
    }
}