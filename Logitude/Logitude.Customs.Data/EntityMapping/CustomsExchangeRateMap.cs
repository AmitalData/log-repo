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
 
    public class CustomsExchangeRateMap : EntityTypeConfiguration<CustomsExchangeRate>
    {
	    string dbms;
        public CustomsExchangeRateMap()
        { 
			  this.ToTable("CustomsExchangeRates", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CurrencyTypeCode).HasColumnName("CurrencyTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate").HasPrecision(15, 10);

            this.Property(t => t.RateDate).HasColumnName("RateDate");

            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
        }
    }
}
	 