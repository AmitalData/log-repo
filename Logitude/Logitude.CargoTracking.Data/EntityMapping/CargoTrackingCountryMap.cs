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
 
    public class CargoTrackingCountryMap : EntityTypeConfiguration<CargoTrackingCountry>
    {
	    string dbms;
        public CargoTrackingCountryMap()
        { 
				this.ToTable("CargoTrackingCountries");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(2).IsFixedLength();

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").IsRequired().HasMaxLength(120).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(120).IsUnicode(true);
        }
    }
}
	 