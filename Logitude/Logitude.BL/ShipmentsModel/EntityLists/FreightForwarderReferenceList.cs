using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class FreightForwarderReferenceList
    {
        [Key]
        public int Tenant { get; set; }
        [Key]
        public string ShipmentId { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public bool ForwarderFileConnect { get; set; }
    }
}