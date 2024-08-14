using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Simplog.Data.CommonDataModel.Mapping
{
    public class WarehouseMap : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FirmCode).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.TypeCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.PrimaryContactName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.CurrencyId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.AirWeightMeasurementCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.OceanWeightMeasurementCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.InlandWeightMeasurementCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.AirWeightRoundingCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.OceanWeightRoundingCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.InlandWeightRoundingCode).HasMaxLength(4).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Warehouses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.FirmCode).HasColumnName("FirmCode");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.MyWarehouse).HasColumnName("MyWarehouse");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");
            this.Property(t => t.ChargeStorage).HasColumnName("ChargeStorage");
            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
            this.Property(t => t.AirWeightMeasurementCode).HasColumnName("AirWeightMeasurementCode");
            this.Property(t => t.OceanWeightMeasurementCode).HasColumnName("OceanWeightMeasurementCode");
            this.Property(t => t.InlandWeightMeasurementCode).HasColumnName("InlandWeightMeasurementCode");
            this.Property(t => t.AirWeightRoundingCode).HasColumnName("AirWeightRoundingCode");
            this.Property(t => t.OceanWeightRoundingCode).HasColumnName("OceanWeightRoundingCode");
            this.Property(t => t.InlandWeightRoundingCode).HasColumnName("InlandWeightRoundingCode");

            // Relationships
            this.HasRequired(t => t.Card).WithOptional(t => t.Warehouse);
            this.HasOptional(t => t.WarehouseType).WithMany().HasForeignKey(d => d.TypeCode);
            this.HasOptional(t => t.Currency).WithMany().HasForeignKey(d => d.CurrencyId);
            this.HasOptional(t => t.AirWeightMeasurement).WithMany().HasForeignKey(d => d.AirWeightMeasurementCode);
            this.HasOptional(t => t.OceanWeightMeasurement).WithMany().HasForeignKey(d => d.OceanWeightMeasurementCode);
            this.HasOptional(t => t.InlandWeightMeasurement).WithMany().HasForeignKey(d => d.InlandWeightMeasurementCode);
            this.HasOptional(t => t.AirWeightRounding).WithMany().HasForeignKey(d => d.AirWeightRoundingCode);
            this.HasOptional(t => t.OceanWeightRounding).WithMany().HasForeignKey(d => d.OceanWeightRoundingCode);
            this.HasOptional(t => t.InlandWeightRounding).WithMany().HasForeignKey(d => d.InlandWeightRoundingCode);
        }
    }
}
