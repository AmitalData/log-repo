using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data;
 
namespace Logitude.CargoTracking.Data.EntityMapping
{
 
    public class CargoTrackingMilestoneMap : EntityTypeConfiguration<CargoTrackingMilestone>
    {
	    string dbms;
        public CargoTrackingMilestoneMap()
        { 
				this.ToTable("CargoTrackingMilestones");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.Weight).HasColumnName("Weight");

            this.Property(t => t.ExportWeight).HasColumnName("ExportWeight");

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.EventTypeId).HasColumnName("EventTypeId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 