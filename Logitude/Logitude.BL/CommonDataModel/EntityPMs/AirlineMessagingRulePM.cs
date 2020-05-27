using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class AirlineMessagingRulePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string RuleFieldId { get; set; }
        public bool IsMandatoryForSending { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string RuleFieldCode { get; set; }
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MessageTypeCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? MaxSize { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AirlineId { get; set; }
    }
}
