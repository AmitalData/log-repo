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
 
    public class WidgetTypeMap : EntityTypeConfiguration<WidgetType>
    {
	    string dbms;
        public WidgetTypeMap()
        { 
				this.ToTable("WidgetTypes");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 