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
 
    public class DashboardSharedUserMap : EntityTypeConfiguration<DashboardSharedUser>
    {
	    string dbms;
        public DashboardSharedUserMap()
        { 
				this.ToTable("DashboardSharedUsers");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DashboardId).HasColumnName("DashboardId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 