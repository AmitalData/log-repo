using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserFreelancerGroupMap : EntityTypeConfiguration<UserFreelancerGroup>
    {
        public UserFreelancerGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant).IsRequired();

            this.Property(t => t.UserId)
              .IsRequired()
              .HasMaxLength(15)
              .IsUnicode(false);

            this.Property(t => t.GroupID)
              .IsRequired()
              .HasMaxLength(15)
              .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("UserFreelancerGroups");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.FreelancerGroupType).WithMany().HasForeignKey(d => d.GroupID);




        }
    }
}
