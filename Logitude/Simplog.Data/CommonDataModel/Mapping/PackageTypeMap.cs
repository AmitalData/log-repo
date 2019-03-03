using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class PackageTypeMap : EntityTypeConfiguration<PackageType>
    {
        public PackageTypeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.PrintAs).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.EnglishName).IsRequired().HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.LocalName).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.Notes).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.MeasurementId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("PackageTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.PrintAs).HasColumnName("PrintAs");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
            this.Property(t => t.IsAir).HasColumnName("IsAir");
            this.Property(t => t.IsOcean).HasColumnName("IsOcean");
            this.Property(t => t.IsInland).HasColumnName("IsInland");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.IsContainer).HasColumnName("IsContainer");
            this.Property(t => t.TEU).HasColumnName("TEU");
            this.Property(t => t.ContainerSize).HasColumnName("ContainerSize");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsRefrigerated).HasColumnName("IsRefrigerated");

            // Relationships
            this.HasOptional(t => t.Measurement).WithMany().HasForeignKey(d => d.MeasurementId);

        }
    }
}
