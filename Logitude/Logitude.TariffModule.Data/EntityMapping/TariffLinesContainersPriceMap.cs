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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data;
 
namespace Logitude.TariffModule.Data.EntityMapping
{
 
    public class TariffLinesContainersPriceMap : EntityTypeConfiguration<TariffLinesContainersPrice>
    {
	    string dbms;
        public TariffLinesContainersPriceMap()
        { 
				this.ToTable("TariffLinesContainersPrices");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.TariffId).HasColumnName("TariffId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TariffLineId).HasColumnName("TariffLineId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SurchargeId).HasColumnName("SurchargeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Price1).HasColumnName("Price1").HasPrecision(18, 3);

            this.Property(t => t.Price2).HasColumnName("Price2").HasPrecision(18, 3);

            this.Property(t => t.Price3).HasColumnName("Price3").HasPrecision(18, 3);

            this.Property(t => t.Price4).HasColumnName("Price4").HasPrecision(18, 3);

            this.Property(t => t.Price5).HasColumnName("Price5").HasPrecision(18, 3);

            this.Property(t => t.CostPrice).HasColumnName("CostPrice").HasPrecision(18, 3);
        }
    }
}
	 