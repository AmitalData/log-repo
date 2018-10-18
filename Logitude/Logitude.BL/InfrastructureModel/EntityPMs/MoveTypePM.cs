using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class MoveTypePM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MoveTypeEnglishName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MoveTypeLocalName { get; set; }

        public string TransportModeId { get; set; }

        public bool AddedManually { get; set; }

        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }

        public string SearchFields { get; set; }

        public bool IsAir { get; set; }

        public bool IsInland { get; set; }

        public bool IsOcean { get; set; }
    }
}