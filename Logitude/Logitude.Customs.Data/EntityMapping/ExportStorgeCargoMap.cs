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
 
    public class ExportStorgeCargoMap : EntityTypeConfiguration<ExportStorgeCargo>
    {
	    string dbms;
        public ExportStorgeCargoMap()
        { 
			  this.ToTable("ExportStorgeCargos", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.StorageID).HasColumnName("StorageID").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CargoTypeCode).HasColumnName("CargoTypeCode").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Manifest).HasColumnName("Manifest").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.SecondCargoID).HasColumnName("SecondCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.ThirdCargoID).HasColumnName("ThirdCargoID").HasMaxLength(35).IsUnicode(false);

            this.Property(t => t.CargoDescription).HasColumnName("CargoDescription").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.CargoType).HasColumnName("CargoType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.HandlingCode).HasColumnName("HandlingCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DangerousGoodsIndication).HasColumnName("DangerousGoodsIndication").HasPrecision(1, 0);

            this.Property(t => t.CodeBreaksIndication).HasColumnName("CodeBreaksIndication").HasPrecision(1, 0);

            this.Property(t => t.DamageCode).HasColumnName("DamageCode").HasPrecision(1, 0);

            this.Property(t => t.ForeignCurrencyType).HasColumnName("ForeignCurrencyType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ForeignCurrencyAmoun).HasColumnName("ForeignCurrencyAmoun").HasPrecision(14, 2);

            this.Property(t => t.GoodsValueNIS).HasColumnName("GoodsValueNIS").HasPrecision(14, 2);

            this.Property(t => t.PackageType).HasColumnName("PackageType").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Quantity).HasColumnName("Quantity").HasPrecision(8, 0);

            this.Property(t => t.MarksNumbers).HasColumnName("MarksNumbers").HasMaxLength(12).IsUnicode(true);

            this.Property(t => t.WeightInPortMandatory).HasColumnName("WeightInPortMandatory").HasPrecision(1, 0);

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(11, 3);

            this.Property(t => t.VolumeSize).HasColumnName("VolumeSize").HasPrecision(8, 0);

            this.Property(t => t.LicensePlateNumber).HasColumnName("LicensePlateNumber").HasMaxLength(11).IsUnicode(false);

            this.Property(t => t.CustomsItem).HasColumnName("CustomsItem").HasMaxLength(18).IsUnicode(true);

            this.Property(t => t.RiskLevel).HasColumnName("RiskLevel").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.DangerousSubstancename).HasColumnName("DangerousSubstancename").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.WeightVerificationNumber).HasColumnName("WeightVerificationNumber").HasMaxLength(10).IsUnicode(true);

            this.Property(t => t.ExporterReportedWeightID).HasColumnName("ExporterReportedWeightID").HasPrecision(9, 0);

            this.Property(t => t.ExporterReportedWeightName).HasColumnName("ExporterReportedWeightName").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.ContainerNumber).HasColumnName("ContainerNumber").HasMaxLength(12).IsUnicode(true);

            this.Property(t => t.CoolingActivated).HasColumnName("CoolingActivated").HasPrecision(1, 0);

            this.Property(t => t.RequiredTemperature).HasColumnName("RequiredTemperature").HasPrecision(3, 1);

            this.Property(t => t.PharmaGroceryIndication).HasColumnName("PharmaGroceryIndication").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.LeftException).HasColumnName("LeftException").HasPrecision(4, 0);

            this.Property(t => t.RightException).HasColumnName("RightException").HasPrecision(4, 0);

            this.Property(t => t.FrontException).HasColumnName("FrontException").HasPrecision(4, 0);

            this.Property(t => t.BackException).HasColumnName("BackException").HasPrecision(4, 0);

            this.Property(t => t.HeightException).HasColumnName("HeightException").HasPrecision(4, 0);

            this.Property(t => t.ContainerLineCode).HasColumnName("ContainerLineCode").HasMaxLength(2).IsUnicode(true);

            this.Property(t => t.VentValue).HasColumnName("VentValue").HasPrecision(3, 0);

            this.Property(t => t.HumidityPercentage).HasColumnName("HumidityPercentage").HasPrecision(3, 0);

            this.Property(t => t.Co2Percentage).HasColumnName("Co2Percentage").HasPrecision(2, 0);

            this.Property(t => t.O2Percentage).HasColumnName("O2Percentage").HasPrecision(2, 0);

            this.Property(t => t.SealNumber).HasColumnName("SealNumber").HasMaxLength(35).IsUnicode(true);

            this.Property(t => t.SealType).HasColumnName("SealType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CoolingReportingMethod).HasColumnName("CoolingReportingMethod").HasMaxLength(10).IsUnicode(false);

            this.Property(t => t.FullnessCode).HasColumnName("FullnessCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.OwnershipCode).HasColumnName("OwnershipCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ContainerTypeWCO).HasColumnName("ContainerTypeWCO").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.UNNumber).HasColumnName("UNNumber").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.RiskGroup).HasColumnName("RiskGroup").HasMaxLength(2).IsUnicode(true);
        }
    }
}
	 