using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FilingInboxAttachmentLogMap : EntityTypeConfiguration<FilingInboxAttachmentLog>
    {
        public FilingInboxAttachmentLogMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DocumentsFilingId).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.FilingInboxAttachmentId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("FilingInboxAttachmentLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DocumentsFilingId).HasColumnName("DocumentsFilingId");
            this.Property(t => t.FilingInboxAttachmentId).HasColumnName("FilingInboxAttachmentId");

            this.HasRequired(t => t.DocumentsFiling).WithMany().HasForeignKey(d => d.DocumentsFilingId);
            this.HasRequired(t => t.FilingInboxAttachment).WithMany().HasForeignKey(d => d.FilingInboxAttachmentId);
        }
    }
}
