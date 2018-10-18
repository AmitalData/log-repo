using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserLicenseMap : EntityTypeConfiguration<UserLicense>
    {
        public UserLicenseMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).IsRequired().HasMaxLength(5).IsUnicode(false);

            this.ToTable("UserLicenses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");

            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Package).WithMany().HasForeignKey(d => d.PackageCode);
        }
    }
}
