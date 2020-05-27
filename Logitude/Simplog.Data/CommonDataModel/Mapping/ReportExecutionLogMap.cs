using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ReportExecutionLogMap : EntityTypeConfiguration<ReportExecutionLog>
    {

        public ReportExecutionLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);


            this.Property(t => t.ReportId)
                 .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.ReportTemplateId)
                 .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.StatusCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ExceptionMessage)
                .HasMaxLength(8000)
                .IsUnicode(true);


            this.Property(t => t.ReportFilterXML)
                .IsMaxLength()
                .IsUnicode(true);


            this.Property(t => t.ExecutedByServerName)
                .HasMaxLength(100)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("ReportExecutionLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage");
            this.Property(t => t.ReportFilterXML).HasColumnName("ReportFilterXML");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.ReportId).HasColumnName("ReportId");
            this.Property(t => t.ReportTemplateId).HasColumnName("ReportTemplateId");

            this.Property(t => t.RetryNumber).HasColumnName("RetryNumber");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.ExecutedByServerName).HasColumnName("ExecutedByServerName");
            this.Property(t => t.DisablePreview).HasColumnName("DisablePreview");




            // Relationships

            this.HasRequired(t => t.CommunicationStatusType)
                .WithMany()
                .HasForeignKey(d => d.StatusCode);

            this.HasRequired(t => t.CreatedByUser)
             .WithMany()
             .HasForeignKey(d => d.CreatedByUserId);


        }
    }
}
