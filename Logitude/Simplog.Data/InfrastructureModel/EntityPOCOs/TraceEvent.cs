using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TraceEvent
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }        
        public string EntityId { get; set; }
        public string EventTypeId { get; set; }
        public string ObjectTableId { get; set; }
        public DateTime LogDateTime { get; set; }
        public DateTime EventDateTime { get; set; }
        public bool Deleted { get; set; }
        public string ExternalId { get; set; }
        public string UserId { get; set; }
        public string CustomerCareUserEmail { get; set; }
        public string Notes { get; set; }
        public bool IsAddedManually { get; set; }
        public string Location { get; set; }
        public string PartnerName { get; set; }
        


        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        
        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }        
    }
}