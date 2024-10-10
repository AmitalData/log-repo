using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class FreightForwarderReferenceMap : EntityTypeConfiguration<FreightForwarderReference>
    {
        public FreightForwarderReferenceMap()
        {
            this.HasKey(t => new { t.Tenant, t.ShipmentId,t.LineNumber });
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ForwarderShipmentNumber).HasMaxLength(20).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("FreightForwarderReferences");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
			this.Property(t => t.LineNumber).HasColumnName("LineNumber");
			this.Property(t => t.ForwarderShipmentNumber).HasColumnName("ForwarderShipmentNumber");
            this.Property(t => t.ForwarderFileConnect).HasColumnName("ForwarderFileConnect");

            // Relationships
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);
        }
    }
}
