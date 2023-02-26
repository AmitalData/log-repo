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
 
    public class AddressCurrencyMap : EntityTypeConfiguration<AddressCurrency>
    {
	    string dbms;
        public AddressCurrencyMap()
        { 
			  this.ToTable("AddressCurrencies", "Customs");
		
		    this.HasKey(t => new { t.AddressId, t.Currency });
	 
            this.Property(t => t.AddressId).HasColumnName("AddressId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.LineNumber).HasColumnName("LineNumber");

            this.Property(t => t.Currency).HasColumnName("Currency").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 