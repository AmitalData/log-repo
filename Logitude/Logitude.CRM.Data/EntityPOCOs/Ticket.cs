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
   
    public class Ticket
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("TicketNumber")]
	    public string TicketNumber { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [ForeignKey("CreatedByContact")]
        [Column("CreatedByContactId")]
	    public string CreatedByContactId { get; set; }
	      
        public virtual Contact CreatedByContact { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("Company")]
        [Column("CompanyId")]
	    public string CompanyId { get; set; }
	      
        public virtual Card Company { get; set; }
        [ForeignKey("Contact")]
        [Column("ContactId")]
	    public string ContactId { get; set; }
	      
        public virtual Contact Contact { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("MainClassification")]
        [Column("MainClassificationId")]
	    public string MainClassificationId { get; set; }
	      
        public virtual TicketClassification MainClassification { get; set; }
        [ForeignKey("Owner")]
        [Column("OwnerId")]
	    public string OwnerId { get; set; }
	      
        public virtual User Owner { get; set; }
        [ForeignKey("Stage")]
        [Column("StageId")]
	    public string StageId { get; set; }
	      
        public virtual TicketStage Stage { get; set; }
        [ForeignKey("Severity")]
        [Column("SeverityId")]
	    public string SeverityId { get; set; }
	      
        public virtual TicketSeverity Severity { get; set; }
        [Column("Subject")]
	    public string Subject { get; set; }
        [ForeignKey("TicketType")]
        [Column("TicketTypeId")]
	    public string TicketTypeId { get; set; }
	      
        public virtual TicketType TicketType { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("IsClosed")]
	    public bool IsClosed { get; set; }
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
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("TicketDescription")]
	    public string TicketDescription { get; set; }
        [ForeignKey("NextActivityType")]
        [Column("NextActivityTypeCode")]
	    public string NextActivityTypeCode { get; set; }
	      
        public virtual ActivityType NextActivityType { get; set; }
        [Column("NextActivitySubject")]
	    public string NextActivitySubject { get; set; }
        [Column("NextActivityDate")]
	    public DateTime? NextActivityDate { get; set; }
        [ForeignKey("ActivityType")]
        [Column("LastCompletedActivityTypeCode")]
	    public string LastCompletedActivityTypeCode { get; set; }
	      
        public virtual ActivityType ActivityType { get; set; }
        [Column("LastCompletedActivitySubject")]
	    public string LastCompletedActivitySubject { get; set; }
        [Column("LastCompletedActivityDate")]
	    public DateTime? LastCompletedActivityDate { get; set; }
        [Column("CCs")]
	    public string CCs { get; set; }
        [Column("Bcc")]
	    public string Bcc { get; set; }
        [ForeignKey("BusinessUnit")]
        [Column("BusinessUnitId")]
	    public string BusinessUnitId { get; set; }
	      
        public virtual BusinessUnit BusinessUnit { get; set; }
        [ForeignKey("EmployeeGroup")]
        [Column("EmployeeGroupId")]
	    public string EmployeeGroupId { get; set; }
	      
        public virtual EmployeeGroup EmployeeGroup { get; set; }
        [Column("FirstResponseTime")]
	    public DateTime? FirstResponseTime { get; set; }
        [Column("FirstResponseDue")]
	    public DateTime? FirstResponseDue { get; set; }
        [Column("FullResolvedTime")]
	    public DateTime? FullResolvedTime { get; set; }
        [Column("ResolveWithinDue")]
	    public DateTime? ResolveWithinDue { get; set; }
        [Column("GuidId")]
	    public string GuidId { get; set; }
        [Column("OpenEscalation")]
	    public int OpenEscalation { get; set; }
        [Column("InternalUsers")]
	    public string InternalUsers { get; set; }
        [Column("ClosureDescription")]
	    public string ClosureDescription { get; set; }
        [Column("Closewithoutnotifying")]
	    public bool Closewithoutnotifying { get; set; }
        [ForeignKey("SecondaryClassification")]
        [Column("SecondaryClassificationId")]
	    public string SecondaryClassificationId { get; set; }
	      
        public virtual TicketClassification SecondaryClassification { get; set; }
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
        [Column("ShipmentNumber")]
	    public string ShipmentNumber { get; set; }
        [Column("FirstResolveDate")]
	    public DateTime? FirstResolveDate { get; set; }
        [ForeignKey("TicketSource")]
        [Column("Source")]
	    public string Source { get; set; }
	      
        public virtual TicketSource TicketSource { get; set; }
        [ForeignKey("TicketCreatedByType")]
        [Column("CreatedbyType")]
	    public string CreatedbyType { get; set; }
	      
        public virtual TicketCreatedByType TicketCreatedByType { get; set; }
        [Column("FirstCloseDate")]
	    public DateTime? FirstCloseDate { get; set; }
        [Column("LastCloseDate")]
	    public DateTime? LastCloseDate { get; set; }
        [Column("OpenDate")]
	    public DateTime? OpenDate { get; set; }
        [Column("OpenPeriodMinutes")]
	    public int? OpenPeriodMinutes { get; set; }
        [Column("InternalMode")]
	    public bool InternalMode { get; set; }
        [ForeignKey("CustomerContact")]
        [Column("CustomerContactId")]
	    public string CustomerContactId { get; set; }
	      
        public virtual Contact CustomerContact { get; set; }
        [ForeignKey("Quote")]
        [Column("QuoteId")]
	    public string QuoteId { get; set; }
	      
        public virtual Quote Quote { get; set; }
        [Column("QuoteNumber")]
	    public string QuoteNumber { get; set; }
        [Column("SLAId")]
	    public string SLAId { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("EntityType")]
	    public string EntityType { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("SupportMailbox")]
        [Column("SupportMailboxId")]
	    public string SupportMailboxId { get; set; }
	      
        public virtual SupportMailbox SupportMailbox { get; set; }
        [Column("LastCorrespondence")]
	    public string LastCorrespondence { get; set; }
        [Column("QuoteRequestFeedback")]
	    public string QuoteRequestFeedback { get; set; }
        [Column("QuoteRequestComments")]
	    public string QuoteRequestComments { get; set; }
    }
}
	 