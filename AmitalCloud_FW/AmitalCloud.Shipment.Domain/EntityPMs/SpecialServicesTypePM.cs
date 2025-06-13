using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class SpecialServicesTypePM : EntityPM
    {
        //[Key]
        //public string Id { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Code { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string EnglishName { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string LocalName { get; set; }

        //public int Tenant { get; set; }
        //public bool IsHybrid { get; set; }
        //public bool IsSecured { get; set; }
        //public string SearchFields { get; set; }
        //public bool InActive { get; set; }
    }
}