using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class MeasurementMap : EntityTypeConfiguration<Measurement>
    {
        public MeasurementMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Code).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.ShortName).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.LocalName).HasMaxLength(100).IsUnicode(true);

            this.ToTable("Measurements");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsContainerMeasurement).HasColumnName("IsContainerMeasurement");
            this.Property(t => t.IsContainer).HasColumnName("IsContainer");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.LocalName).HasColumnName("LocalName");
        }
    }
}
