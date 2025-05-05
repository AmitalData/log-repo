using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    class SearchIndexMap : EntityTypeConfiguration<SearchIndex>
    {
        public SearchIndexMap()
        {
            ToTable("SearchIndexes");
            HasKey(x => x.Index);
            Property(x => x.Index).HasColumnName("Index").IsRequired().HasMaxLength(50);
            Property(x => x.Indexer).HasColumnName("Indexer").IsRequired().HasMaxLength(50);
            Property(x => x.IsView).HasColumnName("IsView").IsRequired();
            Property(x => x.ObjectName).HasColumnName("ObjectName").IsRequired().HasMaxLength(50);
            Property(x => x.TtlMonth).HasColumnName("TtlMonth").IsRequired();
            Property(x => x.TtlField).HasColumnName("TtlField").IsOptional().HasMaxLength(20);
            Property(x => x.BuildIntervalMin).HasColumnName("BuildIntervalMin").IsRequired();
            Property(x => x.LastUpdate).HasColumnName("LastUpdate");
            Property(x => x.LastRemove).HasColumnName("LastRemove");
        }
    }
}
