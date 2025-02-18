using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentCommodityMap : EntityTypeConfiguration<ShipmentCommodity>
    {
        public ShipmentCommodityMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ShipmentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RateClassCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DescriptionOfGoods).HasMaxLength(4000).IsUnicode(true);
            this.Property(t => t.CommodityNumber).HasMaxLength(15).IsUnicode(false);

            this.ToTable("ShipmentCommodities");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ChargeRate).HasColumnName("ChargeRate");
            this.Property(t => t.ChargeAmount).HasColumnName("ChargeAmount");
            this.Property(t => t.RateClassCode).HasColumnName("RateClassCode");
            this.Property(t => t.CommodityNumber).HasColumnName("CommodityNumber");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");
            this.Property(t => t.IsFirstLine).HasColumnName("IsFirstLine");

            this.HasOptional(t => t.RateClass).WithMany().HasForeignKey(d => d.RateClassCode);
            this.HasRequired(t => t.Shipment).WithMany().HasForeignKey(d => d.ShipmentId);

        }
    }
}
