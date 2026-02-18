using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentPayableAmountTypePM : EntityPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

    }
}