using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class AdditionalCurrencyRateMap : EntityTypeConfiguration<AdditionalCurrencyRate>
    {
        public AdditionalCurrencyRateMap()
        { 
			this.ToTable("AdditionalCurrencyRates");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.RateCoefficient).HasColumnName("RateCoefficient");
        }
    }
}
	 