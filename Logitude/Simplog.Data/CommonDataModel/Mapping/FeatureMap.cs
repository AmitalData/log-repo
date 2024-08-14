using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FeatureMap : EntityTypeConfiguration<Feature>
    {
        public FeatureMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.NameTextCodeId).IsRequired().HasMaxLength(60).IsUnicode(true);
            this.Property(t => t.Code).HasMaxLength(120).IsUnicode(false);
            //this.Property(t => t.Code).IsRequired().HasMaxLength(120).IsUnicode(false);
            this.Property(t => t.FeatureTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.IsBusinessUnitEnabled).IsRequired();
            this.Property(t => t.IsOld).IsRequired();
            this.Property(t => t.IsCoreFeature).IsRequired();
            this.Property(t => t.ToggleCode).HasMaxLength(3).IsUnicode(false);
            //this.Property(t => t.NameTextCodeCode).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.NameTextCodeCode).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.FeatureUniqeCode).HasMaxLength(120).IsUnicode(false);
            //this.Property(t => t.FeatureUniqeCode).IsRequired().HasMaxLength(120).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("Features");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.NameTextCodeId).HasColumnName("NameTextCodeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.FeatureTypeCode).HasColumnName("FeatureTypeCode");
            this.Property(t => t.Packagable).HasColumnName("Packagable");
            this.Property(t => t.IsBusinessUnitEnabled).HasColumnName("IsBusinessUnitEnabled");
            this.Property(t => t.IsOld).HasColumnName("IsOld");
            this.Property(t => t.IsCoreFeature).HasColumnName("IsCoreFeature");
            this.Property(t => t.ToggleCode).HasColumnName("ToggleCode");
            this.Property(t => t.NameTextCodeCode).HasColumnName("NameTextCodeCode");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");

            this.HasRequired(t => t.FeatureType).WithMany().HasForeignKey(d => d.FeatureTypeCode);

        }
    }
}
