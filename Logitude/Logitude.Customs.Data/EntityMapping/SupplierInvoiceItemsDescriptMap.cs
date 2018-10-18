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
 
    public class SupplierInvoiceItemsDescriptMap : EntityTypeConfiguration<SupplierInvoiceItemsDescript>
    {
	    string dbms;
        public SupplierInvoiceItemsDescriptMap()
        { 
			  this.ToTable("SupplierInvoiceItemsDescripts", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.LineNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceItemLineNumber).HasColumnName("InvoiceItemLineNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(35).IsUnicode(true);
        }
    }
}
	 