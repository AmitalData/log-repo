using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FeatureChangeMap : EntityTypeConfiguration<FeatureChange>
    {
        public FeatureChangeMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.UserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RoleId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PackageCode).HasMaxLength(5).IsUnicode(false);
            this.Property(t => t.Notes).HasMaxLength(8000).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("FeatureChanges");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.EventDateTime).HasColumnName("EventDateTime");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.PackageCode).HasColumnName("PackageCode");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
            this.HasOptional(t => t.Role).WithMany().HasForeignKey(d => d.RoleId);
            this.HasOptional(t => t.Package).WithMany().HasForeignKey(d => d.PackageCode);

        }
    }
}
