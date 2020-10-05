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
 
    public class CargoTrackingIncrementalStatMap : EntityTypeConfiguration<CargoTrackingIncrementalStat>
    {
	    string dbms;
        public CargoTrackingIncrementalStatMap()
        { 
				this.ToTable("CargoTrackingIncrementalStats");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();

            this.Property(t => t.EndDate).HasColumnName("EndDate").IsRequired();

            this.Property(t => t.Shipments).HasColumnName("Shipments").IsRequired();

            this.Property(t => t.Cards).HasColumnName("Cards").IsRequired();

            this.Property(t => t.Ports).HasColumnName("Ports").IsRequired();

            this.Property(t => t.Countries).HasColumnName("Countries").IsRequired();

            this.Property(t => t.TransportModes).HasColumnName("TransportModes").IsRequired();

            this.Property(t => t.ShipmentComputedFields).HasColumnName("ShipmentComputedFields").IsRequired();

            this.Property(t => t.ShipmentMasterDatas).HasColumnName("ShipmentMasterDatas").IsRequired();

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(null);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.ErrorLog).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.ErrorLog).HasMaxLength(4000);
			}


            this.Property(t => t.ErrorLog).HasColumnName("ErrorLog").IsUnicode(true);
        }
    }
}
	 