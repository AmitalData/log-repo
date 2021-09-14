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
 
    public class QuoteOPPropertiesMap : EntityTypeConfiguration<QuoteOPProperties>
    {
	    string dbms;
        public QuoteOPPropertiesMap()
        { 
				this.ToTable("QuoteOPPropertiess");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.QuoteID).HasColumnName("QuoteID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Order).HasColumnName("Order");

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IncotermId).HasColumnName("IncotermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SpecialServiceID).HasColumnName("SpecialServiceID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 