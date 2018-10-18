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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data;
 
namespace Logitude.CRM.Data.EntityMapping
{
 
    public class CRMFilterSettingMap : EntityTypeConfiguration<CRMFilterSetting>
    {
	    string dbms;
        public CRMFilterSettingMap()
        { 
				this.ToTable("CRMFilterSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ControlNameSpace).HasColumnName("ControlNameSpace").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.FilterName).HasColumnName("FilterName").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.FilterValue).HasColumnName("FilterValue").HasMaxLength(50).IsUnicode(false);
        }
    }
}
	 