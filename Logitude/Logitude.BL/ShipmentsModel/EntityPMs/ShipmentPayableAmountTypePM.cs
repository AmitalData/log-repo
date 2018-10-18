using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentPayableAmountTypePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

    }
}