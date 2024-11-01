using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ContactTenantRoleSetMap : EntityTypeConfiguration<ContactTenantRole>
    {
        public ContactTenantRoleSetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ContactTenantId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.RoleId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ContactTenantRoleSet");
            this.Property(t => t.ContactTenantId).HasColumnName("ContactTenantId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            // Relationships
            this.HasRequired(t => t.ContactTenant)
                .WithMany()
                .HasForeignKey(d => d.ContactTenantId);
            this.HasRequired(t => t.Role)
                .WithMany()
                .HasForeignKey(d => d.RoleId);

        }
    }
}
