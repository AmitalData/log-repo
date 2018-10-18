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
   
    public class Activity
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("ActivityType")]
        [Column("ActivityTypeCode")]
	    public string ActivityTypeCode { get; set; }
	      
        public virtual ActivityType ActivityType { get; set; }
        [Column("Subject")]
	    public string Subject { get; set; }
        [Column("DueDate")]
	    public DateTime? DueDate { get; set; }
        [ForeignKey("ActivityPriority")]
        [Column("PriorityCode")]
	    public string PriorityCode { get; set; }
	      
        public virtual ActivityPriority ActivityPriority { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [Column("StartDateTime")]
	    public DateTime? StartDateTime { get; set; }
        [Column("EndDateTime")]
	    public DateTime? EndDateTime { get; set; }
        [Column("Description")]
	    public string Description { get; set; }
        [ForeignKey("CallType")]
        [Column("CallTypeCode")]
	    public string CallTypeCode { get; set; }
	      
        public virtual CallType CallType { get; set; }
        [Column("CallPurpose")]
	    public string CallPurpose { get; set; }
        [Column("CallDetails")]
	    public string CallDetails { get; set; }
        [Column("CallResult")]
	    public string CallResult { get; set; }
        [Column("PhoneNumber")]
	    public string PhoneNumber { get; set; }
        [Column("Duration")]
	    public int? Duration { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("ActivityStatus")]
        [Column("ActivityStatusCode")]
	    public string ActivityStatusCode { get; set; }
	      
        public virtual ActivityStatus ActivityStatus { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("Branch")]
        [Column("BranchId")]
	    public string BranchId { get; set; }
	      
        public virtual Branch Branch { get; set; }
        [Column("AllDayEvent")]
	    public bool AllDayEvent { get; set; }
        [ForeignKey("ActivityTimeType")]
        [Column("ActivityTimeTypeCode")]
	    public string ActivityTimeTypeCode { get; set; }
	      
        public virtual ActivityTimeType ActivityTimeType { get; set; }
        [Column("Location")]
	    public string Location { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("IsLeftVoiceMail")]
	    public bool IsLeftVoiceMail { get; set; }
        [Column("OutlookId")]
	    public string OutlookId { get; set; }
        [Column("NeedSynchronization")]
	    public bool NeedSynchronization { get; set; }
        [Column("IsOpen")]
	    public bool IsOpen { get; set; }
        [Column("MeetingSummary")]
	    public string MeetingSummary { get; set; }
        [ForeignKey("Customer")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Customer { get; set; }
        [ForeignKey("Opportunity")]
        [Column("OpportunityId")]
	    public string OpportunityId { get; set; }
	      
        public virtual Opportunity Opportunity { get; set; }
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
        [Column("CompleteDate")]
	    public DateTime? CompleteDate { get; set; }
        [ForeignKey("BusinessUnit")]
        [Column("BusinessUnitId")]
	    public string BusinessUnitId { get; set; }
	      
        public virtual BusinessUnit BusinessUnit { get; set; }
        [Column("SenderEmail")]
	    public string SenderEmail { get; set; }
        [ForeignKey("CommunicationLog")]
        [Column("CommunicationLogId")]
	    public string CommunicationLogId { get; set; }
	      
        public virtual CommunicationLog CommunicationLog { get; set; }
        [ForeignKey("SenderContact")]
        [Column("SenderContactId")]
	    public string SenderContactId { get; set; }
	      
        public virtual Contact SenderContact { get; set; }
        [ForeignKey("CallWith")]
        [Column("CallWithId")]
	    public string CallWithId { get; set; }
	      
        public virtual Contact CallWith { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [ForeignKey("Quote")]
        [Column("QuoteId")]
	    public string QuoteId { get; set; }
	      
        public virtual Quote Quote { get; set; }
        [ForeignKey("Ticket")]
        [Column("TicketId")]
	    public string TicketId { get; set; }
	      
        public virtual Ticket Ticket { get; set; }
        [Column("SortingDate")]
	    public DateTime? SortingDate { get; set; }
        [Column("SortingBy")]
	    public string SortingBy { get; set; }
        [Column("SendReceiveDate")]
	    public DateTime? SendReceiveDate { get; set; }
        [Column("ActivityWith")]
	    public string ActivityWith { get; set; }
        [Column("DescriptionRightToLeft")]
	    public bool DescriptionRightToLeft { get; set; }
        [Column("MeetingSummaryRightToLeft")]
	    public bool MeetingSummaryRightToLeft { get; set; }
        [Column("From")]
	    public string From { get; set; }
        [Column("To")]
	    public string To { get; set; }
        [Column("Cc")]
	    public string Cc { get; set; }
        [Column("DueDateOffset")]
	    public int? DueDateOffset { get; set; }
        [Column("DueDateDateField")]
	    public string DueDateDateField { get; set; }
        [ForeignKey("BusinessProcessQueue")]
        [Column("BusinessProcessQueueId")]
	    public string BusinessProcessQueueId { get; set; }
	      
        public virtual BusinessProcessQueue BusinessProcessQueue { get; set; }
        [ForeignKey("Team")]
        [Column("TeamId")]
	    public string TeamId { get; set; }
	      
        public virtual Team Team { get; set; }
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
    }
}
	 