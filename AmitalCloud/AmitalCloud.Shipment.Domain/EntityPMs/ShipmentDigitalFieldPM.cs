using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentDigitalFieldPM : EntityPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsCustomerArchived { get; set; }
    }
}
