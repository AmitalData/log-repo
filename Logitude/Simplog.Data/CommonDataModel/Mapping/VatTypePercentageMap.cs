using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class VatTypePercentageMap : EntityTypeConfiguration<VatTypePercentage>
    {
        public VatTypePercentageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.VatTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("VatTypePercentages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.Percentage).HasColumnName("Percentage");
            this.Property(t => t.VatTypeId).HasColumnName("VatTypeId");

            // Relationships
            this.HasRequired(t => t.VatType)
                .WithMany()
                .HasForeignKey(d => d.VatTypeId);

        }
    }
}
