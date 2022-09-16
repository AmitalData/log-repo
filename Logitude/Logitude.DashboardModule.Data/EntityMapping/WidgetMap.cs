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

            this.Property(t => t.GroupById).HasColumnName("GroupById").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DashboardId).HasColumnName("DashboardId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StartPotistion).HasColumnName("StartPotistion").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.EndPosition).HasColumnName("EndPosition").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Filters).HasColumnName("Filters").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DateGroupCode).HasColumnName("DateGroupCode").HasMaxLength(17).IsUnicode(false);
        }
    }
}
	 