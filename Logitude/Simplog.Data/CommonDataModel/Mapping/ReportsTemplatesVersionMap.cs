

using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportsTemplatesVersionMap : EntityTypeConfiguration<ReportsTemplatesVersion>
    {
        public ReportsTemplatesVersionMap()
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


            this.Property(t => t.TemplateId)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsRequired();

            this.Property(t => t.ReportId)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsRequired();

            this.Property(t => t.ReportDocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);
                

            // Table & Column Mappings
            this.ToTable("ReportsTemplatesVersions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.ReportId).HasColumnName("ReportId");
            this.Property(t => t.ReportDocumentId).HasColumnName("ReportDocumentId");
            this.Property(t => t.IsRestored).HasColumnName("IsRestored");
            // Relationships


            this.HasOptional(t => t.Document)
            .WithMany()
            .HasForeignKey(d => d.ReportDocumentId);


            this.HasRequired(t => t.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId);
             
            this.HasRequired(t => t.ReportsTemplate)
                .WithMany()
                .HasForeignKey(d => d.TemplateId);

            this.HasRequired(t => t.Report)
                .WithMany()
                .HasForeignKey(d => d.ReportId);


            this.HasRequired(t => t.CreatedByUser)
             .WithMany()
             .HasForeignKey(d => d.CreatedByUserId);


        }
    }
}
