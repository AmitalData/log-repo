using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class PickUpDeliveryFromToTypeMap : EntityTypeConfiguration<PickUpDeliveryFromToType>
    {
        public PickUpDeliveryFromToTypeMap()
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
                .HasMaxLength(80)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("PickUpDeliveryFromToTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
