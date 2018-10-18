using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FilingInboxAttachmentMap : EntityTypeConfiguration<FilingInboxAttachment>
    {
        public FilingInboxAttachmentMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FileName).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.DocumentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FilingInboxId).IsRequired().HasMaxLength(15).IsUnicode(false);

            this.ToTable("FilingInboxAttachments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.FilingInboxId).HasColumnName("FilingInboxId");

            this.HasRequired(t => t.Document).WithMany().HasForeignKey(d => d.DocumentId);
            this.HasRequired(t => t.FilingInbox).WithMany().HasForeignKey(d => d.FilingInboxId);
        }
    }
}
