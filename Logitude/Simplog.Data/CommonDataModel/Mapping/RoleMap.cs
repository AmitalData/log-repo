using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.RoleTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.Description).HasMaxLength(250).IsUnicode(true);
            this.Property(t => t.ParentRoleId).HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.IsCustomRole).IsRequired();
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

            this.ToTable("Roles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.RoleTypeCode).HasColumnName("RoleTypeCode");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ParentRoleId).HasColumnName("ParentRoleId");
            this.Property(t => t.IsCustomRole).HasColumnName("IsCustomRole");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");

            this.HasRequired(t => t.RoleType).WithMany().HasForeignKey(d => d.RoleTypeCode);
        }
    }
}
