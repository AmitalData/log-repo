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
 
    public class CB_ComputationMethodDataMap : EntityTypeConfiguration<CB_ComputationMethodData>
    {
	    string dbms;
        public CB_ComputationMethodDataMap()
        { 
			  this.ToTable("CB_ComputationMethodDatas", "Customs");
		
		    this.HasKey(t => new { t.ID });
	 
            this.Property(t => t.ID).HasColumnName("ID").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AlternateDefinedPerUnitMeasure).HasColumnName("AlternateDefinedPerUnitMeasure").HasPrecision(5, 2);

            this.Property(t => t.AlternateRate).HasColumnName("AlternateRate").HasPrecision(5, 2);

            this.Property(t => t.CalculationReference).HasColumnName("CalculationReference").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.ComputationMethodID).HasColumnName("ComputationMethodID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CurrencyTypeID).HasColumnName("CurrencyTypeID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.DefinedPerUnitMethod).HasColumnName("DefinedPerUnitMethod").HasPrecision(5, 2);

            this.Property(t => t.EnglishCalculationReference).HasColumnName("EnglishCalculationReference").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.MeasurementUnitID).HasColumnName("MeasurementUnitID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Alternate_MeasurementUnitID).HasColumnName("Alternate_MeasurementUnitID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.OptionalTaxAddition).HasColumnName("OptionalTaxAddition").HasPrecision(5, 2);

            this.Property(t => t.Rate).HasColumnName("Rate").HasPrecision(5, 2);

            this.Property(t => t.ReductionRate).HasColumnName("ReductionRate").HasPrecision(5, 2);

            this.Property(t => t.TariffRelatedToQuotaID).HasColumnName("TariffRelatedToQuotaID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.EnglishNotes).HasColumnName("EnglishNotes").HasMaxLength(255).IsUnicode(false);
        }
    }
}
	 