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
 
    public class OpportunityMap : EntityTypeConfiguration<Opportunity>
    {
	    string dbms;
        public OpportunityMap()
        { 
				this.ToTable("Opportunities");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.OwnerId).HasColumnName("OwnerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Subject).HasColumnName("Subject").IsRequired().HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LeadSourceId).HasColumnName("LeadSourceId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ContactId).HasColumnName("ContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EstimatedClosingDate).HasColumnName("EstimatedClosingDate");

            this.Property(t => t.StageId).HasColumnName("StageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Probability).HasColumnName("Probability");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RatingCode).HasColumnName("RatingCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsClosed).HasColumnName("IsClosed").IsRequired();

            this.Property(t => t.ActualClosingDate).HasColumnName("ActualClosingDate");

            this.Property(t => t.ClosingDescription).HasColumnName("ClosingDescription").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.NumberOfShipments).HasColumnName("NumberOfShipments");

            this.Property(t => t.ValueField).HasColumnName("ValueField").HasPrecision(18, 2);

            this.Property(t => t.LastStageDate).HasColumnName("LastStageDate");

            this.Property(t => t.LastStageIdBeforeClosure).HasColumnName("LastStageIdBeforeClosure").HasMaxLength(15).IsUnicode(false);

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

            this.Property(t => t.LastCompletedActivityDate).HasColumnName("LastCompletedActivityDate");

            this.Property(t => t.LeadDescription).HasColumnName("LeadDescription").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.LastCompletedActivityTypeCode).HasColumnName("LastCompletedActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LastActivitySubject).HasColumnName("LastActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.NextActivityDate).HasColumnName("NextActivityDate");

            this.Property(t => t.NextActivityTypeCode).HasColumnName("NextActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.NextActivitySubject).HasColumnName("NextActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.Notes).HasColumnName("Notes").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StageDueDate).HasColumnName("StageDueDate");

            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.LeadUserId).HasColumnName("LeadUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LeadPartnerId).HasColumnName("LeadPartnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AgentId).HasColumnName("AgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ForeignClientId).HasColumnName("ForeignClientId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ClosingReasonId).HasColumnName("ClosingReasonId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.OpportunityTypeId).HasColumnName("OpportunityTypeId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 