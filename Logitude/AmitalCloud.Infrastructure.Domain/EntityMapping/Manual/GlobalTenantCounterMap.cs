using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class GlobalTenantCounterMap : EntityTypeConfiguration<GlobalTenantCounter>
    {
        public GlobalTenantCounterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("GlobalTenantCounters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LastNumber).HasColumnName("LastNumber");
        }
    }
}
