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
 
    public class CB_PropertiesDetailsHistoryMap : EntityTypeConfiguration<CB_PropertiesDetailsHistory>
    {
	    string dbms;
        public CB_PropertiesDetailsHistoryMap()
        { 
			  this.ToTable("CB_PropertiesDetailsHistorys", "Customs");
		
		    this.HasKey(t => new { t.CB_ID });
	 
            this.Property(t => t.ID).HasColumnName("ID");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.CustomsItemID).HasColumnName("CustomsItemID");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.EndDate).HasColumnName("EndDate");

            this.Property(t => t.EntityStatusID).HasColumnName("EntityStatusID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.ChangeRequestTypePriority).HasColumnName("ChangeRequestTypePriority");

            this.Property(t => t.IsCarItem).HasColumnName("IsCarItem");

            this.Property(t => t.IsConditionalExemptionItem).HasColumnName("IsConditionalExemptionItem");

            this.Property(t => t.IsCustomsItemDiscount).HasColumnName("IsCustomsItemDiscount");

            this.Property(t => t.IsEntitlementDiscount).HasColumnName("IsEntitlementDiscount");

            this.Property(t => t.IsGreenIndex).HasColumnName("IsGreenIndex");

            this.Property(t => t.IsHybridCar).HasColumnName("IsHybridCar");

            this.Property(t => t.IsImporterDiscount).HasColumnName("IsImporterDiscount");

            this.Property(t => t.IsIndexedLinked).HasColumnName("IsIndexedLinked");

            this.Property(t => t.IsNotAutonomiaUpdate).HasColumnName("IsNotAutonomiaUpdate");

            this.Property(t => t.IsRawMaterial).HasColumnName("IsRawMaterial");

            this.Property(t => t.IsWholesalePrice).HasColumnName("IsWholesalePrice");

            this.Property(t => t.VatDiscountReason).HasColumnName("VatDiscountReason").HasMaxLength(255).IsUnicode(false);

            this.Property(t => t.MaxSupervisionPeriod).HasColumnName("MaxSupervisionPeriod");

            this.Property(t => t.MeasurementUnitID).HasColumnName("MeasurementUnitID").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ConditionalExemptionTypeID).HasColumnName("ConditionalExemptionTypeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.FuelTypeID).HasColumnName("FuelTypeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsElectronic).HasColumnName("IsElectronic");

            this.Property(t => t.CarEngineVolumeID).HasColumnName("CarEngineVolumeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CarWeightID).HasColumnName("CarWeightID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.VatDiscountRate).HasColumnName("VatDiscountRate").HasPrecision(6, 2);

            this.Property(t => t.Discount_CustomItemGroupTypeID).HasColumnName("Discount_CustomItemGroupTypeID").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.IsCarDiscount).HasColumnName("IsCarDiscount");

            this.Property(t => t.DiscountRegularityRequiremType).HasColumnName("DiscountRegularityRequiremType").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CB_ID).HasColumnName("CB_ID").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 