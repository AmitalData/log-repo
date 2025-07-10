
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportsTemplateMap : EntityTypeConfiguration<ReportsTemplate>
    {
        public ReportsTemplateMap()
        {
            
            
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsRequired();

            this.Property(t => t.UpdatedByUserId)
             .HasMaxLength(15)
             .IsUnicode(false)
             .IsRequired();

            this.Property(t => t.Description).IsRequired().IsUnicode(true).HasMaxLength(250);


            this.Property(t => t.ReportId)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsRequired();


            this.Property(t => t.TemplateType)
                .HasMaxLength(1)
                .IsUnicode(false);
            
            this.Property(t => t.From)
                .HasMaxLength(500)
                .IsUnicode(true);


            this.Property(t => t.ReplyTo)
                .HasMaxLength(500)
                .IsUnicode(true);
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.CC)
                .HasMaxLength(500)
                .IsUnicode(true);

            }
            else
            {
                this.Property(t => t.CC)
                .HasMaxLength(4000)
                .IsUnicode(true);

            }

            this.Property(t => t.Subject)
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.ObjectTableId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ReportsTemplates");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CurrentVersion).HasColumnName("CurrentVersion");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            this.Property(t => t.ReportId).HasColumnName("ReportId");

            this.Property(t => t.TemplateType).HasColumnName("TemplateType");


            
            this.Property(t => t.ReplyTo).HasColumnName("ReplyTo");
            this.Property(t => t.CC).HasColumnName("CC");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.IsSystemReportFixed).HasColumnName("IsSystemReportFixed");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.IsCopiedAtSignup).HasColumnName("IsCopiedAtSignup");
            this.Property(t => t.OriginalTemplateId).HasColumnName("OriginalTemplateId");


            //#if ORACLE_DB
            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.From).HasColumnName("From1");
            }
            else
            {
                //#else
                this.Property(t => t.From).HasColumnName("From");
            }

            //#endif

            // Relationships

            this.HasRequired(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId);

            this.HasRequired(t => t.Report)
                .WithMany()
                .HasForeignKey(d => d.ReportId);


            this.HasRequired(t => t.CreatedByUser)
             .WithMany()
             .HasForeignKey(d => d.CreatedByUserId);


        }
    }
}
