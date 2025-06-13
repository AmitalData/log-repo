using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class PickUpDeliveryTypePM : EntityPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}