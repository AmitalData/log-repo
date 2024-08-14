using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ProductItemMap : EntityTypeConfiguration<ProductItem>
    {
        public ProductItemMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CustomerId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SKU).IsRequired().HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Brand).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.ASIN).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.UPC).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.OriginCountryId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipperId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ProductItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.SKU).HasColumnName("SKU");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.ASIN).HasColumnName("ASIN");
            this.Property(t => t.UPC).HasColumnName("UPC");
            this.Property(t => t.OriginCountryId).HasColumnName("OriginCountryId");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");

            this.HasRequired(t => t.Customer).WithMany().HasForeignKey(d => d.CustomerId);
            this.HasOptional(t => t.OriginCountry).WithMany().HasForeignKey(d => d.OriginCountryId);
            this.HasOptional(t => t.Shipper).WithMany().HasForeignKey(d => d.ShipperId);
        }
    }
}
