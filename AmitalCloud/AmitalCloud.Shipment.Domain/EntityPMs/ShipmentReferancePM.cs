using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using AmitalCloud.Shipment.Def.Validators;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    [CustomValidation(typeof(Validators.ShipmentClassLevelValidator), "ValidateClass")]
    // [CustomValidation(typeof(ShipmentReferanceValidator), "IsShipmentReferanceValid")]
    public partial class ShipmentReferancePM : ChildEntitiesCustomFieldPM
    {
        public int Tenant { get; set; }
        [Key]

        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }
        public int LineNumber { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string ReferenceType { get; set; }
        [CustomValidation(typeof(Validators.ShipmentValidationClass), "ValidateClass")]
        public string PartnerId { get; set; }
        public string ReferenceValue { get; set; }

        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
