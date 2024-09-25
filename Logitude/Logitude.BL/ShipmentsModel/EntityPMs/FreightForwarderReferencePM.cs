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
    // [CustomValidation(typeof(FreightForwarderReferenceValidator), "IsFreightForwarderReferenceValid")]
    public class FreightForwarderReferencePM : ChildEntitiesCustomFieldPM
    {
        [Key]
        public int Tenant { get; set; }
        [Key]

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public bool ForwarderFileConnect { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
