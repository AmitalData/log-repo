using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class OtherParticipantIdPM : EntityPM
    {
        //[Key]
        //public string Code { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Name { get; set; }
        //public string SearchFields { get; set; }
    }
}
