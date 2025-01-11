using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class GlobalTenantMap : EntityTypeConfiguration<GlobalTenant>
    {
        public GlobalTenantMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.GlobalDBId).IsRequired().HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.CompanyName).IsRequired().HasMaxLength(100).IsUnicode(true);
            this.Property(t => t.TTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.PrivateLabelId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("GlobalTenants");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GlobalDBId).HasColumnName("GlobalDBId");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.TTY).HasColumnName("TTY");
            this.Property(t => t.PrivateLabelId).HasColumnName("PrivateLabelId");

            // Relationships
            this.HasRequired(t => t.GlobalDB).WithMany().HasForeignKey(d => d.GlobalDBId);
            this.HasOptional(t => t.TenantManagmentPrivateLabel).WithMany().HasForeignKey(d => d.PrivateLabelId);
        }
    }
}
