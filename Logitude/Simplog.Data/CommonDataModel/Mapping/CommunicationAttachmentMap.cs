using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CommunicationAttachmentMap : EntityTypeConfiguration<CommunicationAttachment>
    {
        public CommunicationAttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CommunicationLogId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CommunicationAttachments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");

            // Relationships
            this.HasRequired(t => t.CommunicationLog)
                .WithMany()
                .HasForeignKey(d => d.CommunicationLogId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.Document)
                .WithMany()
                .HasForeignKey(d => d.DocumentId)
                .WillCascadeOnDelete(false);

        }
    }
}
