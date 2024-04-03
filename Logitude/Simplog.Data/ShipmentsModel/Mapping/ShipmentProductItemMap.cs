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

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductItemId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.HTSCode).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.SKU).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.Brand).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.Name).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.OtherDuties).HasMaxLength(200).IsUnicode(false);
            this.Property(t => t.Remarks).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ShipperId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.OriginCountryId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ShipmentProductItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.ProductItemId).HasColumnName("ProductItemId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.HTSCode).HasColumnName("HTSCode");
            this.Property(t => t.SKU).HasColumnName("SKU");
            this.Property(t => t.ApprovedByCustomer).HasColumnName("ApprovedByCustomer");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.VATPercentage).HasColumnName("VATPercentage");
            this.Property(t => t.DutiesPercentage).HasColumnName("DutiesPercentage");
            this.Property(t => t.OtherDuties).HasColumnName("OtherDuties");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");
            this.Property(t => t.OriginCountryId).HasColumnName("OriginCountryId");

            // Relationships
            this.HasRequired(t => t.ProductItem).WithMany().HasForeignKey(d => d.ProductItemId);
            this.HasOptional(t => t.OriginCountry).WithMany().HasForeignKey(d => d.OriginCountryId);
            this.HasOptional(t => t.Shipper).WithMany().HasForeignKey(d => d.ShipperId);
        }
    }
}
