using Simplog.Server.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ShipmentFollowUpPM
    {
        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string JobId { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalDocumentId { get; set; }

        private DateTime? date;

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value != null)
                {
                    date = value.Value;
                }
            }
        }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsNew { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DoneNote { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? DoneDateTime { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool Done { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InternalDocumentId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LegType { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool Deleted { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EntityDateId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeFollowUpName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool ManualActivatedFollowUp { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OwnerUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string OwnerUserName { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Area { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DocumentTypeId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AutomationId { get; set; }




        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DateEscalaActTimeIndicatorCode { get; set; }



        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int DateEscalationTime { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string DateFieldName { get; set; }
        
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}