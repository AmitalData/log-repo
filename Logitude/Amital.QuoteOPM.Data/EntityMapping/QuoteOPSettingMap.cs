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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPSettingMap : EntityTypeConfiguration<QuoteOPSetting>
    {
	    string dbms;
        public QuoteOPSettingMap()
        { 
				this.ToTable("QuoteOPSettings");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CopyExchangeRates).HasColumnName("CopyExchangeRates");

            this.Property(t => t.AutomaticallyCloseDays).HasColumnName("AutomaticallyCloseDays");

            this.Property(t => t.CopyShipper).HasColumnName("CopyShipper");

            this.Property(t => t.CopyConsignee).HasColumnName("CopyConsignee");

            this.Property(t => t.CopyMainCarriage).HasColumnName("CopyMainCarriage");

            this.Property(t => t.CopyPickup).HasColumnName("CopyPickup");

            this.Property(t => t.CopyDelivery).HasColumnName("CopyDelivery");

            this.Property(t => t.CopyChargesTypes).HasColumnName("CopyChargesTypes");

            this.Property(t => t.CopyChargesCost).HasColumnName("CopyChargesCost");

            this.Property(t => t.CopyChargesSale).HasColumnName("CopyChargesSale");

            this.Property(t => t.EditMainCarriage).HasColumnName("EditMainCarriage");

            this.Property(t => t.CopyAgent).HasColumnName("CopyAgent");

            this.Property(t => t.CopyNotify).HasColumnName("CopyNotify");

            this.Property(t => t.IsSaleAsCostCurrency).HasColumnName("IsSaleAsCostCurrency");

            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");

            this.Property(t => t.QuoteExpirationDays).HasColumnName("QuoteExpirationDays");
        }
    }
}
	 