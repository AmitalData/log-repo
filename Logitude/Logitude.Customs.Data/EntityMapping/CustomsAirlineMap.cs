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
 
    public class CustomsAirlineMap : EntityTypeConfiguration<CustomsAirline>
    {
	    string dbms;
        public CustomsAirlineMap()
        { 
			  this.ToTable("CustomsAirlines", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.AirlineCode).HasColumnName("AirlineCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(70).IsUnicode(false);

            this.Property(t => t.InActive).HasColumnName("InActive");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.AirlinePrefix).HasColumnName("AirlinePrefix").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ICAO).HasColumnName("ICAO").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.UnloadPortCode).HasColumnName("UnloadPortCode").HasMaxLength(17).IsUnicode(false);
        }
    }
}
	 