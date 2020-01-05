using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class RoleFeatureMap : EntityTypeConfiguration<RoleFeature>
    {
        public RoleFeatureMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RoleId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FeatureId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.FeatureAccessLevelCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.IsDeleted).IsRequired();
            this.Property(t => t.FeatureUniqeCode).HasMaxLength(120).IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("RoleFeatures");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");
            this.Property(t => t.FeatureAccessLevelCode).HasColumnName("FeatureAccessLevelCode");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.FeatureUniqeCode).HasColumnName("FeatureUniqeCode");


            // Relationships
            //this.HasRequired(t => t.Feature).WithMany().HasForeignKey(d => d.FeatureId);
            this.HasRequired(t => t.Role).WithMany().HasForeignKey(d => d.RoleId);
            this.HasRequired(t => t.FeatureAccessLevel).WithMany().HasForeignKey(d => d.FeatureAccessLevelCode);
        }
    }
}
