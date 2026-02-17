using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityPOCOs;
namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class Opportunity
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [Column("Subject")]
	    public string Subject { get; set; }
        [ForeignKey("Customer")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Customer { get; set; }
        [ForeignKey("LeadSource")]
        [Column("LeadSourceId")]
	    public string LeadSourceId { get; set; }
	      
        public virtual LeadSource LeadSource { get; set; }
        [ForeignKey("Contact")]
        [Column("ContactId")]
	    public string ContactId { get; set; }
	      
        public virtual Contact Contact { get; set; }
        [Column("EstimatedClosingDate")]
	    public DateTime? EstimatedClosingDate { get; set; }
        [ForeignKey("Stage")]
        [Column("StageId")]
	    public string StageId { get; set; }
	      
        public virtual Stage Stage { get; set; }
        [Column("Probability")]
	    public int? Probability { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("Rating")]
        [Column("RatingCode")]
	    public string RatingCode { get; set; }
	      
        public virtual Rating Rating { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
        [Column("ActualClosingDate")]
	    public DateTime? ActualClosingDate { get; set; }
        [Column("ClosingDescription")]
	    public string ClosingDescription { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("NumberOfShipments")]
	    public int? NumberOfShipments { get; set; }
        [Column("ValueField")]
	    public decimal? ValueField { get; set; }
        [Column("LastStageDate")]
	    public DateTime? LastStageDate { get; set; }
        [ForeignKey("LastStage")]
        [Column("LastStageIdBeforeClosure")]
	    public string LastStageIdBeforeClosure { get; set; }
	      
        public virtual Stage LastStage { get; set; }
        [Column("Field1")]
	    public string Field1 { get; set; }
        [Column("Field2")]
	    public string Field2 { get; set; }
        [Column("Field3")]
	    public string Field3 { get; set; }
        [Column("Field4")]
	    public string Field4 { get; set; }
        [Column("Field5")]
	    public string Field5 { get; set; }
        [Column("Field6")]
	    public string Field6 { get; set; }
        [Column("Field7")]
	    public string Field7 { get; set; }
        [Column("Field8")]
	    public string Field8 { get; set; }
        [Column("Field9")]
	    public string Field9 { get; set; }
        [Column("Field10")]
	    public string Field10 { get; set; }
        [Column("LastCompletedActivityDate")]
	    public DateTime? LastCompletedActivityDate { get; set; }
        [Column("LeadDescription")]
	    public string LeadDescription { get; set; }
        [ForeignKey("ActivityType")]
        [Column("LastCompletedActivityTypeCode")]
	    public string LastCompletedActivityTypeCode { get; set; }
	      
        public virtual ActivityType ActivityType { get; set; }
        [Column("LastActivitySubject")]
	    public string LastActivitySubject { get; set; }
        [Column("NextActivityDate")]
	    public DateTime? NextActivityDate { get; set; }
        [ForeignKey("NextActivityType")]
        [Column("NextActivityTypeCode")]
	    public string NextActivityTypeCode { get; set; }
	      
        public virtual ActivityType NextActivityType { get; set; }
        [Column("NextActivitySubject")]
	    public string NextActivitySubject { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("StageDueDate")]
	    public DateTime? StageDueDate { get; set; }
        [ForeignKey("BusinessUnit")]
        [Column("BusinessUnitId")]
	    public string BusinessUnitId { get; set; }
	      
        public virtual BusinessUnit BusinessUnit { get; set; }
        [ForeignKey("LeadUser")]
        [Column("LeadUserId")]
	    public string LeadUserId { get; set; }
	      
        public virtual User LeadUser { get; set; }
        [ForeignKey("LeadPartner")]
        [Column("LeadPartnerId")]
	    public string LeadPartnerId { get; set; }
	      
        public virtual Card LeadPartner { get; set; }
        [ForeignKey("Agent")]
        [Column("AgentId")]
	    public string AgentId { get; set; }
	      
        public virtual Card Agent { get; set; }
        [ForeignKey("ForeignClient")]
        [Column("ForeignClientId")]
	    public string ForeignClientId { get; set; }
	      
        public virtual Card ForeignClient { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [ForeignKey("OpportunityClosingReason")]
        [Column("ClosingReasonId")]
	    public string ClosingReasonId { get; set; }
	      
        public virtual OpportunityClosingReason OpportunityClosingReason { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("OpportunityType")]
        [Column("OpportunityTypeId")]
	    public string OpportunityTypeId { get; set; }
	      
        public virtual OpportunityType OpportunityType { get; set; }
    }
}
	 