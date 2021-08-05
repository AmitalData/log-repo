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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPMap : EntityTypeConfiguration<QuoteOP>
    {
	    string dbms;
        public QuoteOPMap()
        { 
				this.ToTable("QuoteOPs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.LastVersionNumber).HasColumnName("LastVersionNumber");

            this.Property(t => t.FreelancerId).HasColumnName("FreelancerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FreelancerAddressId).HasColumnName("FreelancerAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FreelancerContactId).HasColumnName("FreelancerContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LastModified).HasColumnName("LastModified").IsRequired();

            this.Property(t => t.Field1).HasColumnName("Field1").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field2).HasColumnName("Field2").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field3).HasColumnName("Field3").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field4).HasColumnName("Field4").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field5).HasColumnName("Field5").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field6).HasColumnName("Field6").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field7).HasColumnName("Field7").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field8).HasColumnName("Field8").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field9).HasColumnName("Field9").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field10).HasColumnName("Field10").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.IsByKG).HasColumnName("IsByKG");

            this.Property(t => t.IsByContainer).HasColumnName("IsByContainer");

            this.Property(t => t.EstimateProfitEdited).HasColumnName("EstimateProfitEdited");

            this.Property(t => t.FromAddressId).HasColumnName("FromAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressId).HasColumnName("ToAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MinimumFreightCost).HasColumnName("MinimumFreightCost");

            this.Property(t => t.MinimumFreightSale).HasColumnName("MinimumFreightSale");

            this.Property(t => t.LastStageDate).HasColumnName("LastStageDate");

            this.Property(t => t.AgentReference1).HasColumnName("AgentReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.AgentReference2).HasColumnName("AgentReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.IsSaleCurrencySameAsCost).HasColumnName("IsSaleCurrencySameAsCost");

            this.Property(t => t.EstimateProfit).HasColumnName("EstimateProfit");

            this.Property(t => t.IsFixedPrice).HasColumnName("IsFixedPrice");

            this.Property(t => t.CustomerContactId).HasColumnName("CustomerContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperName).HasColumnName("ShipperName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.DeliveryAddress).HasColumnName("DeliveryAddress").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.PickUpAddress).HasColumnName("PickUpAddress").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.SaleCurrencyId).HasColumnName("SaleCurrencyId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate").IsRequired();

            this.Property(t => t.CustomerName).HasColumnName("CustomerName").HasMaxLength(100).IsUnicode(true);

            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");

            this.Property(t => t.QuoteNumber).HasColumnName("QuoteNumber").IsRequired().HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DirectionId).HasColumnName("DirectionId").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").IsRequired().HasMaxLength(1).IsFixedLength();

            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BranchId).HasColumnName("BranchId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.QuoteCustomerTypeCode).HasColumnName("QuoteCustomerTypeCode").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.CustomerId).HasColumnName("CustomerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperId).HasColumnName("ShipperId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperContactId).HasColumnName("ShipperContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipperReference1).HasColumnName("ShipperReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ShipperReference2).HasColumnName("ShipperReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeContactId).HasColumnName("ConsigneeContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ConsigneeReference1).HasColumnName("ConsigneeReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.ConsigneeReference2).HasColumnName("ConsigneeReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IncotermId).HasColumnName("IncotermId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OpenDate).HasColumnName("OpenDate").IsRequired();

            this.Property(t => t.Notes).HasColumnName("Notes").HasMaxLength(500).IsUnicode(true);

            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods").HasMaxLength(512).IsUnicode(true);

            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");

            this.Property(t => t.IsClosed).HasColumnName("IsClosed");

            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Volume).HasColumnName("Volume");

            this.Property(t => t.Ratio).HasColumnName("Ratio");

            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");

            this.Property(t => t.NumberOfContainers).HasColumnName("NumberOfContainers");

            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous");

            this.Property(t => t.ExpirationDays).HasColumnName("ExpirationDays");

            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");

            this.Property(t => t.IsFreightBySteps).HasColumnName("IsFreightBySteps");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1500).IsUnicode(true);

            this.Property(t => t.PackageType1Id).HasColumnName("PackageType1Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageType2Id).HasColumnName("PackageType2Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageType3Id).HasColumnName("PackageType3Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageType4Id).HasColumnName("PackageType4Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageType5Id).HasColumnName("PackageType5Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PackageType1Quantity).HasColumnName("PackageType1Quantity");

            this.Property(t => t.PackageType2Quantity).HasColumnName("PackageType2Quantity");

            this.Property(t => t.PackageType3Quantity).HasColumnName("PackageType3Quantity");

            this.Property(t => t.PackageType4Quantity).HasColumnName("PackageType4Quantity");

            this.Property(t => t.PackageType5Quantity).HasColumnName("PackageType5Quantity");

            this.Property(t => t.QuoteTypeCode).HasColumnName("QuoteTypeCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");

            this.Property(t => t.FromPartnerId).HasColumnName("FromPartnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPartnerId).HasColumnName("ToPartnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromPartnerAddressId).HasColumnName("FromPartnerAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPartnerAddressId).HasColumnName("ToPartnerAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromAddressCity).HasColumnName("FromAddressCity").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.FromAddressCountryId).HasColumnName("FromAddressCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FromAddressZipCode).HasColumnName("FromAddressZipCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressCity).HasColumnName("ToAddressCity").HasMaxLength(25).IsUnicode(true);

            this.Property(t => t.ToAddressCountryId).HasColumnName("ToAddressCountryId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToAddressZipCode).HasColumnName("ToAddressZipCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.DimFactor).HasColumnName("DimFactor");

            this.Property(t => t.IncludePickUp).HasColumnName("IncludePickUp");

            this.Property(t => t.IncludeDelivery).HasColumnName("IncludeDelivery");

            this.Property(t => t.QuoteClosingReasonCode).HasColumnName("QuoteClosingReasonCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.SentDate).HasColumnName("SentDate");

            this.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");

            this.Property(t => t.DeclinedDate).HasColumnName("DeclinedDate");

            this.Property(t => t.UsageCount).HasColumnName("UsageCount");

            this.Property(t => t.LastUsageDate).HasColumnName("LastUsageDate");

            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId").IsRequired().HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CustomerReference1).HasColumnName("CustomerReference1").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CustomerReference2).HasColumnName("CustomerReference2").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.Subject).HasColumnName("Subject").HasMaxLength(200).IsUnicode(true);

            this.Property(t => t.IsSubjectEdited).HasColumnName("IsSubjectEdited");

            this.Property(t => t.StageId).HasColumnName("StageId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StageDueDate).HasColumnName("StageDueDate");

            this.Property(t => t.RatingCode).HasColumnName("RatingCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");

            this.Property(t => t.LastActivitySubject).HasColumnName("LastActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.LastActivityTypeCode).HasColumnName("LastActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.NextActivityDate).HasColumnName("NextActivityDate");

            this.Property(t => t.NextActivitySubject).HasColumnName("NextActivitySubject").HasMaxLength(255).IsUnicode(true);

            this.Property(t => t.NextActivityTypeCode).HasColumnName("NextActivityTypeCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.IsAutomaticallyClosed).HasColumnName("IsAutomaticallyClosed");

            this.Property(t => t.AutomaticallyCloseDate).HasColumnName("AutomaticallyCloseDate");

            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            this.Property(t => t.AutomaticallyCloseDays).HasColumnName("AutomaticallyCloseDays");

            this.Property(t => t.ProductCode).HasColumnName("ProductCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.TransitTime).HasColumnName("TransitTime").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.DepartureFrequency).HasColumnName("DepartureFrequency").HasMaxLength(30).IsUnicode(true);

            this.Property(t => t.ETD).HasColumnName("ETD");

            this.Property(t => t.ETA).HasColumnName("ETA");

            this.Property(t => t.AgentId).HasColumnName("AgentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AgentAddressId).HasColumnName("AgentAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AgentContactId).HasColumnName("AgentContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.MoveTypeId).HasColumnName("MoveTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.TEU).HasColumnName("TEU");

            this.Property(t => t.ValueOfGoods).HasColumnName("ValueOfGoods");

            this.Property(t => t.ValueOfGoodsCurrencyId).HasColumnName("ValueOfGoodsCurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.IsChargesByVAT).HasColumnName("IsChargesByVAT");

            this.Property(t => t.IsQuoteDataExternal).HasColumnName("IsQuoteDataExternal");

            this.Property(t => t.IsQuoteDocumentExternal).HasColumnName("IsQuoteDocumentExternal");

            this.Property(t => t.TotalPerContainer).HasColumnName("TotalPerContainer");

            this.Property(t => t.QuotationSections).HasColumnName("QuotationSections").HasMaxLength(500).IsUnicode(false);

            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG");

            this.Property(t => t.GrossWeightPerTon).HasColumnName("GrossWeightPerTon");

            this.Property(t => t.NotifyId).HasColumnName("NotifyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NotifyAddressId).HasColumnName("NotifyAddressId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NotifyContactId).HasColumnName("NotifyContactId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.NumberOfFollowUps).HasColumnName("NumberOfFollowUps");

            this.Property(t => t.GrossWeightEdited).HasColumnName("GrossWeightEdited");

            this.Property(t => t.ChargeableWeightEdited).HasColumnName("ChargeableWeightEdited");

            this.Property(t => t.ChargeableWeightInKG).HasColumnName("ChargeableWeightInKG");

            this.Property(t => t.VolumeInCBM).HasColumnName("VolumeInCBM");

            this.Property(t => t.StartDate).HasColumnName("StartDate");

            this.Property(t => t.Field11).HasColumnName("Field11").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field12).HasColumnName("Field12").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field13).HasColumnName("Field13").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field14).HasColumnName("Field14").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field15).HasColumnName("Field15").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field16).HasColumnName("Field16").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field17).HasColumnName("Field17").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field18).HasColumnName("Field18").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field19).HasColumnName("Field19").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.Field20).HasColumnName("Field20").HasMaxLength(250).IsUnicode(true);

            this.Property(t => t.RequestDate).HasColumnName("RequestDate");

            this.Property(t => t.EstimatedProfitInLocal).HasColumnName("EstimatedProfitInLocal");

            this.Property(t => t.EstimatedProfitInProfit).HasColumnName("EstimatedProfitInProfit");

            this.Property(t => t.ProfitCurrencyId).HasColumnName("ProfitCurrencyId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ProfitExchangeRate).HasColumnName("ProfitExchangeRate");

            this.Property(t => t.CountryForStatisticsId).HasColumnName("CountryForStatisticsId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteHTMLDocumentId).HasColumnName("QuoteHTMLDocumentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.QuoteClosingReasonId).HasColumnName("QuoteClosingReasonId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentSubTypeId).HasColumnName("ShipmentSubTypeId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PickupDeliveryRatio).HasColumnName("PickupDeliveryRatio");

            this.Property(t => t.PickupDeliveryChargeableWeight).HasColumnName("PickupDeliveryChargeableWeight");

            this.Property(t => t.PickupDeliveryVolumetricWeight).HasColumnName("PickupDeliveryVolumetricWeight");

            this.Property(t => t.PickupDeliveryCWeightUnitCode).HasColumnName("PickupDeliveryCWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.RegionalTaxId).HasColumnName("RegionalTaxId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RegionalTaxPercentage).HasColumnName("RegionalTaxPercentage");

            this.Property(t => t.DescriptionRightToLeft).HasColumnName("DescriptionRightToLeft").IsRequired();

            this.Property(t => t.AutomaticLastUpdateDate).HasColumnName("AutomaticLastUpdateDate");

            this.Property(t => t.IsMultiCurrency).HasColumnName("IsMultiCurrency");
        }
    }
}
	 