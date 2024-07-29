using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentTypeMap : EntityTypeConfiguration<ShipmentType>
    {
        public ShipmentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(true);

            this.Property(t => t.TransportModeId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("ShipmentTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate"); 

            // Relationships
            //this.HasRequired(t => t.TransportMode)
            //    .WithMany(t => t.ShipmentTypes)
            //    .HasForeignKey(d => d.TransportModeId);

        }
    }
}
