using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class GlobalContactMap : EntityTypeConfiguration<GlobalContact>
    {
        public GlobalContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);

            

            // Table & Column Mappings
            this.ToTable("GlobalContacts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Email).HasColumnName("Email");
          
            this.Property(t => t.GlobalTenantId).HasColumnName("GlobalTenantId");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.IsUser).HasColumnName("IsUser");
            this.Property(t => t.InternetAccess).HasColumnName("InternetAccess");

            // Relationships
            this.HasRequired(t => t.GlobalTenant)
                .WithMany()
                .HasForeignKey(d => d.GlobalTenantId);

        }
    }
}
