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
 
    public class CargoDisconnectQueueMap : EntityTypeConfiguration<CargoDisconnectQueue>
    {
	    string dbms;
        public CargoDisconnectQueueMap()
        { 
				this.ToTable("CargoDisconnectQueues");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.ShipmentType).HasColumnName("ShipmentType").HasMaxLength(1).IsUnicode(false);
        }
    }
}
	 