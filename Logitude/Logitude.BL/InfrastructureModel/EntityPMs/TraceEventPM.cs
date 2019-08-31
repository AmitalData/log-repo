using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class TraceEventPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [DataMember]
        public string ObjectTableId { get; set; }

        [DataMember]
        public string EntityId { get; set; }

        [DataMember]
        public string EntityNumber { get; set; }


        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]        
        public DateTime EventDateTime { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime LogDateTime { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeEnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeLocalName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ContactEnglishFirstName { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public bool ShortView { get; set; }

        [DataMember]
        public bool IsManualEntry { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeCategoryCode { get; set; }

        [DataMember]
        public string PartnerName { get; set; }
        
        [DataMember]
        public bool IsAgentView { get; set; }

        [DataMember]
        public bool IsCustomerView { get; set; }

        [DataMember]
        public bool IsAddedManually { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerCareUserEmail { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Location { get; set; }

        //[DataMember]
        //[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        //public string EventDateTime1 { get; set; }

        //[DataMember]
        //[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        //public string EventDateTime2 { get; set; }
    }
}