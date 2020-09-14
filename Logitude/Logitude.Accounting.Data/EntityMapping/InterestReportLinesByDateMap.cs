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
 
    public class InterestReportLinesByDateMap : EntityTypeConfiguration<InterestReportLinesByDate>
    {
	    string dbms;
        public InterestReportLinesByDateMap()
        { 
				this.ToTable("InterestReportLinesByDates");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.InterestReportId).HasColumnName("InterestReportId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromDate).HasColumnName("FromDate").IsRequired();

            this.Property(t => t.ToDate).HasColumnName("ToDate").IsRequired();

            this.Property(t => t.TotalInterestDays).HasColumnName("TotalInterestDays");

            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount").HasPrecision(18, 2);

            this.Property(t => t.AccumulatedAmount).HasColumnName("AccumulatedAmount").HasPrecision(18, 2);

            this.Property(t => t.StandardInterestPercentage).HasColumnName("StandardInterestPercentage").HasPrecision(4, 2);

            this.Property(t => t.ExceptionalInterestPercentage).HasColumnName("ExceptionalInterestPercentage").HasPrecision(4, 2);

            this.Property(t => t.CreditInterestPercentage).HasColumnName("CreditInterestPercentage").HasPrecision(4, 2);

            this.Property(t => t.StandardInterestAmount).HasColumnName("StandardInterestAmount").HasPrecision(18, 2);

            this.Property(t => t.ExceptionalInterestAmount).HasColumnName("ExceptionalInterestAmount").HasPrecision(18, 2);

            this.Property(t => t.CreditInterestAmount).HasColumnName("CreditInterestAmount").HasPrecision(18, 2);

            this.Property(t => t.CalculatedStandInterestAmount).HasColumnName("CalculatedStandInterestAmount").HasPrecision(20, 4);

            this.Property(t => t.CalculatedExcepInterestAmount).HasColumnName("CalculatedExcepInterestAmount").HasPrecision(20, 4);

            this.Property(t => t.CalculatedCreditInterestAmount).HasColumnName("CalculatedCreditInterestAmount").HasPrecision(20, 4);

            this.Property(t => t.CalculationDetails).HasColumnName("CalculationDetails").HasMaxLength(256).IsUnicode(true);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired();

            this.Property(t => t.IsOpenBalanceLine).HasColumnName("IsOpenBalanceLine");
        }
    }
}
	 