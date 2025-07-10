using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    public partial class ShipmentPackageItemPM : EntityPM
    {
        //[Key]
        //public string PackageId { get; set; }
        //[Key]
        //public int LineNumber { get; set; }

        //public int Tenant { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public string Description { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public int? Quantity { get; set; }

        //[CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        //public decimal? GoodsValue { get; set; }

        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}