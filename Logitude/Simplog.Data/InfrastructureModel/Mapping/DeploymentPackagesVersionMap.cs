using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DeploymentPackagesVersionMap : EntityTypeConfiguration<DeploymentPackagesVersion>
    {
        public DeploymentPackagesVersionMap()
        {
            this.HasKey(t => t.Id); 
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateDate).IsRequired();
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DeploymentPackageID).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DocumentId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.VersionName).IsRequired().HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.VersionNumber).IsRequired();

            this.ToTable("DeploymentPackagesVersions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.DeploymentPackageID).HasColumnName("DeploymentPackageID");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.VersionName).HasColumnName("VersionName");
            this.Property(t => t.VersionNumber).HasColumnName("VersionNumber");

            // Relationships
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);
            this.HasRequired(t => t.Document).WithMany().HasForeignKey(d => d.DocumentId);
            this.HasRequired(t => t.DeploymentPackage).WithMany().HasForeignKey(d => d.DeploymentPackageID);
        }
    }
}
