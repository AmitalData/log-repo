using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class AWBChargesCodePM : EntityPM
    {
        //[Key]
        //public string Code { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Name { get; set; }

        //public string SearchFields { get; set; }
    }
}