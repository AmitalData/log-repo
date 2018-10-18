using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TermsofUseSignatureMap : EntityTypeConfiguration<TermsofUseSignature>
    {
        public TermsofUseSignatureMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ContactId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TermsofUseSignatures");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SignedDatetime).HasColumnName("SignedDatetime");
            this.Property(t => t.ContactId).HasColumnName("ContactId");
            this.Property(t => t.TermsofUseVersion).HasColumnName("TermsofUseVersion");

            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany()
                .HasForeignKey(d => d.ContactId);
            this.HasRequired(t => t.TermsofUse)
                .WithMany()
                .HasForeignKey(d => d.TermsofUseVersion);

        }
    }
}
