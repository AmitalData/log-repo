using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data;
 
namespace Logitude.DashboardModule.Data.EntityMapping
{
 
    public class DashboardGlobalFilterMap : EntityTypeConfiguration<DashboardGlobalFilter>
    {
	    string dbms;
        public DashboardGlobalFilterMap()
        { 
				this.ToTable("DashboardGlobalFilters");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DashboardId).HasColumnName("DashboardId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsCommonFilter).HasColumnName("IsCommonFilter").IsRequired();

            this.Property(t => t.CommonFilterField).HasColumnName("CommonFilterField").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.DataSetId).HasColumnName("DataSetId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DataSetFieldId).HasColumnName("DataSetFieldId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FilterOperator).HasColumnName("FilterOperator").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.DataTypeCode).HasColumnName("DataTypeCode").IsRequired().HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();
        }
    }
}
	 