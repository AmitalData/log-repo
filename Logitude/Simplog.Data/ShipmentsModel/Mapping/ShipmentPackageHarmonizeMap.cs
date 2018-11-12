using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class ShipmentPackageHarmonizeMap : EntityTypeConfiguration<ShipmentPackageHarmonize>
    {
        public ShipmentPackageHarmonizeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Harmonize).IsRequired().HasMaxLength(60).IsUnicode(false);

            this.ToTable("ShipmentPackageHarmonize");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PackageId).HasColumnName("PackageId");
            this.Property(t => t.Harmonize).HasColumnName("Harmonize");

            this.HasRequired(t => t.Package).WithMany().HasForeignKey(d => d.PackageId);
        }
    }
}
