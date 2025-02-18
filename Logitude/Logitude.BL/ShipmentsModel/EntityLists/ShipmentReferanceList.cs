using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ShipmentReferanceList
    {
        [Key]
        public string ShipmentId { get; set; }
        public int Tenant { get; set; }
        public int LineNumber { get; set; }
        public string ReferenceType { get; set; }
        public string PartnerId { get; set; }
        public string ReferenceValue { get; set; }
    }
}