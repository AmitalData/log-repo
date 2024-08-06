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
 
    public class DashboardGlobalPresetFilterMap : EntityTypeConfiguration<DashboardGlobalPresetFilter>
    {
	    string dbms;
        public DashboardGlobalPresetFilterMap()
        { 
				this.ToTable("DashboardGlobalPresetFilters");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.DisplayName).HasColumnName("DisplayName").IsRequired().HasMaxLength(300).IsUnicode(true);

            this.Property(t => t.DataTypeCode).HasColumnName("DataTypeCode").IsRequired().HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.IsDisabled).HasColumnName("IsDisabled");

            this.Property(t => t.IsMultiSelect).HasColumnName("IsMultiSelect");

            this.Property(t => t.JoinedTableName).HasColumnName("JoinedTableName").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.Sort).HasColumnName("Sort");

            this.Property(t => t.JoinedTableDisplayField).HasColumnName("JoinedTableDisplayField").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.CanSearch).HasColumnName("CanSearch");
        }
    }
}
	 