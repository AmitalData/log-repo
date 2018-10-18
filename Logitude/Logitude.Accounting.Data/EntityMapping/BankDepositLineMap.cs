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
 
    public class BankDepositLineMap : EntityTypeConfiguration<BankDepositLine>
    {
	    string dbms;
        public BankDepositLineMap()
        { 
				this.ToTable("BankDepositLines");
		
		    this.HasKey(t => new { t.DepositId, t.Line });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.DepositId).HasColumnName("DepositId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Line).HasColumnName("Line").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.ARPaymentChequeId).HasColumnName("ARPaymentChequeId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsOutOfDeposit).HasColumnName("IsOutOfDeposit");

            this.Property(t => t.OutOfDepositeDate).HasColumnName("OutOfDepositeDate");

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 