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
 
    public class SuppInvoiceItemsAbachStatementMap : EntityTypeConfiguration<SuppInvoiceItemsAbachStatement>
    {
	    string dbms;
        public SuppInvoiceItemsAbachStatementMap()
        { 
			  this.ToTable("SuppInvoiceItemsAbachStatement", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.InvoiceCounterKey, t.InvoiceItemLineNumber, t.SequenceNumeric });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceCounterKey).HasColumnName("InvoiceCounterKey").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.InvoiceItemLineNumber).HasColumnName("InvoiceItemLineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.SequenceNumeric).HasColumnName("SequenceNumeric").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.StatementTypeCode).HasColumnName("StatementTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.IsStatementInd).HasColumnName("IsStatementInd");
        }
    }
}
	 