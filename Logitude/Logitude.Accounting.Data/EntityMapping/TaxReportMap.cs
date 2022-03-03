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
 
    public class TaxReportMap : EntityTypeConfiguration<TaxReport>
    {
	    string dbms;
        public TaxReportMap()
        { 
				this.ToTable("TaxReports");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastUpdateDate).HasColumnName("LastUpdateDate").IsRequired();

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.TaxReportMonth).HasColumnName("TaxReportMonth").IsRequired();

            this.Property(t => t.TaxReportNumber).HasColumnName("TaxReportNumber").HasMaxLength(6).IsUnicode(false);

            this.Property(t => t.VatNumber).HasColumnName("VatNumber").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.TaxReportTypeCode).HasColumnName("TaxReportTypeCode").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.TaxableOutputAmount).HasColumnName("TaxableOutputAmount").HasPrecision(16, 2);

            this.Property(t => t.OutputTaxAmount).HasColumnName("OutputTaxAmount").HasPrecision(16, 2);

            this.Property(t => t.TaxableOutputsWithDiffPercent).HasColumnName("TaxableOutputsWithDiffPercent").HasPrecision(16, 2);

            this.Property(t => t.OutputTaxAmountWithDiffPercent).HasColumnName("OutputTaxAmountWithDiffPercent").HasPrecision(16, 2);

            this.Property(t => t.ExemptTaxableOutput).HasColumnName("ExemptTaxableOutput").HasPrecision(16, 2);

            this.Property(t => t.OutputLinesCount).HasColumnName("OutputLinesCount");

            this.Property(t => t.OtherInputsTaxAmount).HasColumnName("OtherInputsTaxAmount").HasPrecision(16, 2);

            this.Property(t => t.EquipmentInputsTaxAmount).HasColumnName("EquipmentInputsTaxAmount").HasPrecision(16, 2);

            this.Property(t => t.InputLinesCount).HasColumnName("InputLinesCount");

            this.Property(t => t.AmountForPayRefund).HasColumnName("AmountForPayRefund").HasPrecision(16, 2);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ProcessStartDate).HasColumnName("ProcessStartDate");

            this.Property(t => t.ProcessEndDate).HasColumnName("ProcessEndDate");

            this.Property(t => t.ProcessProgress).HasColumnName("ProcessProgress");

            this.Property(t => t.NeedsRebulid).HasColumnName("NeedsRebulid");

            this.Property(t => t.CreatedInTwoMonthsLogic).HasColumnName("CreatedInTwoMonthsLogic");

            this.Property(t => t.OutputTaxAmountRound).HasColumnName("OutputTaxAmountRound").HasPrecision(16, 2);

            this.Property(t => t.InputsTaxAmountRound).HasColumnName("InputsTaxAmountRound").HasPrecision(16, 2);
        }
    }
}
	 