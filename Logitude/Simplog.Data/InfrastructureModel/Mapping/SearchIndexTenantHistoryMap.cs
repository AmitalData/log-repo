using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    class SearchIndexTenantHistoryMap : EntityTypeConfiguration<SearchIndexTenantHistory>
    {
        public SearchIndexTenantHistoryMap()
        {
            ToTable("SearchIndexTenantHistory");
            HasKey(t => new { t.Index, t.Tenant });
            Property(t => t.Index).HasColumnName("Index").IsRequired().HasMaxLength(50);
            Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();
            Property(t => t.TtlMonth).HasColumnName("TtlMonth").IsRequired();
            Property(t => t.LastUpdate).HasColumnName("LastUpdate");
        }
    }
}
