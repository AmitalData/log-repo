using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
     [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class CommunicationLogPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        public DateTime? DoneDate { get; set; }
        public string CommunicationLogTypeCode { get; set; }
        public string CommunicationStatusTypeCode { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "The length of the subject must be less than 500 character!")]
        public string Subject { get; set; }

        public string InOut { get; set; }
        public string EntityId { get; set; }
        public string CC { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentInId { get; set; }
        public string DocumentOutId { get; set; }
        public string CreatedByUserId { get; set; }
        public string DocumentId { get; set; }
        public string To { get; set; }
        public int Retries { get; set; }
        public string CommunicationLogTypeName { get; set; }
        public string CommunicationStatusTypeName { get; set; }
        public string BCC { get; set; }
        public string From { get; set; }
        public DateTime LastStatusDate { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string ObjectTableName { get; set; }
        public string SearchFields { get; set; }
        public string EntityReference { get; set; }

        public bool IsWaiting { get; set; }
        public string ExceptionMessage { get; set; }

        public string Logs { get; set; }
        public string CorrelationID { get; set; }

        public DateTime? NextTryDateTime { get; set; }

        public DateTime CreateDateUTC { get; set; }
        public DateTime? DoneDateUTC { get; set; }
        public DateTime LastStatusDateUTC { get; set; }
        public DateTime? NextTryDateTimeUTC { get; set; }

        public int Priority { get; set; }
        public string QueueName { get; set; }
        public string MessageLockId { get; set; }

        public string TenantName { get; set; }
        public string AWBNumber { get; set; }

        public string ReplyToList { get; set; }

        public string ChildEntityId { get; set; }
        public string ChildObjectTableId { get; set; }
        public string SecurityId { get; set; }
        public string LogSettings { get; set; }
        public bool IsBodySecured { get; set; }

        public string EmailDeliveryError { get; set; }
        public string ResponseDocumentId { get; set; }
        public string UniqueNumber { get; set; }
        public bool? WasAnalyzed { get; set; }
    }
}
