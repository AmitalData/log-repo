using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DWObjectFieldCategoriesMap : EntityTypeConfiguration<DWObjectFieldCategories>
    {
        
        public DWObjectFieldCategoriesMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.DWCategoryCode).IsRequired().HasMaxLength(50).IsUnicode(false); 
            this.Property(t => t.DWObjectFieldCode).IsRequired().HasMaxLength(50).IsUnicode(false);
           

            this.ToTable("DWObjectFieldCategories");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DWObjectFieldCode).HasColumnName("DWObjectFieldCode");
            this.Property(t => t.DWCategoryCode).HasColumnName("DWCategoryCode");

            //this.HasRequired(t => t.DWObjectField).WithMany().HasForeignKey(d => d.DWObjectFieldCode);
            this.HasRequired(t => t.DWCategory).WithMany().HasForeignKey(d => d.DWCategoryCode);
        }
    }
}
