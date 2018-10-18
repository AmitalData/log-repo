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
 
    public class GLAccountTotalByMonthMap : EntityTypeConfiguration<GLAccountTotalByMonth>
    {
	    string dbms;
        public GLAccountTotalByMonthMap()
        { 
				this.ToTable("GLAccountTotalByMonths");
		
		    this.HasKey(t => new { t.AccountId, t.DateTypeCode, t.Year, t.Month, t.CurrencyId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DateTypeCode).HasColumnName("DateTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Year).HasColumnName("Year").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.Month).HasColumnName("Month").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.CurrencyId).HasColumnName("CurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LocalAmountDebit).HasColumnName("LocalAmountDebit").HasPrecision(16, 2);

            this.Property(t => t.LocalAmountCredit).HasColumnName("LocalAmountCredit").HasPrecision(16, 2);

            this.Property(t => t.ForeignAmountDebit).HasColumnName("ForeignAmountDebit").HasPrecision(16, 2);

            this.Property(t => t.ForeignAmountCredit).HasColumnName("ForeignAmountCredit").HasPrecision(16, 2);
        }
    }
}
	 