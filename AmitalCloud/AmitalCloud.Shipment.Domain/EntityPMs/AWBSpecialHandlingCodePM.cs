using Logitude.Server.Tools;
using System.ComponentModel.DataAnnotations;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class AWBSpecialHandlingCodePM : EntityPM
    {
        //[Key]
        //public string Id { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Code { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Name { get; set; }

        //public bool IsIATA { get; set; }
        //public bool InActive { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string AirlineId { get; set; }

        //public string SearchFields { get; set; }
    }
}