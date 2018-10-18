using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class IATACodeMap : EntityTypeConfiguration<IATACode>
    {
        public IATACodeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.MeasurementCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.DueTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.IsIATA).IsRequired();
            this.Property(t => t.InActive).IsRequired();
            this.Property(t => t.AirlineId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("IATACodes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MeasurementCode).HasColumnName("MeasurementCode");
            this.Property(t => t.DueTypeCode).HasColumnName("DueTypeCode");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.IsIATA).HasColumnName("IsIATA");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.AirlineId).HasColumnName("AirlineId");

            this.HasOptional(t => t.DueType).WithMany().HasForeignKey(d => d.DueTypeCode);
            this.HasOptional(t => t.Airline).WithMany().HasForeignKey(d => d.AirlineId);
        }
    }
}
