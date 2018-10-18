using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CommunicationLogStepMap : EntityTypeConfiguration<CommunicationLogStep>
    {
        public CommunicationLogStepMap()
        {
            // Primary Key
            this.HasKey(t => new { t.StepNumber, t.CommunicationLogId });

            // Properties
            this.Property(t => t.CommunicationLogId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Log)
                .HasMaxLength(8000)
                .IsUnicode(true);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            //this.Property(t => t.ParamIn1)
            //    .HasMaxLength(1000)
            //    .IsUnicode(false);

            //this.Property(t => t.ParamOut1)
            //    .HasMaxLength(1000)
            //    .IsUnicode(false);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.DocumentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
            this.Property(t => t.IsLogCompress);





            // Table & Column Mappings
            this.ToTable("CommunicationLogSteps");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.StepNumber).HasColumnName("StepNumber");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Log).HasColumnName("Log");
            this.Property(t => t.Name).HasColumnName("Name");
            //itzik this.Property(t => t.ParamIn1).HasColumnName("ParamIn1");
            //itzik this.Property(t => t.ParamOut1).HasColumnName("ParamOut1");
            this.Property(t => t.Retries).HasColumnName("Retries");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.IsLogCompress).HasColumnName("IsLogCompress");


            // Relationships
            this.HasRequired(t => t.CommunicationLog)
                .WithMany()
                .HasForeignKey(d => d.CommunicationLogId).WillCascadeOnDelete(false);

            this.HasRequired(t => t.CommunicationStatusType)
                .WithMany()
                .HasForeignKey(d => d.Status).WillCascadeOnDelete(false); ;

            this.HasRequired(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.DocumentId);
        }
    }
}
