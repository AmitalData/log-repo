using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPackageItemMap : EntityTypeConfiguration<ShipmentPackageItem>
    {
        public ShipmentPackageItemMap()
        {
            this.HasKey(t => new { t.PackageId, t.LineNumber });

            this.Property(t => t.PackageId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LineNumber).HasDatabaseGeneratedOption(null);
            this.Property(t => t.Description).IsRequired().HasMaxLength(200).IsUnicode(true);

            this.ToTable("ShipmentPackageItems");
            this.Property(t => t.PackageId).HasColumnName("PackageId");
            this.Property(t => t.LineNumber).HasColumnName("LineNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Quantity).HasColumnName("Quantity").IsRequired();
            this.Property(t => t.GoodsValue).HasColumnName("GoodsValue");

            this.HasRequired(t => t.ShipmentPackage).WithMany().HasForeignKey(d => d.PackageId);

        }
    }
}
