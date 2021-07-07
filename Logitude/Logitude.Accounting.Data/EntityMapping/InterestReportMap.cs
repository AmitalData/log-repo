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
 
    public class InterestReportMap : EntityTypeConfiguration<InterestReport>
    {
	    string dbms;
        public InterestReportMap()
        { 
				this.ToTable("InterestReports");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ReportNumber).HasColumnName("ReportNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InterestCalculationDate).HasColumnName("InterestCalculationDate").IsRequired();

            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount").HasPrecision(18, 2);

            this.Property(t => t.OpenBalance).HasColumnName("OpenBalance").HasPrecision(18, 2);

            this.Property(t => t.CloseBalance).HasColumnName("CloseBalance").HasPrecision(18, 2);

            this.Property(t => t.ARinvoiceId).HasColumnName("ARinvoiceId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceAmount).HasColumnName("InvoiceAmount").HasPrecision(18, 2);

            this.Property(t => t.GLAccountInterestCreditLimit).HasColumnName("GLAccountInterestCreditLimit").HasPrecision(18, 2);

            this.Property(t => t.InterestReportStatusCode).HasColumnName("InterestReportStatusCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InvoiceFailureReason).HasColumnName("InvoiceFailureReason").HasMaxLength(1024).IsUnicode(true);

            this.Property(t => t.CreditAllotmentPercentage).HasColumnName("CreditAllotmentPercentage").HasPrecision(4, 2);

            this.Property(t => t.CalCreditAllotmentCommission).HasColumnName("CalCreditAllotmentCommission").HasPrecision(18, 2);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.CalculatedPostponedChequesCommision).HasColumnName("CalculatedPostponedChequesComm").HasPrecision(16, 2);
			}
			else
			{
              this.Property(t => t.CalculatedPostponedChequesCommision).HasColumnName("CalculatedPostponedChequesCommision").HasPrecision(16, 2);
			}

        }
    }
}
	 