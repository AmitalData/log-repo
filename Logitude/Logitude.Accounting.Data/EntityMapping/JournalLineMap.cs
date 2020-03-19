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
 
    public class JournalLineMap : EntityTypeConfiguration<JournalLine>
    {
	    string dbms;
        public JournalLineMap()
        { 
				this.ToTable("JournalLines");
		
		    this.HasKey(t => new { t.JournalId, t.Line });
	 
            this.Property(t => t.JournalId).HasColumnName("JournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ActionCode).HasColumnName("ActionCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DebitControlAccountId).HasColumnName("DebitControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DebitAccountId).HasColumnName("DebitAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreditControlAccountId).HasColumnName("CreditControlAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreditAccountId).HasColumnName("CreditAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DocumentDate).HasColumnName("DocumentDate");

            this.Property(t => t.AccountingDate).HasColumnName("AccountingDate").IsRequired();

            this.Property(t => t.DueDate).HasColumnName("DueDate");

            this.Property(t => t.LocalAmount).HasColumnName("LocalAmount").HasPrecision(16, 2);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").HasPrecision(16, 2);

            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate").HasPrecision(16, 5);

            this.Property(t => t.Reference1).HasColumnName("Reference1").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.Reference2).HasColumnName("Reference2").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.Reference3).HasColumnName("Reference3").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.ExternalOpenAmount).HasColumnName("ExternalOpenAmount").HasPrecision(16, 2);

            this.Property(t => t.ExternalReconcileNumber).HasColumnName("ExternalReconcileNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsExternalReconcile).HasColumnName("IsExternalReconcile");

            this.Property(t => t.ActionId).HasColumnName("ActionId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 