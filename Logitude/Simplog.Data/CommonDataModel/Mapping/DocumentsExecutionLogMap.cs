using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{


    public class DocumentsExecutionLogMap : EntityTypeConfiguration<DocumentsExecutionLog>
    {
        public DocumentsExecutionLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);


            this.Property(t => t.DocumentTypeId)
                 .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.DocumentTypeTemplateId)
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


            this.Property(t => t.RequestXML)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(true);


            this.Property(t => t.Logs)
                .HasMaxLength(8000)
                .IsUnicode(true);

            this.Property(t => t.ExecutedByServerName)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DocumentsExecutionLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StatusCode).HasColumnName("StatusCode");
            this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage");
            this.Property(t => t.RequestXML).HasColumnName("RequestXML");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.DocumentTypeTemplateId).HasColumnName("DocumentTypeTemplateId");

            this.Property(t => t.RetryNumber).HasColumnName("RetryNumber");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.Logs).HasColumnName("Logs");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.ExecutedByServerName).HasColumnName("ExecutedByServerName");

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
