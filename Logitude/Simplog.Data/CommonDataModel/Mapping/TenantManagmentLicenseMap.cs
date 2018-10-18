using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class TenantManagmentLicenseMap : EntityTypeConfiguration<TenantManagmentLicense>
    {
        public TenantManagmentLicenseMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).IsRequired().HasMaxLength(4).IsUnicode(false);

            this.ToTable("TenantManagmentLicenses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.NumberOfUsers).HasColumnName("NumberOfUsers");

            this.HasRequired(t => t.Package).WithMany().HasForeignKey(d => d.PackageCode);
        }
    }
}
