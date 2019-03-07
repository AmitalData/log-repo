using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{


    public class DWObjectFieldMap : EntityTypeConfiguration<DWObjectField>
    {
        public DWObjectFieldMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.DWObjectTableCode).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.DataTypeCode).IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.DimensionTableCode).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.AggregationTypeCode).HasMaxLength(5).IsUnicode(false);
            //this.Property(t => t.Category1).HasMaxLength(150).IsUnicode(false);
            //this.Property(t => t.Category2).HasMaxLength(150).IsUnicode(false);
            this.Property(t => t.LOVAdditionalColumns).HasMaxLength(1000).IsUnicode(false);
            this.Property(t => t.HelpText).HasMaxLength(2000).IsUnicode(false);
             
            this.ToTable("DWObjectFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.DWObjectTableCode).HasColumnName("DWObjectTableCode");
            this.Property(t => t.DataTypeCode).HasColumnName("DataTypeCode");
            this.Property(t => t.DimensionTableCode).HasColumnName("DimensionTableCode");
            this.Property(t => t.MinLength).HasColumnName("MinLength");
            this.Property(t => t.MaxLength).HasColumnName("MaxLength");
            this.Property(t => t.IsRequired).HasColumnName("IsRequired");
            this.Property(t => t.IsPrimaryKey).HasColumnName("IsPrimaryKey");
            this.Property(t => t.IsMeasurement).HasColumnName("IsMeasurement");
            this.Property(t => t.AggregationTypeCode).HasColumnName("AggregationTypeCode");
            this.Property(t => t.DisplayInQueryBuilder).HasColumnName("DisplayInQueryBuilder");
            //this.Property(t => t.Category1).HasColumnName("Category1"); 
            //this.Property(t => t.Category2).HasColumnName("Category2");
            this.Property(t => t.LOVAdditionalColumns).HasColumnName("LOVAdditionalColumns");
            this.Property(t => t.HideTree).HasColumnName("HideTree");
            this.Property(t => t.HelpText).HasColumnName("HelpText");
            this.Property(t => t.CannotFilter).HasColumnName("CannotFilter");

            this.HasRequired(t => t.DWObjectTable).WithMany().HasForeignKey(d => d.DWObjectTableCode);
            this.HasOptional(t => t.DimensionTable).WithMany().HasForeignKey(d => d.DimensionTableCode);

        }
    }



}
