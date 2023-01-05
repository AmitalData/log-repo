using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DeploymentPackageMap : EntityTypeConfiguration<DeploymentPackage>
    {
        public DeploymentPackageMap()
        {
            this.HasKey(t => t.Id); 
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Tenant).IsRequired();
            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.CreatedBy).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateDate).IsRequired();
            this.Property(t => t.UpdatedBy).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.DirectionId).IsRequired().HasMaxLength(1).IsUnicode(false);
            this.Property(t => t.VersionId).HasMaxLength(15).IsUnicode(false);

            this.ToTable("DeploymentPackages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.VersionId).HasColumnName("VersionId");

            // Relationships
            this.HasRequired(t => t.CreatedByUser).WithMany().HasForeignKey(d => d.CreatedBy);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedBy);
            this.HasRequired(t => t.Direction).WithMany().HasForeignKey(d => d.DirectionId);
            this.HasRequired(t=>t.DeploymentPackagesVersion).WithMany().HasForeignKey(d => d.VersionId);
        }
    }
}
