using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class AWBCustomsInformationPM : EntityPM
    {
        //[Key]
        //public string Code { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Name { get; set; }
        //public string SearchFields { get; set; }
    }
}