using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ComputingPartnerTablePM
    {
        [Key]
        public string ObjectTableId { get; set; }
        [Key]
        public string ComputingPartnerId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Name { get; set; } 

        public int Tenant { get; set; }
        public bool HasPartnerList { get; set; }
        public bool MustUsePartnerList { get; set; }
        public bool TransalationRequired { get; set; }
        public bool TenantLevelTranslationBlocked { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        public string ObjectTableName { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string ComputingPartnerName { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}