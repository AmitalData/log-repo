using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CurrencyRateMap : EntityTypeConfiguration<CurrencyRate>
    {
        public CurrencyRateMap()
        { 
			this.ToTable("CurrencyRates");
		
		    this.HasKey(t => new { t.Id });

            this.Property(t => t.Id).HasColumnName("Id").IsRequired();

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ExchangeRateId).HasColumnName("ExchangeRateId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AdditionalCurrencyRateId).HasColumnName("AdditionalCurrencyRateId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Rate).HasColumnName("Rate");
        }
    }
}
	 