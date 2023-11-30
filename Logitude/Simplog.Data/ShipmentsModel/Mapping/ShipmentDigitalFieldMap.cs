using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentDigitalFieldMap : EntityTypeConfiguration<ShipmentDigitalField>
    {
        public ShipmentDigitalFieldMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            // Table & Column Mappings
            this.ToTable("ShipmentDigitalFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsCustomerArchived).HasColumnName("IsCustomerArchived");
        }
    }
}
