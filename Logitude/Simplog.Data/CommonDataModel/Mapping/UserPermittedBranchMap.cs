using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserPermittedBranchMap: EntityTypeConfiguration<UserPermittedBranch>
    {
        public UserPermittedBranchMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BranchId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
               .IsRequired();

           

            // Table & Column Mappings
            this.ToTable("UserPermittedBranches");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
             

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId);
        }
    }
}
