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
 
    public class CargoReferencesSyncQueueMap : EntityTypeConfiguration<CargoReferencesSyncQueue>
    {
	    string dbms;
        public CargoReferencesSyncQueueMap()
        { 
				this.ToTable("CargoReferencesSyncQueues");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ForwardingShipmentId).HasColumnName("ForwardingShipmentId").IsRequired().HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.ShipmentNeedUpdateType).HasColumnName("ShipmentNeedUpdateType").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}
	 