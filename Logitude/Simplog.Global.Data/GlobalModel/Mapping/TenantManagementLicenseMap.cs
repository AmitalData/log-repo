using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class TenantManagementLicenseMap : EntityTypeConfiguration<TenantManagementLicense>
    {
        public TenantManagementLicenseMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).IsRequired().HasMaxLength(5).IsUnicode(false);

            this.ToTable("TenantManagementLicenses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.NumberOfUsers).HasColumnName("NumberOfUsers");
        }
    }
}
