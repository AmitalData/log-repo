using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class FollowUp
    {
        [Key]
        public string Id { get; set; }
        public string ShipmentId { get; set; }
        public string JobId { get; set; }
        public int Tenant { get; set; }
        public string DocumentsFilingId { get; set; }

        private DateTime? date;
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
        public bool IsNew { get; set; }
        public string  Notes { get; set; }
        public string DoneNote { get; set; }
        public DateTime? DoneDateTime { get; set; }
        public bool Done { get; set; }
        public string InternalDocumentId { get; set; }
        public string LegType { get; set; }
        public string EventTypeId { get; set; }
        public string QuoteId { get; set; }
        public string OwnerUserId { get; set; }
        public string Area { get; set; }
        public string DocumentTypeId { get; set; }
        public string AutomationId { get; set; }

        public string DateEscalationActionTimeIndicatorCode { get; set; }
        public int DateEscalationTime { get; set; }
        public string DateFieldName { get; set; }

        [ForeignKey("OwnerUserId")]
        public virtual User OwnerUser { get; set; }

        [ForeignKey("QuoteId")]
        public virtual Quote Quote { get; set; }

        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }

        [ForeignKey("InternalDocumentId")]
        public DocumentOut InternalDocument { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("ExternalDocumentId")]
        public DocumentsFiling ExternalDocument { get; set; }


        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentType { get; set; }

        [ForeignKey("AutomationId")]
        public Automation Automation { get; set; }

    }
}