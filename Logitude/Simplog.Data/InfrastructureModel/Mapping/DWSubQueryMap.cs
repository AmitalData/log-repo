using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{


    public class DWSubQueryMap : EntityTypeConfiguration<DWSubQuery>
    {
        public DWSubQueryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DWQueryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DWFactTableCode).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.SQLString).IsMaxLength().IsUnicode(true);
            //this.Property(t => t.CreatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            //this.Property(t => t.UpdateByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            //this.Property(t => t.CreatedDate);
            //this.Property(t => t.UpdatedDate);
            this.Property(t => t.ColumnsXML).IsMaxLength().IsUnicode(true);
            this.Property(t => t.FiltersXML).IsMaxLength().IsUnicode(true);


            this.ToTable("DWSubQueries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DWQueryId).HasColumnName("DWQueryId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DWFactTableCode).HasColumnName("DWFactTableCode");
            this.Property(t => t.SQLString).HasColumnName("SQLString");
            //this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            //this.Property(t => t.UpdateByUserId).HasColumnName("UpdateByUserId");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.ColumnsXML).HasColumnName("ColumnsXML");
            this.Property(t => t.FiltersXML).HasColumnName("FiltersXML");


            //this.HasRequired(t => t.DWObjectTable).WithMany().HasForeignKey(d => d.DWFactTableCode);
            //this.HasRequired(t => t.CreatedBy).WithMany().HasForeignKey(d => d.CreatedByUserId);
            //this.HasRequired(t => t.UpdateBy).WithMany().HasForeignKey(d => d.UpdateByUserId);
            this.HasRequired(t => t.DWQuery).WithMany().HasForeignKey(d => d.DWQueryId);

        }
    }


}
