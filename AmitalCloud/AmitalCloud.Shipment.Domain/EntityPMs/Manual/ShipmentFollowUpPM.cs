using AmitalCloud.Shipment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{
    [CustomValidation(typeof(IShipmentClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(IShipmentDeliveryValidator), "IsShipmentDeliveryValid")]
    public partial class ShipmentFollowUpPM : ChildEntitiesCustomFieldPM
    {
        public ShipmentFollowUpPM(FollowUp follow)
        {
            Tenant = follow.Tenant;
                    Date = follow.Date;
            //Done = follow.Done;
            //DoneDateTime = follow.DoneDateTime;
            //DoneNote = follow.DoneNote;
            ExternalDocumentId = follow.DocumentsFilingId;
                    Id = follow.Id;
                    InternalDocumentId = follow.InternalDocumentId;
                    //IsNew = follow.IsNew;
                    JobId = follow.JobId;
                    LegType = follow.LegType;
                    Notes = follow.Notes;
                    ShipmentId = follow.ShipmentId;
                    EventTypeId = follow.EventTypeId;
                    EventTypeFollowUpName = follow.EventType.FollowUpEnglishName;
                    ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp;
                    OwnerUserId = follow.OwnerUserId;
                    OwnerUserName = follow.OwnerUser.Contact.EnglishName;
                    Area = follow.Area;
                    DocumentTypeId = follow.DocumentTypeId;
                    AutomationId = follow.AutomationId;
                    DateEscalationActionTimeIndicatorCode = follow.DateEscalationActionTimeIndicatorCode;
                    DateEscalationTime = follow.DateEscalationTime;
                    DateFieldName = follow.DateFieldName;
        }

        [Key]
        public string Id { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string ShipmentId { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string JobId { get; set; }

        public int Tenant { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string ExternalDocumentId { get; set; }

        private DateTime? date;

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
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

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public bool IsNew { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string Notes { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string DoneNote { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public DateTime? DoneDateTime { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public bool Done { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string InternalDocumentId { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string LegType { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public bool Deleted { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string EntityDateId { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string EventTypeId { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string EventTypeFollowUpName { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public bool ManualActivatedFollowUp { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string OwnerUserId { get; set; }

        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string OwnerUserName { get; set; }


        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string Area { get; set; }
        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string DocumentTypeId { get; set; }
        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string AutomationId { get; set; }




        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string DateEscalationActionTimeIndicatorCode { get; set; }



        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public int DateEscalationTime { get; set; }


        [CustomValidation(typeof(IShipmentValidationClass), "ValidateClass")]
        public string DateFieldName { get; set; }
        
        //public ChangeSetOperation ChangeSetOp { get; set; }
    }
}