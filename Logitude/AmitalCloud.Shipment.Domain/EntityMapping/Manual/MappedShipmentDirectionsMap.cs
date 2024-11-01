using AmitalCloud.Shipment.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Shipment.Domain.EntityMapping
{
    public class MappedShipmentDirectionsMap : EntityTypeConfiguration<MappedShipmentDirections>
    {
        public MappedShipmentDirectionsMap()
        {
            this.HasKey(t => new { t.Tenant, t.ShipmentDirectionId });


            this.Property(t => t.Tenant)
            .IsRequired();

            this.Property(t => t.ShipmentDirectionId)
            .IsRequired()
            .HasMaxLength(1)
            .IsUnicode(false);

            this.Property(t => t.UpdateDateTime)
            .IsRequired(); 

            // Table & Column Mappings
            this.ToTable("MappedShipmentDirections");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentDirectionId).HasColumnName("ShipmentDirectionId");
            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime"); 

            //relationships 
            this.HasRequired(t => t.Direction).WithMany().HasForeignKey(d => d.ShipmentDirectionId); 
        }
    }
}
