using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PackageFeatureMap : EntityTypeConfiguration<PackageFeature>
    {
        public PackageFeatureMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.FeatureId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FeatureUniqeCode).IsRequired().HasMaxLength(120).IsUnicode(false);

            this.ToTable("PackageFeatures");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");

            //this.HasRequired(t => t.Feature).WithMany().HasForeignKey(d => d.FeatureId);
            this.HasRequired(t => t.Package).WithMany().HasForeignKey(d => d.PackageCode);
        }
    }
}
