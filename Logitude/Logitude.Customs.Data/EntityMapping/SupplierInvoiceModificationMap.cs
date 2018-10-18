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
 
    public class SupplierInvoiceModificationMap : EntityTypeConfiguration<SupplierInvoiceModification>
    {
	    string dbms;
        public SupplierInvoiceModificationMap()
        { 
			  this.ToTable("SupplierInvoiceModifications", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.ModificationCounterKey });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CurrencyTypeCode).HasColumnName("CurrencyTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Amount).HasColumnName("Amount").HasPrecision(16, 2);

            this.Property(t => t.ModificationCounterKey).HasColumnName("ModificationCounterKey").IsRequired().HasDatabaseGeneratedOption(null);
        }
    }
}
	 