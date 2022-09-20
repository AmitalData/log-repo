using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentDigitalField
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCustomerArchived { get; set; }
    }
}
