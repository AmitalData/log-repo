using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FilingInboxMap : EntityTypeConfiguration<FilingInbox>
    {
        public FilingInboxMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Sender).HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.Subject).HasMaxLength(200).IsUnicode(true);
            this.Property(t => t.BodyDocumentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("FilingInboxes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.BodyDocumentId).HasColumnName("BodyDocumentId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasRequired(t => t.BodyDocument).WithMany().HasForeignKey(d => d.BodyDocumentId);
            this.HasOptional(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
        }
    }
}
