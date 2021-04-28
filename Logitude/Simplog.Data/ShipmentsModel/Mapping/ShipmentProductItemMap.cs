using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    class ShipmentProductItemMap : EntityTypeConfiguration<ShipmentProductItem>
    {
        public ShipmentProductItemMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ShipmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ProductItemId)
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentProductItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ProductItemId).HasColumnName("ProductItemId");            

            // Relationships
            //this.HasOptional(t => t.CustomerProductItem)
            //    .WithMany()
            //    .HasForeignKey(d => d.ProductItemId);

            this.HasRequired(t => t.Shipment)
                .WithMany()
                .HasForeignKey(d => d.ShipmentId);

        }
    }
}
