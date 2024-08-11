using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class FollowUpMap : EntityTypeConfiguration<FollowUp>
    {
        public FollowUpMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.JobId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.DocumentsFilingId).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.InternalDocumentId).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.LegType).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ShipmentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EventTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QuoteId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OwnerUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DocumentTypeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AutomationId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Area).HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.DateFieldName).HasMaxLength(240).IsUnicode(true);
            this.Property(t => t.DateEscalationActionTimeIndicatorCode).HasMaxLength(2).IsUnicode(false);
            
            

            this.Ignore(d => d.IsNew);
            this.Ignore(d => d.DoneNote);
            this.Ignore(d => d.DoneDateTime);
            this.Ignore(d => d.Done);

            // Table & Column Mappings
            this.ToTable("FollowUps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Date).HasColumnName("FollowUpDate");
                this.Property(t => t.DateEscalationActionTimeIndicatorCode).HasColumnName("DateEscalationActionTimeCode");
            }
            //#else
            else
            {
                this.Property(t => t.Date).HasColumnName("Date");
                this.Property(t => t.DateEscalationActionTimeIndicatorCode).HasColumnName("DateEscalationActionTimeIndicatorCode");
            }
            
//#endif
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.DocumentsFilingId).HasColumnName("DocumentsFilingId");
            this.Property(t => t.InternalDocumentId).HasColumnName("InternalDocumentId");
            this.Property(t => t.LegType).HasColumnName("LegType");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.EventTypeId).HasColumnName("EventTypeId");
            this.Property(t => t.QuoteId).HasColumnName("QuoteId");
            this.Property(t => t.OwnerUserId).HasColumnName("OwnerUserId");

            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.AutomationId).HasColumnName("AutomationId");
            this.Property(t => t.Area).HasColumnName("Area");
            this.Property(t => t.DateEscalationTime).HasColumnName("DateEscalationTime");
        
            this.Property(t => t.DateFieldName).HasColumnName("DateFieldName");


            // Relationships
            this.HasOptional(t => t.ExternalDocument).WithMany().HasForeignKey(d => d.DocumentsFilingId);
            this.HasOptional(t => t.InternalDocument).WithMany().HasForeignKey(d => d.InternalDocumentId);
            this.HasOptional(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
            this.HasRequired(t => t.OwnerUser).WithMany().HasForeignKey(d => d.OwnerUserId);
            this.HasOptional(t => t.Quote).WithMany().HasForeignKey(d => d.QuoteId);
            this.HasOptional(t => t.DocumentType).WithMany().HasForeignKey(d => d.DocumentTypeId);
            this.HasOptional(t => t.Automation).WithMany().HasForeignKey(d => d.AutomationId);
        }
    }
}
