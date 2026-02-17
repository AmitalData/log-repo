using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{


    public class DWQueryMap : EntityTypeConfiguration<DWQuery>
    {
        public DWQueryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            //this.Property(t => t.DWObjectTableCode).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.SQLString).IsMaxLength().IsUnicode(true);
            this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UpdateByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreatedDate);
            this.Property(t => t.UpdatedDate);


            this.ToTable("DWQueries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            //this.Property(t => t.DWObjectTableCode).HasColumnName("DWObjectTableCode");
            this.Property(t => t.SQLString).HasColumnName("SQLString");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateByUserId).HasColumnName("UpdateByUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");


            //this.HasRequired(t => t.DWObjectTable).WithMany().HasForeignKey(d => d.DWObjectTableCode);
            this.HasRequired(t => t.CreatedBy).WithMany().HasForeignKey(d => d.CreatedByUserId);
            this.HasRequired(t => t.UpdateBy).WithMany().HasForeignKey(d => d.UpdateByUserId);

        }
    }


}
