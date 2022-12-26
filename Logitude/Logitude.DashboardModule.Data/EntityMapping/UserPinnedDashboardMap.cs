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
 
    public class UserPinnedDashboardMap : EntityTypeConfiguration<UserPinnedDashboard>
    {
	    string dbms;
        public UserPinnedDashboardMap()
        { 
				this.ToTable("UserPinnedDashboards");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Dashboards).HasColumnName("Dashboards").HasMaxLength(1000).IsUnicode(false);
        }
    }
}
	 