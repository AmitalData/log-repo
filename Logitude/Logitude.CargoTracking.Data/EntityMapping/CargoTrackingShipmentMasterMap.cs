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
 
    public class CargoTrackingShipmentMasterMap : EntityTypeConfiguration<CargoTrackingShipmentMaster>
    {
	    string dbms;
        public CargoTrackingShipmentMasterMap()
        { 
				this.ToTable("CargoTrackingShipmentMasters");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Master).HasColumnName("Master").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.MainCarriageATD).HasColumnName("MainCarriageATD");

            this.Property(t => t.MainCarriageETD).HasColumnName("MainCarriageETD");

            this.Property(t => t.MainCarriageATA).HasColumnName("MainCarriageATA");

            this.Property(t => t.MainCarriageETA).HasColumnName("MainCarriageETA");

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 