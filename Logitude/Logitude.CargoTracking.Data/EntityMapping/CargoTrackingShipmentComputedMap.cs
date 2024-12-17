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
 
    public class CargoTrackingShipmentComputedMap : EntityTypeConfiguration<CargoTrackingShipmentComputed>
    {
	    string dbms;
        public CargoTrackingShipmentComputedMap()
        { 
				this.ToTable("CargoTrackingShipmentComputeds");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FirstPickupATD).HasColumnName("FirstPickupATD");

            this.Property(t => t.FinalDeliveryATA).HasColumnName("FinalDeliveryATA");

            this.Property(t => t.FinalDeliveryETA).HasColumnName("FinalDeliveryETA");

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 