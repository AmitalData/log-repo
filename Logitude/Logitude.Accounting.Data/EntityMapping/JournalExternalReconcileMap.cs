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
 
    public class JournalExternalReconcileMap : EntityTypeConfiguration<JournalExternalReconcile>
    {
	    string dbms;
        public JournalExternalReconcileMap()
        { 
				this.ToTable("JournalExternalReconciles");
		
		    this.HasKey(t => new { t.JournalId, t.Line });
	 
            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.LedgerTransactionId).HasColumnName("LedgerTransactionId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReconcileExternalPageLineId).HasColumnName("ReconcileExternalPageLineId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SkipAccountsValidation).HasColumnName("SkipAccountsValidation");
        }
    }
}
	 