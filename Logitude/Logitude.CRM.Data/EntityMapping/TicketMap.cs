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
 
    public class TicketMap : EntityTypeConfiguration<Ticket>
    {
	    string dbms;
        public TicketMap()
        { 
				this.ToTable("Tickets");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.TicketNumber).HasColumnName("TicketNumber").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByContactId).HasColumnName("CreatedByContactId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.CompanyId).HasColumnName("CompanyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContactId).HasColumnName("ContactId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainClassificationId).HasColumnName("MainClassificationId").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StageId).HasColumnName("StageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SeverityId).HasColumnName("SeverityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Subject).HasColumnName("Subject").IsRequired().HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.TicketTypeId).HasColumnName("TicketTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled").IsRequired();

            this.Property(t => t.IsClosed).HasColumnName("IsClosed").IsRequired();

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

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.TicketDescription).HasColumnName("TicketDescription").IsRequired().IsMaxLength().IsUnicode(true);

            this.Property(t => t.NextActivityTypeCode).HasColumnName("NextActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.NextActivitySubject).HasColumnName("NextActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.NextActivityDate).HasColumnName("NextActivityDate");

            this.Property(t => t.LastCompletedActivityTypeCode).HasColumnName("LastCompletedActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LastCompletedActivitySubject).HasColumnName("LastCompletedActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.LastCompletedActivityDate).HasColumnName("LastCompletedActivityDate");

            this.Property(t => t.CCs).HasColumnName("CCs").HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.Bcc).HasColumnName("Bcc").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.EmployeeGroupId).HasColumnName("EmployeeGroupId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FirstResponseTime).HasColumnName("FirstResponseTime");

            this.Property(t => t.FirstResponseDue).HasColumnName("FirstResponseDue");

            this.Property(t => t.FullResolvedTime).HasColumnName("FullResolvedTime");

            this.Property(t => t.ResolveWithinDue).HasColumnName("ResolveWithinDue");

            this.Property(t => t.GuidId).HasColumnName("GuidId").HasMaxLength(22).IsUnicode(false);

            this.Property(t => t.OpenEscalation).HasColumnName("OpenEscalation");

            this.Property(t => t.InternalUsers).HasColumnName("InternalUsers").HasMaxLength(4000).IsUnicode(false);

            this.Property(t => t.ClosureDescription).HasColumnName("ClosureDescription").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Closewithoutnotifying).HasColumnName("Closewithoutnotifying");

            this.Property(t => t.SecondaryClassificationId).HasColumnName("SecondaryClassificationId").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentNumber).HasColumnName("ShipmentNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.FirstResolveDate).HasColumnName("FirstResolveDate");

            this.Property(t => t.Source).HasColumnName("Source").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CreatedbyType).HasColumnName("CreatedbyType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.FirstCloseDate).HasColumnName("FirstCloseDate");

            this.Property(t => t.LastCloseDate).HasColumnName("LastCloseDate");

            this.Property(t => t.OpenDate).HasColumnName("OpenDate");

            this.Property(t => t.OpenPeriodMinutes).HasColumnName("OpenPeriodMinutes");

            this.Property(t => t.InternalMode).HasColumnName("InternalMode");

            this.Property(t => t.CustomerContactId).HasColumnName("CustomerContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteId).HasColumnName("QuoteId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteNumber).HasColumnName("QuoteNumber").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.SLAId).HasColumnName("SLAId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityType).HasColumnName("EntityType").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 