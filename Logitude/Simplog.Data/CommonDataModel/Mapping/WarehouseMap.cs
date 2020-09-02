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
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.FirmCode).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.TypeCode)
             .HasMaxLength(4)
             .IsUnicode(false);

            this.Property(t => t.PrimaryContactName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

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

            // Relationships
            this.HasRequired(t => t.Card)
                .WithOptional(t => t.Warehouse);

            this.HasOptional(t => t.WarehouseType)
                .WithMany()
                .HasForeignKey(d => d.TypeCode);
        }
    }
}
