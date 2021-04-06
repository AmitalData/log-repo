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
 
    public class GLAccountAgingDataMap : EntityTypeConfiguration<GLAccountAgingData>
    {
	    string dbms;
        public GLAccountAgingDataMap()
        { 
				this.ToTable("GLAccountAgingDatas");
		
		    this.HasKey(t => new { t.AccountId });
	 
            this.Property(t => t.AccountId).HasColumnName("AccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.PeriodPast).HasColumnName("PeriodPast").HasPrecision(16, 2);

            this.Property(t => t.Period0).HasColumnName("Period0").HasPrecision(16, 2);

            this.Property(t => t.Period1).HasColumnName("Period1").HasPrecision(16, 2);

            this.Property(t => t.Period2).HasColumnName("Period2").HasPrecision(16, 2);

            this.Property(t => t.Period3).HasColumnName("Period3").HasPrecision(16, 2);

            this.Property(t => t.Period4).HasColumnName("Period4").HasPrecision(16, 2);

            this.Property(t => t.Period5).HasColumnName("Period5").HasPrecision(16, 2);

            this.Property(t => t.PeriodFuture).HasColumnName("PeriodFuture").HasPrecision(16, 2);

            this.Property(t => t.TotalOpenTransactions).HasColumnName("TotalOpenTransactions");
        }
    }
}
	 