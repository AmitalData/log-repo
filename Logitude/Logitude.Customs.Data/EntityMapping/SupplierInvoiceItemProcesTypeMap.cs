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
 
    public class SupplierInvoiceItemProcesTypeMap : EntityTypeConfiguration<SupplierInvoiceItemProcesType>
    {
	    string dbms;
        public SupplierInvoiceItemProcesTypeMap()
        { 
			  this.ToTable("SupplierInvoiceItemProcesTypes", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.LineNumber });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceItemLineNumber).HasColumnName("InvoiceItemLineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ProcessTypeCode).HasColumnName("ProcessTypeCode").HasMaxLength(7).IsUnicode(false);
        }
    }
}
	 