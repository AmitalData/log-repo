using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class EventTypePM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }
                
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }

        [DataMember]
        public bool AddedManually { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]     
        public string EnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]       
        public string LocalName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedLocalName { get; set; }

        [DataMember]
        public bool IsManualEntry { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityStatusId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ObjectTableId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventGroupCode { get; set; }

        [DataMember]
        public bool ShortView { get; set; }

        [DataMember]
        public bool IsFollowUp { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FollowUpEnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FollowUpLocalName { get; set; }

        [DataMember]
        public bool ManualActivatedFollowUp { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityStatusName { get; set; }

        [DataMember]
        public bool InActive { get; set; }

        [DataMember]
        public string SearchFields { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerRoleId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AgentRoleId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeCategoryCode { get; set; }

        [DataMember]
        public bool IsCustomerView { get; set; }

        [DataMember]
        public bool IsAgentView { get; set; }

        [DataMember]
        public bool IsSharedLogisticsEnabled { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ObjectTableName { get; set; }

        [DataMember]
        public bool IsHybrid { get; set; }

        [DataMember]
        public bool AllowedInAutomation { get; set; }


        [DataMember]
        public string CustomField { get; set; }

        [DataMember]
        public bool IsStatusNotModified { get; set; }

        public string PartnerCode { get; set; }
    }
}