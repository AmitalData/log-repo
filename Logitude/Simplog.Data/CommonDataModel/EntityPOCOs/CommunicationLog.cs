using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CommunicationLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUTC { get; set; }

        public DateTime? DoneDate { get; set; }
        public DateTime? DoneDateUTC { get; set; }

        public DateTime LastStatusDate { get; set; }
        public DateTime LastStatusDateUTC { get; set; }

        public DateTime? NextTryDateTime { get; set; }
        public DateTime? NextTryDateTimeUTC { get; set; }

        public string CommunicationLogTypeCode { get; set; }
        public string CommunicationStatusTypeCode { get; set; }
        public string Subject { get; set; }
        public string InOut { get; set; }

        string entityid;
        public string EntityId { get { return entityid; } set { entityid = value; } }

        public string CC { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentOutId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string CreatedByUserId { get; set; }
        public string DocumentId { get; set; }
        public string To { get; set; }
        public int Retries { get; set; }
        public string BCC { get; set; }
        public string From { get; set; }
        public string SearchFields { get; set; }
        public string EntityReference { get; set; }
        public string ExceptionMessage { get; set; }
        public string Logs { get; set; }
        public string CorrelationID { get; set; }
        public string ResponseDocumentId { get; set; }

        public int Priority { get; set; }
        public string QueueName { get; set; }
        public string MessageLockId { get; set; }

        public string AWBNumber { get; set; }

        public string ReplyToList { get; set; }

        public string ChildEntityId { get; set; }
        public string ChildObjectTableId { get; set; }

        public string LogSettings { get; set; }

        public bool IsSecured { get; set; }

        public string EmailDeliveryError { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("DocumentOutId")]
        public virtual DocumentOut InternalDocument { get; set; }

        [ForeignKey("DocumentsFilingId")]
        public virtual DocumentsFiling ExternalDocument { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        
        [ForeignKey("CommunicationLogTypeCode")]
        public virtual CommunicationLogType CommunicationLogType { get; set; }

        [ForeignKey("CommunicationStatusTypeCode")]
        public virtual CommunicationStatusType CommunicationStatusType { get; set; }

        [ForeignKey("Tenant")]
        public virtual Tenant CurrentTenant { get; set; }


        [ForeignKey("ResponseDocumentId")]
        public virtual Document ResponseDocument { get; set; }
        


    }
}