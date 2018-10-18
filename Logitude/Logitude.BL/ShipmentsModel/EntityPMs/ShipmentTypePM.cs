using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentTypePM
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
        public string TransportModeId { get; set; }
        public string SearchFields { get; set; }
    }
}