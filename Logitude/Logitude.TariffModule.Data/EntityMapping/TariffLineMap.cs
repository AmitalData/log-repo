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
 
    public class TariffLineMap : EntityTypeConfiguration<TariffLine>
    {
	    string dbms;
        public TariffLineMap()
        { 
				this.ToTable("TariffLines");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StartDate).HasColumnName("StartDate").IsRequired();

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");

            this.Property(t => t.TariffId).HasColumnName("TariffId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Version).HasColumnName("Version");

            this.Property(t => t.MinPrice).HasColumnName("MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Step1Price).HasColumnName("Step1Price").HasPrecision(18, 3);

            this.Property(t => t.Step2Price).HasColumnName("Step2Price").HasPrecision(18, 3);

            this.Property(t => t.Step3Price).HasColumnName("Step3Price").HasPrecision(18, 3);

            this.Property(t => t.Step4Price).HasColumnName("Step4Price").HasPrecision(18, 3);

            this.Property(t => t.Step5Price).HasColumnName("Step5Price").HasPrecision(18, 3);

            this.Property(t => t.Step6Price).HasColumnName("Step6Price").HasPrecision(18, 3);

            this.Property(t => t.Step7Price).HasColumnName("Step7Price").HasPrecision(18, 3);

            this.Property(t => t.Step8Price).HasColumnName("Step8Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge1Price).HasColumnName("Surcharge1Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge2Price).HasColumnName("Surcharge2Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge3Price).HasColumnName("Surcharge3Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge4Price).HasColumnName("Surcharge4Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge5Price).HasColumnName("Surcharge5Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge6Price).HasColumnName("Surcharge6Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge7Price).HasColumnName("Surcharge7Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge8Price).HasColumnName("Surcharge8Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge9Price).HasColumnName("Surcharge9Price").HasPrecision(18, 3);

            this.Property(t => t.Surcharge10Price).HasColumnName("Surcharge10Price").HasPrecision(18, 3);

            this.Property(t => t.OriginPortId).HasColumnName("OriginPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DestinationPortId).HasColumnName("DestinationPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OriginPortText).HasColumnName("OriginPortText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.DestinationPortText).HasColumnName("DestinationPortText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.MinPriceText).HasColumnName("MinPriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step1PriceText).HasColumnName("Step1PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step2PriceText).HasColumnName("Step2PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step3PriceText).HasColumnName("Step3PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step4PriceText).HasColumnName("Step4PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step5PriceText).HasColumnName("Step5PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step6PriceText).HasColumnName("Step6PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step7PriceText).HasColumnName("Step7PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Step8PriceText).HasColumnName("Step8PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge1PriceText).HasColumnName("Surcharge1PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge2PriceText).HasColumnName("Surcharge2PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge3PriceText).HasColumnName("Surcharge3PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge4PriceText).HasColumnName("Surcharge4PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge5PriceText).HasColumnName("Surcharge5PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge6PriceText).HasColumnName("Surcharge6PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge7PriceText).HasColumnName("Surcharge7PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge8PriceText).HasColumnName("Surcharge8PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge9PriceText).HasColumnName("Surcharge9PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.Surcharge10PriceText).HasColumnName("Surcharge10PriceText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.HasErrors).HasColumnName("HasErrors");

            this.Property(t => t.ErrorText).HasColumnName("ErrorText").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.LineUniqueKey).HasColumnName("LineUniqueKey").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.LineUniqueKeyText).HasColumnName("LineUniqueKeyText").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.Index).HasColumnName("Index").IsRequired();

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.IsFromAllOtherPorts).HasColumnName("IsFromAllOtherPorts");

            this.Property(t => t.IsToAllOtherPorts).HasColumnName("IsToAllOtherPorts");

            this.Property(t => t.Surcharge1MinPrice).HasColumnName("Surcharge1MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge2MinPrice).HasColumnName("Surcharge2MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge3MinPrice).HasColumnName("Surcharge3MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge4MinPrice).HasColumnName("Surcharge4MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge5MinPrice).HasColumnName("Surcharge5MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge6MinPrice).HasColumnName("Surcharge6MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge7MinPrice).HasColumnName("Surcharge7MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge8MinPrice).HasColumnName("Surcharge8MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge9MinPrice).HasColumnName("Surcharge9MinPrice").HasPrecision(18, 3);

            this.Property(t => t.Surcharge10MinPrice).HasColumnName("Surcharge10MinPrice").HasPrecision(18, 3);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TransitTime).HasColumnName("TransitTime").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.IsDifferentCurrenciesPerCharge).HasColumnName("IsDifferentCurrenciesPerCharge");

            this.Property(t => t.Surcharge1CurrencyId).HasColumnName("Surcharge1CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge2CurrencyId).HasColumnName("Surcharge2CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge3CurrencyId).HasColumnName("Surcharge3CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge4CurrencyId).HasColumnName("Surcharge4CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge5CurrencyId).HasColumnName("Surcharge5CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge6CurrencyId).HasColumnName("Surcharge6CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge7CurrencyId).HasColumnName("Surcharge7CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge8CurrencyId).HasColumnName("Surcharge8CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge9CurrencyId).HasColumnName("Surcharge9CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Surcharge10CurrencyId).HasColumnName("Surcharge10CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ViaPortId).HasColumnName("ViaPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ViaPortText).HasColumnName("ViaPortText").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.FromCountryId).HasColumnName("FromCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToCountryId).HasColumnName("ToCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsFromAllOtherCountries).HasColumnName("IsFromAllOtherCountries");

            this.Property(t => t.IsToAllOtherCountries).HasColumnName("IsToAllOtherCountries");

            this.Property(t => t.UnitOfMeasurementCode).HasColumnName("UnitOfMeasurementCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 