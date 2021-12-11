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

            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IncotermId).HasColumnName("IncotermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SpecialServiceID).HasColumnName("SpecialServiceID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromAddressId).HasColumnName("FromAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromAddressZipCode).HasColumnName("FromAddressZipCode").HasMaxLength(15).IsFixedLength();

            this.Property(t => t.FromAddressCountryId).HasColumnName("FromAddressCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromAddressCity).HasColumnName("FromAddressCity").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.ToAddressId).HasColumnName("ToAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressCity).HasColumnName("ToAddressCity").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.ToAddressCountryId).HasColumnName("ToAddressCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressZipCode).HasColumnName("ToAddressZipCode").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 