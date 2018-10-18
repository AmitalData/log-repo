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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class JournalReconcileMap : EntityTypeConfiguration<JournalReconcile>
    {
	    string dbms;
        public JournalReconcileMap()
        { 
				this.ToTable("JournalReconciles");
		
		    this.HasKey(t => new { t.JournalId, t.LedgerTransactionId });
	 
            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.LedgerTransactionId).HasColumnName("LedgerTransactionId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired();

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReconciliationAmount).HasColumnName("ReconciliationAmount").HasPrecision(16, 2);

            this.Property(t => t.IsPartial).HasColumnName("IsPartial").IsRequired();
        }
    }
}
	 