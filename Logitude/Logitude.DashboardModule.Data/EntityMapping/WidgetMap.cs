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
 
    public class WidgetMap : EntityTypeConfiguration<Widget>
    {
	    string dbms;
        public WidgetMap()
        { 
				this.ToTable("Widgets");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Title).HasColumnName("Title").IsRequired().HasMaxLength(300).IsUnicode(true);

            this.Property(t => t.GroupById).HasColumnName("GroupById").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DashboardId).HasColumnName("DashboardId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartPotistion).HasColumnName("StartPotistion").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.EndPosition).HasColumnName("EndPosition").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Filters).HasColumnName("Filters").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DateGroupCode).HasColumnName("DateGroupCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.MaximumGrouping).HasColumnName("MaximumGrouping");

            this.Property(t => t.SortBy).HasColumnName("SortBy");

            this.Property(t => t.SortDirection).HasColumnName("SortDirection").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.TimeOverTime).HasColumnName("TimeOverTime");

            this.Property(t => t.ComparisonPeriod).HasColumnName("ComparisonPeriod");

            this.Property(t => t.Increase).HasColumnName("Increase").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ComparisonOperator).HasColumnName("ComparisonOperator").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.ComparisonDateGroup).HasColumnName("ComparisonDateGroup").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.FromDate).HasColumnName("FromDate");

            this.Property(t => t.ToDate).HasColumnName("ToDate");

            this.Property(t => t.SecondaryGroupById).HasColumnName("SecondaryGroupById").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SecondaryDateGroupCode).HasColumnName("SecondaryDateGroupCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.Alignment).HasColumnName("Alignment").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ThousandSeparator).HasColumnName("ThousandSeparator");

            this.Property(t => t.UseNumberAbbreviation).HasColumnName("UseNumberAbbreviation");

            this.Property(t => t.DecimalPlaces).HasColumnName("DecimalPlaces");

            this.Property(t => t.UseAbbreviationAfter).HasColumnName("UseAbbreviationAfter").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LabelsPosition).HasColumnName("LabelsPosition").HasMaxLength(100).IsUnicode(false);
        }
    }
}
	 