using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class ActivityMap : EntityTypeConfiguration<Activity>
    {
	    string dbms;
        public ActivityMap()
        { 
				this.ToTable("Activities");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ActivityTypeCode).HasColumnName("ActivityTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Subject).HasColumnName("Subject").IsRequired().HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.DueDate).HasColumnName("DueDate");

            this.Property(t => t.PriorityCode).HasColumnName("PriorityCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");

            this.Property(t => t.EndDateTime).HasColumnName("EndDateTime");

            this.Property(t => t.Description).HasColumnName("Description").IsMaxLength().IsUnicode(true);

            this.Property(t => t.CallTypeCode).HasColumnName("CallTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.CallPurpose).HasColumnName("CallPurpose").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.CallDetails).HasColumnName("CallDetails").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.CallResult).HasColumnName("CallResult").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Duration).HasColumnName("Duration");

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.ActivityStatusCode).HasColumnName("ActivityStatusCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.BranchId).HasColumnName("BranchId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AllDayEvent).HasColumnName("AllDayEvent").IsRequired();

            this.Property(t => t.ActivityTimeTypeCode).HasColumnName("ActivityTimeTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Location).HasColumnName("Location").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.IsLeftVoiceMail).HasColumnName("IsLeftVoiceMail").IsRequired();

            this.Property(t => t.OutlookId).HasColumnName("OutlookId").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.NeedSynchronization).HasColumnName("NeedSynchronization").IsRequired();

            this.Property(t => t.IsOpen).HasColumnName("IsOpen").IsRequired();

            this.Property(t => t.MeetingSummary).HasColumnName("MeetingSummary").IsMaxLength().IsUnicode(true);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Field1).HasColumnName("Field1").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field2).HasColumnName("Field2").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field3).HasColumnName("Field3").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field4).HasColumnName("Field4").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field5).HasColumnName("Field5").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field6).HasColumnName("Field6").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field7).HasColumnName("Field7").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field8).HasColumnName("Field8").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field9).HasColumnName("Field9").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field10).HasColumnName("Field10").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.CompleteDate).HasColumnName("CompleteDate");

            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.SenderEmail).HasColumnName("SenderEmail").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SenderContactId).HasColumnName("SenderContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CallWithId).HasColumnName("CallWithId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.QuoteId).HasColumnName("QuoteId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TicketId).HasColumnName("TicketId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SortingDate).HasColumnName("SortingDate");

            this.Property(t => t.SortingBy).HasColumnName("SortingBy").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.SendReceiveDate).HasColumnName("SendReceiveDate");

            this.Property(t => t.ActivityWith).HasColumnName("ActivityWith").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.DescriptionRightToLeft).HasColumnName("DescriptionRightToLeft");

            this.Property(t => t.MeetingSummaryRightToLeft).HasColumnName("MeetingSummaryRightToLeft");

            this.Property(t => t.From).HasColumnName("From").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.To).HasColumnName("To").HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.Cc).HasColumnName("Cc").HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.DueDateOffset).HasColumnName("DueDateOffset");

            this.Property(t => t.DueDateDateField).HasColumnName("DueDateDateField").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.BusinessProcessQueueId).HasColumnName("BusinessProcessQueueId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TeamId).HasColumnName("TeamId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TestProperty).HasColumnName("TestProperty").IsRequired();
        }
    }
}
	 