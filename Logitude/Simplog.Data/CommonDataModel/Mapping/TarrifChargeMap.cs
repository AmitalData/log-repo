using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TarrifChargeMap : EntityTypeConfiguration<TarrifCharge>
    {
        public TarrifChargeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.TarrifHeaderId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ChargesTypeId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MeasurementId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TarrifCharges");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.TarrifHeaderId).HasColumnName("TarrifHeaderId");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");

            // Relationships
            this.HasRequired(t => t.ChargesType)
                .WithMany()
                .HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.Currency)
                .WithMany()
                .HasForeignKey(d => d.CurrencyId);
            this.HasRequired(t => t.Measurement)
                .WithMany()
                .HasForeignKey(d => d.MeasurementId);
            this.HasRequired(t => t.TarrifHeader)
                .WithMany()
                .HasForeignKey(d => d.TarrifHeaderId);

        }
    }
}
