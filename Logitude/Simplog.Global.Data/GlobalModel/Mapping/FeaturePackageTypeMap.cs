using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class FeaturePackageTypeMap : EntityTypeConfiguration<FeaturePackageType>
    {
        public FeaturePackageTypeMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("FeaturePackageTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
