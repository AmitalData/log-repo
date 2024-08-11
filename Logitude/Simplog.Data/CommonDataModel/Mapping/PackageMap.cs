using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PackageMap : EntityTypeConfiguration<Package>
    {
        public PackageMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(80).IsUnicode(true);
            this.Property(t => t.FeaturePackageTypeCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("Packages");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FeaturePackageTypeCode).HasColumnName("FeaturePackageTypeCode");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasRequired(t => t.FeaturePackageType).WithMany().HasForeignKey(d => d.FeaturePackageTypeCode);
        }
    }
}
