using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentTypePM : EntityPM
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
        public string TransportModeId { get; set; }
        public string SearchFields { get; set; }
    }
}