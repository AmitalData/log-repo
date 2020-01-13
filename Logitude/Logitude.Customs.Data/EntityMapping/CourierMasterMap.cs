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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class CourierMasterMap : EntityTypeConfiguration<CourierMaster>
    {
	    string dbms;
        public CourierMasterMap()
        { 
			  this.ToTable("CourierMasters", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.AirlineId).HasColumnName("AirlineId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MAWB).HasColumnName("MAWB").IsRequired().HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.HAWB).HasColumnName("HAWB").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.EstimatedArrivalDate).HasColumnName("EstimatedArrivalDate");

            this.Property(t => t.GatewayPortCode).HasColumnName("GatewayPortCode").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.OriginPortCode).HasColumnName("OriginPortCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.IsOpen).HasColumnName("IsOpen");

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MAWBTypeCode).HasColumnName("MAWBTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ManifestNumber).HasColumnName("ManifestNumber").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.PackageQuantity).HasColumnName("PackageQuantity");

            this.Property(t => t.GrossMassMeasure).HasColumnName("GrossMassMeasure").HasPrecision(18, 2);

            this.Property(t => t.ShortHAWB).HasColumnName("ShortHAWB").HasMaxLength(8).IsUnicode(true);

            this.Property(t => t.FlightNumber).HasColumnName("FlightNumber").HasMaxLength(4).IsUnicode(true);

            this.Property(t => t.DepartureDate).HasColumnName("DepartureDate");

            this.Property(t => t.WeightValueCode).HasColumnName("WeightValueCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.StorageSiteCode).HasColumnName("StorageSiteCode").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.TruckerId).HasColumnName("TruckerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IntegratorCode).HasColumnName("IntegratorCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsReadyForInvoice).HasColumnName("IsReadyForInvoice");
        }
    }
}
	 