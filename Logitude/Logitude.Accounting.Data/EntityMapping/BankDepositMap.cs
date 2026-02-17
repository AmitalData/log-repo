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
 
    public class BankDepositMap : EntityTypeConfiguration<BankDeposit>
    {
	    string dbms;
        public BankDepositMap()
        { 
				this.ToTable("BankDeposits");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DepositNumber).HasColumnName("DepositNumber").IsRequired();

            this.Property(t => t.DepositDate).HasColumnName("DepositDate").IsRequired();

            this.Property(t => t.DepositCurrencyId).HasColumnName("DepositCurrencyId").IsRequired().HasMaxLength(2).IsFixedLength();

            this.Property(t => t.LocalDepositAmount).HasColumnName("LocalDepositAmount").IsRequired().HasPrecision(16, 2);

            this.Property(t => t.ForeignAmount).HasColumnName("ForeignAmount").IsRequired().HasPrecision(16, 2);

            this.Property(t => t.DepositBankAccountId).HasColumnName("DepositBankAccountId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CashBookId).HasColumnName("CashBookId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AccountingDate).HasColumnName("AccountingDate").IsRequired();

            this.Property(t => t.IsCanceled).HasColumnName("IsCanceled");
        }
    }
}
	 