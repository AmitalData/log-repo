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
 
    public class SupplierInvoiceFreightAmountMap : EntityTypeConfiguration<SupplierInvoiceFreightAmount>
    {
	    string dbms;
        public SupplierInvoiceFreightAmountMap()
        { 
			  this.ToTable("SupplierInvoiceFreightAmounts", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.CurrencyTypeCode });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CurrencyTypeCode).HasColumnName("CurrencyTypeCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Amount).HasColumnName("Amount").HasPrecision(16, 2);
        }
    }
}
	 