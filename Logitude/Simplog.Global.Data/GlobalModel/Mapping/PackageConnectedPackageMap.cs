using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class PackageConnectedPackageMap : EntityTypeConfiguration<PackageConnectedPackage>
    {
        public PackageConnectedPackageMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).IsRequired().HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.ConnectedPackageCode).IsRequired().HasMaxLength(5).IsUnicode(false);

            this.ToTable("PackageConnectedPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.ConnectedPackageCode).HasColumnName("ConnectedPackageCode");

            this.HasRequired(t => t.Package).WithMany().HasForeignKey(d => d.PackageCode);
            this.HasRequired(t => t.ConnectedPackage).WithMany().HasForeignKey(d => d.ConnectedPackageCode);
        }
    }
}
