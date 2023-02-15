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
 
    public class VendorCurrencyMap : EntityTypeConfiguration<VendorCurrency>
    {
	    string dbms;
        public VendorCurrencyMap()
        { 
			  this.ToTable("VendorCurrencies", "Customs");
		
		    this.HasKey(t => new { t.VendorId, t.Tenant, t.LineNumber, t.Currency });
	 
            this.Property(t => t.VendorId).HasColumnName("VendorId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.Currency).HasColumnName("Currency").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 