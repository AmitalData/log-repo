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
 
    public class QuoteOPTotalVATMap : EntityTypeConfiguration<QuoteOPTotalVAT>
    {
	    string dbms;
        public QuoteOPTotalVATMap()
        { 
				this.ToTable("QuoteOPTotalVATs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.QuoteOPId).HasColumnName("QuoteOPId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteCurrencyVATAmount).HasColumnName("QuoteCurrencyVATAmount");

            this.Property(t => t.QuoteCurrencyVatableAmount).HasColumnName("QuoteCurrencyVatableAmount");

            this.Property(t => t.LocalCurrencyVATAmount).HasColumnName("LocalCurrencyVATAmount");

            this.Property(t => t.LocalCurrencyVatableAmount).HasColumnName("LocalCurrencyVatableAmount");

            this.Property(t => t.ProfitCurrencyVATAmount).HasColumnName("ProfitCurrencyVATAmount");

            this.Property(t => t.ProfitCurrencyVatableAmount).HasColumnName("ProfitCurrencyVatableAmount");

            this.Property(t => t.VatPercent).HasColumnName("VatPercent");

            this.Property(t => t.ExternalVATCard).HasColumnName("ExternalVATCard").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.ExternalTAXItemId).HasColumnName("ExternalTAXItemId").HasMaxLength(25).IsUnicode(false);

            this.Property(t => t.VatOPTypeId).HasColumnName("VatOPTypeId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 