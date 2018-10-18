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
 
    public class AccountingPeriodMap : EntityTypeConfiguration<AccountingPeriod>
    {
	    string dbms;
        public AccountingPeriodMap()
        { 
				this.ToTable("AccountingPeriods");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.Year).HasColumnName("Year").IsRequired();

            this.Property(t => t.PeriodTypeCode).HasColumnName("PeriodTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.OpenMonth).HasColumnName("OpenMonth");

            this.Property(t => t.ClosedMonth).HasColumnName("ClosedMonth");
        }
    }
}
	 