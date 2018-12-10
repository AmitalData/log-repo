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
 
    public class GLAccountMoreDataMap : EntityTypeConfiguration<GLAccountMoreData>
    {
	    string dbms;
        public GLAccountMoreDataMap()
        { 
				this.ToTable("GLAccountMoreDatas");
		
		    this.HasKey(t => new { t.AccountId });
	 
            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.BalanceInLocalCurrency).HasColumnName("BalanceInLocalCurrency").HasPrecision(16, 2);

            this.Property(t => t.LocalBalanceInDue).HasColumnName("LocalBalanceInDue").HasPrecision(16, 2);

            this.Property(t => t.NextDueDate).HasColumnName("NextDueDate");

            this.Property(t => t.TotalOpenChequesInLocalCur).HasColumnName("TotalOpenChequesInLocalCur").HasPrecision(16, 2);

            this.Property(t => t.TotFutureOpenChequesInLocalCur).HasColumnName("TotFutureOpenChequesInLocalCur").HasPrecision(16, 2);
        }
    }
}
	 