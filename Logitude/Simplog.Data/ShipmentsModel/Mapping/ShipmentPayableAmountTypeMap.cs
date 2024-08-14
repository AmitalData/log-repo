using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPayableAmountTypeMap : EntityTypeConfiguration<ShipmentPayableAmountType>
    {
        public ShipmentPayableAmountTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentPayableAmountTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
