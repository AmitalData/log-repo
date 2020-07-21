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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data;
 
namespace Logitude.CargoTracking.Data.EntityMapping
{
 
    public class CargoTrackingWatermarkMap : EntityTypeConfiguration<CargoTrackingWatermark>
    {
	    string dbms;
        public CargoTrackingWatermarkMap()
        { 
				this.ToTable("CargoTrackingWatermarks");
		
		    this.HasKey(t => new { t.TableName });
	 
            this.Property(t => t.TableName).HasColumnName("TableName").IsRequired().HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate");
        }
    }
}
	 