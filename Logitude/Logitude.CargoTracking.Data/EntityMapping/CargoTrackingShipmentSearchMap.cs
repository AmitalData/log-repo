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
 
    public class CargoTrackingShipmentSearchMap : EntityTypeConfiguration<CargoTrackingShipmentSearch>
    {
	    string dbms;
        public CargoTrackingShipmentSearchMap()
        { 
				this.ToTable("CargoTrackingShipmentSearches");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ShipmentDate).HasColumnName("ShipmentDate").IsRequired();

            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsPublic).HasColumnName("IsPublic");

            this.Property(t => t.ReferenceType).HasColumnName("ReferenceType").IsRequired().HasMaxLength(200).IsUnicode(false);
        }
    }
}
	 