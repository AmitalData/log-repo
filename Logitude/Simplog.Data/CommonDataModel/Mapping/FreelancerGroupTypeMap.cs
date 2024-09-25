using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FreelancerGroupTypeMap : EntityTypeConfiguration<FreelancerGroupType>
    {
        public FreelancerGroupTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .HasMaxLength(15)
                .IsUnicode(true);

            this.Property(t => t.EnglishName)
                .HasMaxLength(30)
                .IsUnicode(false);

            this.Property(t => t.InActive).HasColumnName(columnName: "InActive");

            
            // Table & Column Mappings
            this.ToTable("FreelancerGroupTypes");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");            
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.EnglishName).HasColumnName("EnglishName");


        }
    }
}
