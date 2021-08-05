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
 
    public class QuoteOPPriceStepsMap : EntityTypeConfiguration<QuoteOPPriceSteps>
    {
	    string dbms;
        public QuoteOPPriceStepsMap()
        { 
				this.ToTable("QuoteOPPriceSteps");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.QuoteOPId).HasColumnName("QuoteOPId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteOPChargeId).HasColumnName("QuoteOPChargeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Step).HasColumnName("Step").IsRequired();

            this.Property(t => t.CostUnitPrice).HasColumnName("CostUnitPrice");

            this.Property(t => t.SaleUnitPrice).HasColumnName("SaleUnitPrice");

            this.Property(t => t.MarkupValue).HasColumnName("MarkupValue");
        }
    }
}
	 