using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentAWBPrintOnlyMap : EntityTypeConfiguration<ShipmentAWBPrintOnly>
    {
        public ShipmentAWBPrintOnlyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.PrepaidCollectId)
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.DueTypeCode)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.IATACodeId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.MeasurementId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentAWBPrintOnlies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.PrepaidCollectId).HasColumnName("PrepaidCollectId");
            this.Property(t => t.DueTypeCode).HasColumnName("DueTypeCode");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.IATACodeId).HasColumnName("IATACodeId");
            this.Property(t => t.MeasurementId).HasColumnName("MeasurementId");

            //// Relationships
            this.HasRequired(t => t.Currency)
                .WithMany()
                .HasForeignKey(d => d.CurrencyId);

            this.HasRequired(t => t.DueType)
                .WithMany()
                .HasForeignKey(d => d.DueTypeCode);

            this.HasRequired(t => t.Shipment)
                .WithMany()
                .HasForeignKey(d => d.ShipmentId);

        }
    }
}
