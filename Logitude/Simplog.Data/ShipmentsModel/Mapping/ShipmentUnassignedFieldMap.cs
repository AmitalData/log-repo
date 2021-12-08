using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;


namespace Simplog.Data.ShipmentsModel.Mapping
{
    class ShipmentUnassignedFieldMap : EntityTypeConfiguration<ShipmentUnassignedField>
    {
        public ShipmentUnassignedFieldMap()
        {           
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.FieldName).HasMaxLength(35).IsUnicode(true);
            this.Property(t => t.ReceivedCode).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ReceivedData).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.ReplacedDataId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentUnassignedFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ReceivedCode).HasColumnName("ReceivedCode");
            this.Property(t => t.ReceivedData).HasColumnName("ReceivedData");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.ReplacedDataId).HasColumnName("ReplacedDataId");
 
            // Relationships
        }
    }
}
