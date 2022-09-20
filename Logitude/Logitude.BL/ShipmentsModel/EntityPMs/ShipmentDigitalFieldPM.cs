using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentDigitalFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCustomerArchived { get; set; }
    }
}
