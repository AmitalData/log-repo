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
 
    public class BankAccountMap : EntityTypeConfiguration<BankAccount>
    {
	    string dbms;
        public BankAccountMap()
        { 
				this.ToTable("BankAccounts");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.LocalName).HasColumnName("LocalName").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.EnglishName).HasColumnName("EnglishName").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.BankId).HasColumnName("BankId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BranchNumber).HasColumnName("BranchNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DeferredGLAccountId).HasColumnName("DeferredGLAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IBAN).HasColumnName("IBAN").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.SwiftCode).HasColumnName("SwiftCode").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.BranchAddress).HasColumnName("BranchAddress").HasMaxLength(60).IsUnicode(true);

            this.Property(t => t.Inactive).HasColumnName("Inactive");

            this.Property(t => t.ChequeCounter).HasColumnName("ChequeCounter");

            this.Property(t => t.LastPageNumber).HasColumnName("LastPageNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastPageEndDate).HasColumnName("LastPageEndDate");

            this.Property(t => t.LastPageCloseBalance).HasColumnName("LastPageCloseBalance").HasPrecision(16, 2);

            this.Property(t => t.TransferGLAcccountId).HasColumnName("TransferGLAcccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PrintingBranchNumber).HasColumnName("PrintingBranchNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PrintingAccountNumber).HasColumnName("PrintingAccountNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TotalOpenExternalTransactions).HasColumnName("TotalOpenExternalTransactions").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.TotalOpenPagesLines).HasColumnName("TotalOpenPagesLines").HasMaxLength(7).IsUnicode(false);

            this.Property(t => t.ChequeCounterSeriesID).HasColumnName("ChequeCounterSeriesID");
        }
    }
}
	 