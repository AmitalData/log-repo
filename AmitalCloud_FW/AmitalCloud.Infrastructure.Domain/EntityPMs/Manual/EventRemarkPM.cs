using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    [CustomValidation(typeof(IClassLevelValidator), "ValidateClass")]
    [DataContract]
    public partial class EventRemarkPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public int Tenant { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public DateTime CreateDate { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public string EventTypeId { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public string PartnerTypeId { get; set; }
        [DataMember]
        [CustomValidation(typeof(IValidationClass), "ValidateClass")]
        public bool IsChoose { get; set; }
    }


}
