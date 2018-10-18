using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ContactTenantMap : EntityTypeConfiguration<ContactTenant>
    {
        public ContactTenantMap()
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
            this.ToTable("ContactTenants");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TenantId).HasColumnName("TenantId");
            this.Property(t => t.ContactId).HasColumnName("ContactId");

            // Relationships
            this.HasRequired(t => t.Contact)
                .WithMany()
                .HasForeignKey(d => d.ContactId);
            this.HasRequired(t => t.Tenant)
                .WithMany()
                .HasForeignKey(d => d.TenantId);

        }
    }
}
