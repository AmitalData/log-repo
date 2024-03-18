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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class CB_QuotaDetailsHistoryMap : EntityTypeConfiguration<CB_QuotaDetailsHistory>
    {
	    string dbms;
        public CB_QuotaDetailsHistoryMap()
        { 
			  this.ToTable("CB_QuotaDetailsHistorys", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsImportLicenseRequired).HasColumnName("IsImportLicenseRequired");

            this.Property(t => t.MeasurementUnitID).HasColumnName("MeasurementUnitID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity");

            this.Property(t => t.QuotaComputationBasisID).HasColumnName("QuotaComputationBasisID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.QuotaIncrementID).HasColumnName("QuotaIncrementID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RenewalMethodID).HasColumnName("RenewalMethodID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RenewalUntilDate).HasColumnName("RenewalUntilDate");

            this.Property(t => t.QuotaID).HasColumnName("QuotaID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");

            this.Property(t => t.QuotaValueIncrement).HasColumnName("QuotaValueIncrement").HasPrecision(12, 2);

            this.Property(t => t.PerYearFrequency).HasColumnName("PerYearFrequency").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CurrencyTypeID).HasColumnName("CurrencyTypeID").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 