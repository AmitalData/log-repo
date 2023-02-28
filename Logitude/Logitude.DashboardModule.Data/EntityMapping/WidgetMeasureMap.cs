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
 
    public class WidgetMeasureMap : EntityTypeConfiguration<WidgetMeasure>
    {
	    string dbms;
        public WidgetMeasureMap()
        { 
				this.ToTable("WidgetMeasures");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.WidgetId).HasColumnName("WidgetId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MeasureCode).HasColumnName("MeasureCode").IsRequired().HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.MeasureFieldId).HasColumnName("MeasureFieldId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RenderAs).HasColumnName("RenderAs").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.YAxisType).HasColumnName("YAxisType").HasMaxLength(100).IsUnicode(false);
        }
    }
}
	 