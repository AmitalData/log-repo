using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.CRM.Data.EntityPOCOs
{
    public class OpportunityAnalytic
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string OwnerId { get; set; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string LeadSourceId { get; set; }
        public string ContactId { get; set; }
        public DateTime? EstimatedClosingDate { get; set; }
        public string StageId { get; set; }
        public int? Probability { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string RatingCode { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ActualClosingDate { get; set; }
        public string ClosingDescription { get; set; }
        public int? NumberOfShipments { get; set; }
        public decimal? ValueField { get; set; }
        public DateTime? LastStageDate { get; set; }
        public string LastStageIdBeforeClosure { get; set; }
        public DateTime? LastCompletedActivityDate { get; set; }
        public string LeadDescription { get; set; }
        public string LastCompletedActivityTypeCode { get; set; }
        public string LastActivitySubject { get; set; }
        public DateTime? NextActivityDate { get; set; }
        public string NextActivityTypeCode { get; set; }
        public string NextActivitySubject { get; set; }
        public DateTime? StageDueDate { get; set; }
        public string BusinessUnitId { get; set; }
        public string LeadUserId { get; set; }
        public string LeadPartnerId { get; set; }
        public string AgentId { get; set; }
        public string ForeignClientId { get; set; }
        public string ClosingReasonId { get; set; }
        public bool IsCancelled { get; set; }
        public string OpportunityTypeId { get; set; }
        public int? NumberOfConnectedQuotes { get; set; }
    }
}
