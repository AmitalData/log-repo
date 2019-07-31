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
 
    public class JournalMap : EntityTypeConfiguration<Journal>
    {
	    string dbms;
        public JournalMap()
        { 
				this.ToTable("Journals");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.JournalNumber).HasColumnName("JournalNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.AccountingDate).HasColumnName("AccountingDate").IsRequired();

            this.Property(t => t.TypeCode).HasColumnName("TypeCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountingEntityCode).HasColumnName("AccountingEntityCode").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountingEntityId).HasColumnName("AccountingEntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExternalNo).HasColumnName("ExternalNo").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ApproveDate).HasColumnName("ApproveDate");

            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.AccountingEntityReference).HasColumnName("AccountingEntityReference").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.OriginalJournalId).HasColumnName("OriginalJournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VoidedByUserId).HasColumnName("VoidedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.VoidDate).HasColumnName("VoidDate");

            this.Property(t => t.IsVoided).HasColumnName("IsVoided");

            this.Property(t => t.VoidedByJournalId).HasColumnName("VoidedByJournalId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExternalSystem).HasColumnName("ExternalSystem").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.QueueId).HasColumnName("QueueId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsLedgerCreated).HasColumnName("IsLedgerCreated");
        }
    }
}
	 