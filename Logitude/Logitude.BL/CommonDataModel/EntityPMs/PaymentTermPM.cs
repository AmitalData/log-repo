using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class PaymentTermPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EnglishName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int Days { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Description { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalDescription { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalIdExternalId { get; set; }

        public string ComputedLocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool DisplayInLOV { get; set; }        
        public string SearchFields { get; set; }
        public bool IsManuallySet { get; set; }     
        public bool CurrentMonth { get; set; }
        public string CalculatedLocalName { get; set; }
        public string CalculatedEnglishName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FromDateTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }
    }
}