using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AllActiveGLAccountsViewMap : EntityTypeConfiguration<AllActiveGLAccountsView>
    {
        public AllActiveGLAccountsViewMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15);
         

            this.ToTable("AllActiveGLAccountsViews");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
           

            
        }
    }
}
