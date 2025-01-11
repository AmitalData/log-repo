using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class ShipmentAssemblyPM : EntityPM
    {
        //[Key]
        //public string Id { get; set; }
        //public int Tenant { get; set; }
        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ShipmentId { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string ShipperId { get; set; }
        //public string ShipperName { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string House { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public DateTime CreateDate { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public DateTime UpdateDate { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string CreatedByUserId { get; set; }
        //public string CreatedByUserName { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string UpdatedByUserId { get; set; }
        //public string UpdatedByUserName { get; set; }

        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
