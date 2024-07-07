using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.Validators;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    // [CustomValidation(typeof(ShipmentReferanceValidator), "IsShipmentReferanceValid")]
    public class ShipmentReferancePM : ChildEntitiesCustomFieldPM
    {
        public int Tenant { get; set; }
        [Key]

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }
        public int LineNumber { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReferenceType { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerId { get; set; }
        public string ReferenceValue { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
