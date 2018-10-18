using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
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
