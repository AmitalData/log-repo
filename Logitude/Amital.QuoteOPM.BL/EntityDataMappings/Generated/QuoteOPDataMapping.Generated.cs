
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPDataMapping: IMapping<QuoteOPPM, QuoteOP>,IMappingEncodeBase64NVARCHARFields<QuoteOPPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteTemplateId, 
	         ConcurrencyGUID, 
	         LastVersionNumber, 
	         FreelancerId, 
	         FreelancerAddressId, 
	         FreelancerContactId, 
	         LastModified, 
	         Field1, 
	         Field2, 
	         Field3, 
	         Field4, 
	         Field5, 
	         Field6, 
	         Field7, 
	         Field8, 
	         Field9, 
	         Field10, 
	         IsByKG, 
	         IsByContainer, 
	         EstimateProfitEdited, 
	         FromAddressId, 
	         ToAddressId, 
	         OpportunityId, 
	         MinimumFreightCost, 
	         MinimumFreightSale, 
	         LastStageDate, 
	         AgentReference1, 
	         AgentReference2, 
	         IsSaleCurrencySameAsCost, 
	         EstimateProfit, 
	         IsFixedPrice, 
	         CustomerContactId, 
	         ShipperName, 
	         ConsigneeName, 
	         DeliveryAddress, 
	         PickUpAddress, 
	         SaleCurrencyId, 
	         ExchangeRate, 
	         CustomerName, 
	         IsCancelled, 
	         QuoteNumber, 
	         MainCarriageCarrierId, 
	         DirectionId, 
	         TransportModeId, 
	         DepartmentId, 
	         BranchId, 
	         ShipmentTypeId, 
	         QuoteCustomerTypeCode, 
	         CustomerId, 
	         ShipperId, 
	         ShipperContactId, 
	         ShipperReference1, 
	         ShipperReference2, 
	         ConsigneeId, 
	         ConsigneeContactId, 
	         ConsigneeReference1, 
	         ConsigneeReference2, 
	         FromPortId, 
	         ToPortId, 
	         IncotermId, 
	         SalesmanUserId, 
	         CreatedByUserId, 
	         OpenDate, 
	         Notes, 
	         DescriptionOfGoods, 
	         ChargeableWeight, 
	         GrossWeight, 
	         IsClosed, 
	         DimensionsUnitCode, 
	         Volume, 
	         Ratio, 
	         NumberOfPackages, 
	         NumberOfContainers, 
	         VolumeUnitCode, 
	         IsDangerous, 
	         ExpirationDays, 
	         ExpirationDate, 
	         IsFreightBySteps, 
	         SearchFields, 
	         PackageType1Id, 
	         PackageType2Id, 
	         PackageType3Id, 
	         PackageType4Id, 
	         PackageType5Id, 
	         PackageType1Quantity, 
	         PackageType2Quantity, 
	         PackageType3Quantity, 
	         PackageType4Quantity, 
	         PackageType5Quantity, 
	         QuoteTypeCode, 
	         GrossWeightUnitCode, 
	         ChargeableWeightUnitCode, 
	         VolumetricWeight, 
	         FromPartnerId, 
	         ToPartnerId, 
	         FromPartnerAddressId, 
	         ToPartnerAddressId, 
	         FromAddressCity, 
	         FromAddressCountryId, 
	         FromAddressZipCode, 
	         ToAddressCity, 
	         ToAddressCountryId, 
	         ToAddressZipCode, 
	         DimFactor, 
	         IncludePickUp, 
	         IncludeDelivery, 
	         QuoteClosingReasonCode, 
	         SentDate, 
	         AcceptedDate, 
	         DeclinedDate, 
	         UsageCount, 
	         LastUsageDate, 
	         BusinessUnitId, 
	         CustomerReference1, 
	         CustomerReference2, 
	         Subject, 
	         IsSubjectEdited, 
	         StageId, 
	         StageDueDate, 
	         RatingCode, 
	         LastActivityDate, 
	         LastActivitySubject, 
	         LastActivityTypeCode, 
	         NextActivityDate, 
	         NextActivitySubject, 
	         NextActivityTypeCode, 
	         IsAutomaticallyClosed, 
	         AutomaticallyCloseDate, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         AutomaticallyCloseDays, 
	         ProductCode, 
	         TransitTime, 
	         DepartureFrequency, 
	         ETD, 
	         ETA, 
	         AgentId, 
	         AgentAddressId, 
	         AgentContactId, 
	         MoveTypeId, 
	         TEU, 
	         ValueOfGoods, 
	         ValueOfGoodsCurrencyId, 
	         IsChargesByVAT, 
	         IsQuoteDataExternal, 
	         IsQuoteDocumentExternal, 
	         TotalPerContainer, 
	         QuotationSections, 
	         GrossWeightInKG, 
	         GrossWeightPerTon, 
	         NotifyId, 
	         NotifyAddressId, 
	         NotifyContactId, 
	         NumberOfFollowUps, 
	         GrossWeightEdited, 
	         ChargeableWeightEdited, 
	         ChargeableWeightInKG, 
	         VolumeInCBM, 
	         StartDate, 
	         Field11, 
	         Field12, 
	         Field13, 
	         Field14, 
	         Field15, 
	         Field16, 
	         Field17, 
	         Field18, 
	         Field19, 
	         Field20, 
	         RequestDate, 
	         EstimatedProfitInLocal, 
	         EstimatedProfitInProfit, 
	         ProfitCurrencyId, 
	         ProfitExchangeRate, 
	         CountryForStatisticsId, 
	         QuoteHTMLDocumentId, 
	         QuoteClosingReasonId, 
	         ShipmentSubTypeId, 
	         PickupDeliveryRatio, 
	         PickupDeliveryChargeableWeight, 
	         PickupDeliveryVolumetricWeight, 
	         PickupDeliveryCWeightUnitCode, 
	         RegionalTaxId, 
	         RegionalTaxPercentage, 
	         DescriptionRightToLeft, 
	         AutomaticLastUpdateDate, 
	         IsMultiCurrency, 
	         SpecialServiceId, 
	         FromAddressId, 
	         ToAddressId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         QuoteTemplateId, 
	         ConcurrencyGUID, 
	         LastVersionNumber, 
	         FreelancerId, 
	         FreelancerAddressId, 
	         FreelancerContactId, 
	         LastModified, 
	         Field1, 
	         Field2, 
	         Field3, 
	         Field4, 
	         Field5, 
	         Field6, 
	         Field7, 
	         Field8, 
	         Field9, 
	         Field10, 
	         IsByKG, 
	         IsByContainer, 
	         EstimateProfitEdited, 
	         OpportunityId, 
	         LastStageDate, 
	         AgentReference1, 
	         AgentReference2, 
	         IsSaleCurrencySameAsCost, 
	         EstimateProfit, 
	         IsFixedPrice, 
	         CustomerContactId, 
	         CostTotalAmountInLocalCurrency, 
	         SaleTotalAmountInLocalCurrency, 
	         CostTotalAmountInSaleCurrency, 
	         SaleTotalAmountInSaleCurrency, 
	         EstimateProfitInSaleCurrency, 
	         ShipperName, 
	         ConsigneeName, 
	         DeliveryAddress, 
	         PickUpAddress, 
	         SaleCurrencyId, 
	         ExchangeRate, 
	         CustomerName, 
	         CustomerNote, 
	         IsCancelled, 
	         TotalReceivablesAmount, 
	         QuoteNumber, 
	         MainCarriageCarrierId, 
	         DirectionId, 
	         TransportModeId, 
	         DepartmentId, 
	         BranchId, 
	         ShipmentTypeId, 
	         QuoteCustomerTypeCode, 
	         CustomerId, 
	         ShipperId, 
	         ShipperContactId, 
	         ShipperReference1, 
	         ShipperReference2, 
	         ConsigneeId, 
	         ConsigneeContactId, 
	         ConsigneeReference1, 
	         ConsigneeReference2, 
	         FromPortId, 
	         FromPort, 
	         ToPortId, 
	         ToPort, 
	         IncotermId, 
	         SalesmanUserId, 
	         CreatedByUserId, 
	         OpenDate, 
	         Notes, 
	         DescriptionOfGoods, 
	         ChargeableWeight, 
	         GrossWeight, 
	         IsClosed, 
	         DimensionsUnitCode, 
	         Volume, 
	         Ratio, 
	         NumberOfPackages, 
	         NumberOfContainers, 
	         VolumeUnitCode, 
	         IsDangerous, 
	         ExpirationDays, 
	         ExpirationDate, 
	         IsFreightBySteps, 
	         TotalContainers, 
	         SearchFields, 
	         PackageType1Id, 
	         PackageType2Id, 
	         PackageType3Id, 
	         PackageType4Id, 
	         PackageType5Id, 
	         PackageType1Quantity, 
	         PackageType2Quantity, 
	         PackageType3Quantity, 
	         PackageType4Quantity, 
	         PackageType5Quantity, 
	         QuoteTypeCode, 
	         QuoteTypeName, 
	         GrossWeightUnitCode, 
	         ChargeableWeightUnitCode, 
	         VolumetricWeight, 
	         PickupLocation, 
	         DeliveryLocation, 
	         DepartmentName, 
	         BranchName, 
	         FromPartnerId, 
	         ToPartnerId, 
	         FromPartnerAddressId, 
	         ToPartnerAddressId, 
	         FromLocation, 
	         ToLocation, 
	         FromAddressCity, 
	         FromAddressCountryId, 
	         FromAddressZipCode, 
	         ToAddressCity, 
	         ToAddressCountryId, 
	         ToAddressZipCode, 
	         DimFactor, 
	         IncludePickUp, 
	         IncludeDelivery, 
	         PickUpAddressId, 
	         DeliveryAddressId, 
	         QuoteClosingReasonCode, 
	         SentDate, 
	         AcceptedDate, 
	         DeclinedDate, 
	         UsageCount, 
	         LastUsageDate, 
	         BusinessUnitId, 
	         CustomerReference1, 
	         CustomerReference2, 
	         Subject, 
	         IsSubjectEdited, 
	         SalesmanName, 
	         IncotermCode, 
	         StageId, 
	         StageName, 
	         StageDueDate, 
	         RatingCode, 
	         LastActivityDate, 
	         LastActivitySubject, 
	         LastActivityTypeCode, 
	         NextActivityDate, 
	         NextActivitySubject, 
	         NextActivityTypeCode, 
	         RatingName, 
	         StageMaxDays, 
	         IsAutomaticallyClosed, 
	         AutomaticallyCloseDate, 
	         UpdatedByUserId, 
	         UpdateDate, 
	         AutomaticallyCloseDays, 
	         ProductCode, 
	         ETDLabel, 
	         ETALabel, 
	         TransitTime, 
	         DepartureFrequency, 
	         ETD, 
	         ETA, 
	         AgentId, 
	         AgentName, 
	         AgentAddressId, 
	         AgentContactId, 
	         MoveTypeId, 
	         TEU, 
	         SalesTotalAmounts, 
	         ValueOfGoods, 
	         ValueOfGoodsCurrencyId, 
	         IsChargesByVAT, 
	         IsSecured, 
	         DirectionName, 
	         TransportModeName, 
	         IsCustomerSet, 
	         BaseShipmentNumber, 
	         SaleCurrencyCode, 
	         FreelancerName, 
	         ShipperNote, 
	         ConsigneeNote, 
	         ShipperMainAddressId, 
	         ShipperPickAddressId, 
	         ConsigneeMainAddressId, 
	         ConsigneePickAddressId, 
	         CustomerRankName, 
	         FromPortName, 
	         FromPortCountry, 
	         ToPortName, 
	         ToPortCountry, 
	         ActionType, 
	         EventNote, 
	         IsCopy, 
	         ToCountryId, 
	         FromCountryId, 
	         FromCountryIsEC, 
	         ToCountryIsEC, 
	         FromPartnerName, 
	         ToPartnerName, 
	         IsPotentialShipper, 
	         IsPotentialConsignee, 
	         IncotermName, 
	         FromCountryCode, 
	         FromCountryName, 
	         ToCountryCode, 
	         ToCountryName, 
	         IsHybrid, 
	         MarkFollowUpsAsDone, 
	         IsQuoteDataExternal, 
	         IsQuoteDocumentExternal, 
	         TotalPerContainer, 
	         QuotationSections, 
	         SameOrFixed, 
	         GrossWeightInKG, 
	         GrossWeightPerTon, 
	         NotifyId, 
	         NotifyAddressId, 
	         NotifyContactId, 
	         NotifyName, 
	         TotalSaleIncludingVATAmountInSaleCurrency, 
	         TotalSaleIncludingVATAmountInLocalCurrency, 
	         NumberOfFollowUps, 
	         NotifyNote, 
	         NotifyAddress1, 
	         NotifyAddress2, 
	         NotifyZipCode, 
	         NotifyStateId, 
	         NotifyCountryId, 
	         NotifyCity, 
	         DontExportQuotationsToIntegratedSystem, 
	         QuoteLevel, 
	         ConvertToLCL, 
	         ConvertToFCL, 
	         GrossWeightEdited, 
	         ChargeableWeightEdited, 
	         ChargeableWeightInKG, 
	         VolumeInCBM, 
	         StartDate, 
	         IsCopyExchangeRates, 
	         QuoteVersion, 
	         Field11, 
	         Field12, 
	         Field13, 
	         Field14, 
	         Field15, 
	         Field16, 
	         Field17, 
	         Field18, 
	         Field19, 
	         Field20, 
	         RequestDate, 
	         IsCreatedFromTicket, 
	         TicketCreateDate, 
	         EstimatedProfitInLocal, 
	         EstimatedProfitInProfit, 
	         ProfitCurrencyId, 
	         ProfitExchangeRate, 
	         CountryForStatisticsId, 
	         ExternalEntityNumber, 
	         QuoteHTMLDocumentId, 
	         QuoteClosingReasonId, 
	         ShipmentSubTypeId, 
	         ShipmentSubTypeName, 
	         DeliveryCity, 
	         DeliveryCountryId, 
	         DeliveryZipCode, 
	         PickupCity, 
	         PickupCountryId, 
	         PickupZipCode, 
	         PickupDeliveryRatio, 
	         PickupDeliveryChargeableWeight, 
	         PickupDeliveryVolumetricWeight, 
	         PickupDeliveryCWeightUnitCode, 
	         RegionalTaxId, 
	         RegionalTaxPercentage, 
	         DescriptionRightToLeft, 
	         IsMultiCurrency, 
	         IsRefreshFollowUp, 
	         IsRefreshQuoteFollowUps, 
	         ConvertTransportMode, 
	         SpecialServiceId, 
	         SpecialServiceName, 
	         FromAddressId, 
	         ToAddressId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
				entityPOCO.QuoteTemplateId = entityPM.QuoteTemplateId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastVersionNumber))
            {
				entityPOCO.LastVersionNumber = entityPM.LastVersionNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerId))
            {
				entityPOCO.FreelancerId = entityPM.FreelancerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerAddressId))
            {
				entityPOCO.FreelancerAddressId = entityPM.FreelancerAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerContactId))
            {
				entityPOCO.FreelancerContactId = entityPM.FreelancerContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastModified))
            {
				entityPOCO.LastModified = entityPM.LastModified;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field1))
            {
				entityPOCO.Field1 = entityPM.Field1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field2))
            {
				entityPOCO.Field2 = entityPM.Field2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field3))
            {
				entityPOCO.Field3 = entityPM.Field3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field4))
            {
				entityPOCO.Field4 = entityPM.Field4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field5))
            {
				entityPOCO.Field5 = entityPM.Field5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field6))
            {
				entityPOCO.Field6 = entityPM.Field6;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field7))
            {
				entityPOCO.Field7 = entityPM.Field7;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field8))
            {
				entityPOCO.Field8 = entityPM.Field8;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field9))
            {
				entityPOCO.Field9 = entityPM.Field9;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field10))
            {
				entityPOCO.Field10 = entityPM.Field10;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsByKG))
            {
				entityPOCO.IsByKG = entityPM.IsByKG;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsByContainer))
            {
				entityPOCO.IsByContainer = entityPM.IsByContainer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimateProfitEdited))
            {
				entityPOCO.EstimateProfitEdited = entityPM.EstimateProfitEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
				entityPOCO.OpportunityId = entityPM.OpportunityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageDate))
            {
				entityPOCO.LastStageDate = entityPM.LastStageDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentReference1))
            {
				entityPOCO.AgentReference1 = entityPM.AgentReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentReference2))
            {
				entityPOCO.AgentReference2 = entityPM.AgentReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSaleCurrencySameAsCost))
            {
				entityPOCO.IsSaleCurrencySameAsCost = entityPM.IsSaleCurrencySameAsCost;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimateProfit))
            {
				entityPOCO.EstimateProfit = entityPM.EstimateProfit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFixedPrice))
            {
				entityPOCO.IsFixedPrice = entityPM.IsFixedPrice;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerContactId))
            {
				entityPOCO.CustomerContactId = entityPM.CustomerContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
				entityPOCO.ShipperName = entityPM.ShipperName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
				entityPOCO.ConsigneeName = entityPM.ConsigneeName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryAddress))
            {
				entityPOCO.DeliveryAddress = entityPM.DeliveryAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickUpAddress))
            {
				entityPOCO.PickUpAddress = entityPM.PickUpAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleCurrencyId))
            {
				entityPOCO.SaleCurrencyId = entityPM.SaleCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
				entityPOCO.ExchangeRate = entityPM.ExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerName))
            {
				entityPOCO.CustomerName = entityPM.CustomerName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteNumber))
            {
				entityPOCO.QuoteNumber = entityPM.QuoteNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
				entityPOCO.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
				entityPOCO.DirectionId = entityPM.DirectionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartmentId))
            {
				entityPOCO.DepartmentId = entityPM.DepartmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchId))
            {
				entityPOCO.BranchId = entityPM.BranchId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
				entityPOCO.ShipmentTypeId = entityPM.ShipmentTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCustomerTypeCode))
            {
				entityPOCO.QuoteCustomerTypeCode = entityPM.QuoteCustomerTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperContactId))
            {
				entityPOCO.ShipperContactId = entityPM.ShipperContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference1))
            {
				entityPOCO.ShipperReference1 = entityPM.ShipperReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference2))
            {
				entityPOCO.ShipperReference2 = entityPM.ShipperReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeContactId))
            {
				entityPOCO.ConsigneeContactId = entityPM.ConsigneeContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference1))
            {
				entityPOCO.ConsigneeReference1 = entityPM.ConsigneeReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference2))
            {
				entityPOCO.ConsigneeReference2 = entityPM.ConsigneeReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
				entityPOCO.FromPortId = entityPM.FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
				entityPOCO.ToPortId = entityPM.ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
				entityPOCO.IncotermId = entityPM.IncotermId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesmanUserId))
            {
				entityPOCO.SalesmanUserId = entityPM.SalesmanUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
				entityPOCO.OpenDate = entityPM.OpenDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
				entityPOCO.DescriptionOfGoods = entityPM.DescriptionOfGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
				entityPOCO.ChargeableWeight = entityPM.ChargeableWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
				entityPOCO.IsClosed = entityPM.IsClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
				entityPOCO.DimensionsUnitCode = entityPM.DimensionsUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
				entityPOCO.Ratio = entityPM.Ratio;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPackages))
            {
				entityPOCO.NumberOfPackages = entityPM.NumberOfPackages;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfContainers))
            {
				entityPOCO.NumberOfContainers = entityPM.NumberOfContainers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
				entityPOCO.VolumeUnitCode = entityPM.VolumeUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
				entityPOCO.IsDangerous = entityPM.IsDangerous;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDays))
            {
				entityPOCO.ExpirationDays = entityPM.ExpirationDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
				entityPOCO.ExpirationDate = entityPM.ExpirationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFreightBySteps))
            {
				entityPOCO.IsFreightBySteps = entityPM.IsFreightBySteps;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType1Id))
            {
				entityPOCO.PackageType1Id = entityPM.PackageType1Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType2Id))
            {
				entityPOCO.PackageType2Id = entityPM.PackageType2Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType3Id))
            {
				entityPOCO.PackageType3Id = entityPM.PackageType3Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType4Id))
            {
				entityPOCO.PackageType4Id = entityPM.PackageType4Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType5Id))
            {
				entityPOCO.PackageType5Id = entityPM.PackageType5Id;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType1Quantity))
            {
				entityPOCO.PackageType1Quantity = entityPM.PackageType1Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType2Quantity))
            {
				entityPOCO.PackageType2Quantity = entityPM.PackageType2Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType3Quantity))
            {
				entityPOCO.PackageType3Quantity = entityPM.PackageType3Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType4Quantity))
            {
				entityPOCO.PackageType4Quantity = entityPM.PackageType4Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType5Quantity))
            {
				entityPOCO.PackageType5Quantity = entityPM.PackageType5Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTypeCode))
            {
				entityPOCO.QuoteTypeCode = entityPM.QuoteTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
				entityPOCO.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
				entityPOCO.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
				entityPOCO.VolumetricWeight = entityPM.VolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerId))
            {
				entityPOCO.FromPartnerId = entityPM.FromPartnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerId))
            {
				entityPOCO.ToPartnerId = entityPM.ToPartnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerAddressId))
            {
				entityPOCO.FromPartnerAddressId = entityPM.FromPartnerAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerAddressId))
            {
				entityPOCO.ToPartnerAddressId = entityPM.ToPartnerAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCity))
            {
				entityPOCO.FromAddressCity = entityPM.FromAddressCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCountryId))
            {
				entityPOCO.FromAddressCountryId = entityPM.FromAddressCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressZipCode))
            {
				entityPOCO.FromAddressZipCode = entityPM.FromAddressZipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCity))
            {
				entityPOCO.ToAddressCity = entityPM.ToAddressCity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCountryId))
            {
				entityPOCO.ToAddressCountryId = entityPM.ToAddressCountryId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressZipCode))
            {
				entityPOCO.ToAddressZipCode = entityPM.ToAddressZipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimFactor))
            {
				entityPOCO.DimFactor = entityPM.DimFactor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncludePickUp))
            {
				entityPOCO.IncludePickUp = entityPM.IncludePickUp;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncludeDelivery))
            {
				entityPOCO.IncludeDelivery = entityPM.IncludeDelivery;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteClosingReasonCode))
            {
				entityPOCO.QuoteClosingReasonCode = entityPM.QuoteClosingReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SentDate))
            {
				entityPOCO.SentDate = entityPM.SentDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AcceptedDate))
            {
				entityPOCO.AcceptedDate = entityPM.AcceptedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclinedDate))
            {
				entityPOCO.DeclinedDate = entityPM.DeclinedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UsageCount))
            {
				entityPOCO.UsageCount = entityPM.UsageCount;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUsageDate))
            {
				entityPOCO.LastUsageDate = entityPM.LastUsageDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
				entityPOCO.BusinessUnitId = entityPM.BusinessUnitId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference1))
            {
				entityPOCO.CustomerReference1 = entityPM.CustomerReference1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference2))
            {
				entityPOCO.CustomerReference2 = entityPM.CustomerReference2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
				entityPOCO.Subject = entityPM.Subject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSubjectEdited))
            {
				entityPOCO.IsSubjectEdited = entityPM.IsSubjectEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
				entityPOCO.StageId = entityPM.StageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageDueDate))
            {
				entityPOCO.StageDueDate = entityPM.StageDueDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RatingCode))
            {
				entityPOCO.RatingCode = entityPM.RatingCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivityDate))
            {
				entityPOCO.LastActivityDate = entityPM.LastActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivitySubject))
            {
				entityPOCO.LastActivitySubject = entityPM.LastActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivityTypeCode))
            {
				entityPOCO.LastActivityTypeCode = entityPM.LastActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
				entityPOCO.NextActivityDate = entityPM.NextActivityDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
				entityPOCO.NextActivitySubject = entityPM.NextActivitySubject;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
				entityPOCO.NextActivityTypeCode = entityPM.NextActivityTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAutomaticallyClosed))
            {
				entityPOCO.IsAutomaticallyClosed = entityPM.IsAutomaticallyClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDate))
            {
				entityPOCO.AutomaticallyCloseDate = entityPM.AutomaticallyCloseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDays))
            {
				entityPOCO.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductCode))
            {
				entityPOCO.ProductCode = entityPM.ProductCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransitTime))
            {
				entityPOCO.TransitTime = entityPM.TransitTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureFrequency))
            {
				entityPOCO.DepartureFrequency = entityPM.DepartureFrequency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
				entityPOCO.ETA = entityPM.ETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
				entityPOCO.AgentId = entityPM.AgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentAddressId))
            {
				entityPOCO.AgentAddressId = entityPM.AgentAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentContactId))
            {
				entityPOCO.AgentContactId = entityPM.AgentContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MoveTypeId))
            {
				entityPOCO.MoveTypeId = entityPM.MoveTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
				entityPOCO.TEU = entityPM.TEU;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueOfGoods))
            {
				entityPOCO.ValueOfGoods = entityPM.ValueOfGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueOfGoodsCurrencyId))
            {
				entityPOCO.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChargesByVAT))
            {
				entityPOCO.IsChargesByVAT = entityPM.IsChargesByVAT;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsQuoteDataExternal))
            {
				entityPOCO.IsQuoteDataExternal = entityPM.IsQuoteDataExternal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsQuoteDocumentExternal))
            {
				entityPOCO.IsQuoteDocumentExternal = entityPM.IsQuoteDocumentExternal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalPerContainer))
            {
				entityPOCO.TotalPerContainer = entityPM.TotalPerContainer;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotationSections))
            {
				entityPOCO.QuotationSections = entityPM.QuotationSections;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightInKG))
            {
				entityPOCO.GrossWeightInKG = entityPM.GrossWeightInKG;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightPerTon))
            {
				entityPOCO.GrossWeightPerTon = entityPM.GrossWeightPerTon;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyId))
            {
				entityPOCO.NotifyId = entityPM.NotifyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyAddressId))
            {
				entityPOCO.NotifyAddressId = entityPM.NotifyAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyContactId))
            {
				entityPOCO.NotifyContactId = entityPM.NotifyContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfFollowUps))
            {
				entityPOCO.NumberOfFollowUps = entityPM.NumberOfFollowUps;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightEdited))
            {
				entityPOCO.GrossWeightEdited = entityPM.GrossWeightEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightEdited))
            {
				entityPOCO.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightInKG))
            {
				entityPOCO.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeInCBM))
            {
				entityPOCO.VolumeInCBM = entityPM.VolumeInCBM;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field11))
            {
				entityPOCO.Field11 = entityPM.Field11;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field12))
            {
				entityPOCO.Field12 = entityPM.Field12;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field13))
            {
				entityPOCO.Field13 = entityPM.Field13;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field14))
            {
				entityPOCO.Field14 = entityPM.Field14;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field15))
            {
				entityPOCO.Field15 = entityPM.Field15;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field16))
            {
				entityPOCO.Field16 = entityPM.Field16;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field17))
            {
				entityPOCO.Field17 = entityPM.Field17;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field18))
            {
				entityPOCO.Field18 = entityPM.Field18;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field19))
            {
				entityPOCO.Field19 = entityPM.Field19;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field20))
            {
				entityPOCO.Field20 = entityPM.Field20;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDate))
            {
				entityPOCO.RequestDate = entityPM.RequestDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedProfitInLocal))
            {
				entityPOCO.EstimatedProfitInLocal = entityPM.EstimatedProfitInLocal;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedProfitInProfit))
            {
				entityPOCO.EstimatedProfitInProfit = entityPM.EstimatedProfitInProfit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyId))
            {
				entityPOCO.ProfitCurrencyId = entityPM.ProfitCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitExchangeRate))
            {
				entityPOCO.ProfitExchangeRate = entityPM.ProfitExchangeRate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryForStatisticsId))
            {
				entityPOCO.CountryForStatisticsId = entityPM.CountryForStatisticsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteHTMLDocumentId))
            {
				entityPOCO.QuoteHTMLDocumentId = entityPM.QuoteHTMLDocumentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteClosingReasonId))
            {
				entityPOCO.QuoteClosingReasonId = entityPM.QuoteClosingReasonId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentSubTypeId))
            {
				entityPOCO.ShipmentSubTypeId = entityPM.ShipmentSubTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryRatio))
            {
				entityPOCO.PickupDeliveryRatio = entityPM.PickupDeliveryRatio;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryChargeableWeight))
            {
				entityPOCO.PickupDeliveryChargeableWeight = entityPM.PickupDeliveryChargeableWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryVolumetricWeight))
            {
				entityPOCO.PickupDeliveryVolumetricWeight = entityPM.PickupDeliveryVolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryCWeightUnitCode))
            {
				entityPOCO.PickupDeliveryCWeightUnitCode = entityPM.PickupDeliveryCWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegionalTaxId))
            {
				entityPOCO.RegionalTaxId = entityPM.RegionalTaxId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegionalTaxPercentage))
            {
				entityPOCO.RegionalTaxPercentage = entityPM.RegionalTaxPercentage;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionRightToLeft))
            {
				entityPOCO.DescriptionRightToLeft = entityPM.DescriptionRightToLeft;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
				entityPOCO.IsMultiCurrency = entityPM.IsMultiCurrency;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServiceId))
            {
				entityPOCO.SpecialServiceId = entityPM.SpecialServiceId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
				entityPOCO.FromAddressId = entityPM.FromAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
				entityPOCO.ToAddressId = entityPM.ToAddressId;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteTemplateId))
            {
					entityPM.QuoteTemplateId = entityPOCO.QuoteTemplateId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastVersionNumber))
            {
					entityPM.LastVersionNumber = entityPOCO.LastVersionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FreelancerId))
            {
					entityPM.FreelancerId = entityPOCO.FreelancerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FreelancerAddressId))
            {
					entityPM.FreelancerAddressId = entityPOCO.FreelancerAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FreelancerContactId))
            {
					entityPM.FreelancerContactId = entityPOCO.FreelancerContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastModified))
            {
					entityPM.LastModified = entityPOCO.LastModified;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field1))
            {
					entityPM.Field1 = entityPOCO.Field1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field2))
            {
					entityPM.Field2 = entityPOCO.Field2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field3))
            {
					entityPM.Field3 = entityPOCO.Field3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field4))
            {
					entityPM.Field4 = entityPOCO.Field4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field5))
            {
					entityPM.Field5 = entityPOCO.Field5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field6))
            {
					entityPM.Field6 = entityPOCO.Field6;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field7))
            {
					entityPM.Field7 = entityPOCO.Field7;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field8))
            {
					entityPM.Field8 = entityPOCO.Field8;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field9))
            {
					entityPM.Field9 = entityPOCO.Field9;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field10))
            {
					entityPM.Field10 = entityPOCO.Field10;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsByKG))
            {
					entityPM.IsByKG = entityPOCO.IsByKG;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsByContainer))
            {
					entityPM.IsByContainer = entityPOCO.IsByContainer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimateProfitEdited))
            {
					entityPM.EstimateProfitEdited = entityPOCO.EstimateProfitEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastStageDate))
            {
					entityPM.LastStageDate = entityPOCO.LastStageDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentReference1))
            {
					entityPM.AgentReference1 = entityPOCO.AgentReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentReference2))
            {
					entityPM.AgentReference2 = entityPOCO.AgentReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSaleCurrencySameAsCost))
            {
					entityPM.IsSaleCurrencySameAsCost = entityPOCO.IsSaleCurrencySameAsCost;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimateProfit))
            {
					entityPM.EstimateProfit = entityPOCO.EstimateProfit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsFixedPrice))
            {
					entityPM.IsFixedPrice = entityPOCO.IsFixedPrice;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerContactId))
            {
					entityPM.CustomerContactId = entityPOCO.CustomerContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperName))
            {
					entityPM.ShipperName = entityPOCO.ShipperName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeName))
            {
					entityPM.ConsigneeName = entityPOCO.ConsigneeName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryAddress))
            {
					entityPM.DeliveryAddress = entityPOCO.DeliveryAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickUpAddress))
            {
					entityPM.PickUpAddress = entityPOCO.PickUpAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SaleCurrencyId))
            {
					entityPM.SaleCurrencyId = entityPOCO.SaleCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExchangeRate))
            {
					entityPM.ExchangeRate = entityPOCO.ExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerName))
            {
					entityPM.CustomerName = entityPOCO.CustomerName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteNumber))
            {
					entityPM.QuoteNumber = entityPOCO.QuoteNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageCarrierId))
            {
					entityPM.MainCarriageCarrierId = entityPOCO.MainCarriageCarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionId))
            {
					entityPM.DirectionId = entityPOCO.DirectionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartmentId))
            {
					entityPM.DepartmentId = entityPOCO.DepartmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchId))
            {
					entityPM.BranchId = entityPOCO.BranchId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentTypeId))
            {
					entityPM.ShipmentTypeId = entityPOCO.ShipmentTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteCustomerTypeCode))
            {
					entityPM.QuoteCustomerTypeCode = entityPOCO.QuoteCustomerTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperContactId))
            {
					entityPM.ShipperContactId = entityPOCO.ShipperContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperReference1))
            {
					entityPM.ShipperReference1 = entityPOCO.ShipperReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperReference2))
            {
					entityPM.ShipperReference2 = entityPOCO.ShipperReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeContactId))
            {
					entityPM.ConsigneeContactId = entityPOCO.ConsigneeContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeReference1))
            {
					entityPM.ConsigneeReference1 = entityPOCO.ConsigneeReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeReference2))
            {
					entityPM.ConsigneeReference2 = entityPOCO.ConsigneeReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortId))
            {
					entityPM.FromPortId = entityPOCO.FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortId))
            {
					entityPM.ToPortId = entityPOCO.ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncotermId))
            {
					entityPM.IncotermId = entityPOCO.IncotermId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SalesmanUserId))
            {
					entityPM.SalesmanUserId = entityPOCO.SalesmanUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpenDate))
            {
					entityPM.OpenDate = entityPOCO.OpenDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionOfGoods))
            {
					entityPM.DescriptionOfGoods = entityPOCO.DescriptionOfGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeight))
            {
					entityPM.ChargeableWeight = entityPOCO.ChargeableWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsClosed))
            {
					entityPM.IsClosed = entityPOCO.IsClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DimensionsUnitCode))
            {
					entityPM.DimensionsUnitCode = entityPOCO.DimensionsUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Ratio))
            {
					entityPM.Ratio = entityPOCO.Ratio;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfPackages))
            {
					entityPM.NumberOfPackages = entityPOCO.NumberOfPackages;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfContainers))
            {
					entityPM.NumberOfContainers = entityPOCO.NumberOfContainers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeUnitCode))
            {
					entityPM.VolumeUnitCode = entityPOCO.VolumeUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDangerous))
            {
					entityPM.IsDangerous = entityPOCO.IsDangerous;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpirationDays))
            {
					entityPM.ExpirationDays = entityPOCO.ExpirationDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExpirationDate))
            {
					entityPM.ExpirationDate = entityPOCO.ExpirationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsFreightBySteps))
            {
					entityPM.IsFreightBySteps = entityPOCO.IsFreightBySteps;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType1Id))
            {
					entityPM.PackageType1Id = entityPOCO.PackageType1Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType2Id))
            {
					entityPM.PackageType2Id = entityPOCO.PackageType2Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType3Id))
            {
					entityPM.PackageType3Id = entityPOCO.PackageType3Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType4Id))
            {
					entityPM.PackageType4Id = entityPOCO.PackageType4Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType5Id))
            {
					entityPM.PackageType5Id = entityPOCO.PackageType5Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType1Quantity))
            {
					entityPM.PackageType1Quantity = entityPOCO.PackageType1Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType2Quantity))
            {
					entityPM.PackageType2Quantity = entityPOCO.PackageType2Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType3Quantity))
            {
					entityPM.PackageType3Quantity = entityPOCO.PackageType3Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType4Quantity))
            {
					entityPM.PackageType4Quantity = entityPOCO.PackageType4Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackageType5Quantity))
            {
					entityPM.PackageType5Quantity = entityPOCO.PackageType5Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteTypeCode))
            {
					entityPM.QuoteTypeCode = entityPOCO.QuoteTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightUnitCode))
            {
					entityPM.GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightUnitCode))
            {
					entityPM.ChargeableWeightUnitCode = entityPOCO.ChargeableWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumetricWeight))
            {
					entityPM.VolumetricWeight = entityPOCO.VolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPartnerId))
            {
					entityPM.FromPartnerId = entityPOCO.FromPartnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPartnerId))
            {
					entityPM.ToPartnerId = entityPOCO.ToPartnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPartnerAddressId))
            {
					entityPM.FromPartnerAddressId = entityPOCO.FromPartnerAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPartnerAddressId))
            {
					entityPM.ToPartnerAddressId = entityPOCO.ToPartnerAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressCity))
            {
					entityPM.FromAddressCity = entityPOCO.FromAddressCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressCountryId))
            {
					entityPM.FromAddressCountryId = entityPOCO.FromAddressCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressZipCode))
            {
					entityPM.FromAddressZipCode = entityPOCO.FromAddressZipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressCity))
            {
					entityPM.ToAddressCity = entityPOCO.ToAddressCity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressCountryId))
            {
					entityPM.ToAddressCountryId = entityPOCO.ToAddressCountryId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressZipCode))
            {
					entityPM.ToAddressZipCode = entityPOCO.ToAddressZipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DimFactor))
            {
					entityPM.DimFactor = entityPOCO.DimFactor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncludePickUp))
            {
					entityPM.IncludePickUp = entityPOCO.IncludePickUp;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncludeDelivery))
            {
					entityPM.IncludeDelivery = entityPOCO.IncludeDelivery;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteClosingReasonCode))
            {
					entityPM.QuoteClosingReasonCode = entityPOCO.QuoteClosingReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SentDate))
            {
					entityPM.SentDate = entityPOCO.SentDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AcceptedDate))
            {
					entityPM.AcceptedDate = entityPOCO.AcceptedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclinedDate))
            {
					entityPM.DeclinedDate = entityPOCO.DeclinedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UsageCount))
            {
					entityPM.UsageCount = entityPOCO.UsageCount;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUsageDate))
            {
					entityPM.LastUsageDate = entityPOCO.LastUsageDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessUnitId))
            {
					entityPM.BusinessUnitId = entityPOCO.BusinessUnitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerReference1))
            {
					entityPM.CustomerReference1 = entityPOCO.CustomerReference1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerReference2))
            {
					entityPM.CustomerReference2 = entityPOCO.CustomerReference2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Subject))
            {
					entityPM.Subject = entityPOCO.Subject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSubjectEdited))
            {
					entityPM.IsSubjectEdited = entityPOCO.IsSubjectEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StageId))
            {
					entityPM.StageId = entityPOCO.StageId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StageDueDate))
            {
					entityPM.StageDueDate = entityPOCO.StageDueDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RatingCode))
            {
					entityPM.RatingCode = entityPOCO.RatingCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastActivityDate))
            {
					entityPM.LastActivityDate = entityPOCO.LastActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastActivitySubject))
            {
					entityPM.LastActivitySubject = entityPOCO.LastActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastActivityTypeCode))
            {
					entityPM.LastActivityTypeCode = entityPOCO.LastActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityDate))
            {
					entityPM.NextActivityDate = entityPOCO.NextActivityDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivitySubject))
            {
					entityPM.NextActivitySubject = entityPOCO.NextActivitySubject;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NextActivityTypeCode))
            {
					entityPM.NextActivityTypeCode = entityPOCO.NextActivityTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAutomaticallyClosed))
            {
					entityPM.IsAutomaticallyClosed = entityPOCO.IsAutomaticallyClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutomaticallyCloseDate))
            {
					entityPM.AutomaticallyCloseDate = entityPOCO.AutomaticallyCloseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutomaticallyCloseDays))
            {
					entityPM.AutomaticallyCloseDays = entityPOCO.AutomaticallyCloseDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProductCode))
            {
					entityPM.ProductCode = entityPOCO.ProductCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransitTime))
            {
					entityPM.TransitTime = entityPOCO.TransitTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartureFrequency))
            {
					entityPM.DepartureFrequency = entityPOCO.DepartureFrequency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETA))
            {
					entityPM.ETA = entityPOCO.ETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentId))
            {
					entityPM.AgentId = entityPOCO.AgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentAddressId))
            {
					entityPM.AgentAddressId = entityPOCO.AgentAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentContactId))
            {
					entityPM.AgentContactId = entityPOCO.AgentContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MoveTypeId))
            {
					entityPM.MoveTypeId = entityPOCO.MoveTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TEU))
            {
					entityPM.TEU = entityPOCO.TEU;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueOfGoods))
            {
					entityPM.ValueOfGoods = entityPOCO.ValueOfGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ValueOfGoodsCurrencyId))
            {
					entityPM.ValueOfGoodsCurrencyId = entityPOCO.ValueOfGoodsCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsChargesByVAT))
            {
					entityPM.IsChargesByVAT = entityPOCO.IsChargesByVAT;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsQuoteDataExternal))
            {
					entityPM.IsQuoteDataExternal = entityPOCO.IsQuoteDataExternal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsQuoteDocumentExternal))
            {
					entityPM.IsQuoteDocumentExternal = entityPOCO.IsQuoteDocumentExternal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TotalPerContainer))
            {
					entityPM.TotalPerContainer = entityPOCO.TotalPerContainer;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuotationSections))
            {
					entityPM.QuotationSections = entityPOCO.QuotationSections;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightInKG))
            {
					entityPM.GrossWeightInKG = entityPOCO.GrossWeightInKG;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightPerTon))
            {
					entityPM.GrossWeightPerTon = entityPOCO.GrossWeightPerTon;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotifyId))
            {
					entityPM.NotifyId = entityPOCO.NotifyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotifyAddressId))
            {
					entityPM.NotifyAddressId = entityPOCO.NotifyAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotifyContactId))
            {
					entityPM.NotifyContactId = entityPOCO.NotifyContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfFollowUps))
            {
					entityPM.NumberOfFollowUps = entityPOCO.NumberOfFollowUps;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightEdited))
            {
					entityPM.GrossWeightEdited = entityPOCO.GrossWeightEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightEdited))
            {
					entityPM.ChargeableWeightEdited = entityPOCO.ChargeableWeightEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightInKG))
            {
					entityPM.ChargeableWeightInKG = entityPOCO.ChargeableWeightInKG;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeInCBM))
            {
					entityPM.VolumeInCBM = entityPOCO.VolumeInCBM;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field11))
            {
					entityPM.Field11 = entityPOCO.Field11;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field12))
            {
					entityPM.Field12 = entityPOCO.Field12;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field13))
            {
					entityPM.Field13 = entityPOCO.Field13;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field14))
            {
					entityPM.Field14 = entityPOCO.Field14;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field15))
            {
					entityPM.Field15 = entityPOCO.Field15;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field16))
            {
					entityPM.Field16 = entityPOCO.Field16;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field17))
            {
					entityPM.Field17 = entityPOCO.Field17;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field18))
            {
					entityPM.Field18 = entityPOCO.Field18;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field19))
            {
					entityPM.Field19 = entityPOCO.Field19;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Field20))
            {
					entityPM.Field20 = entityPOCO.Field20;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestDate))
            {
					entityPM.RequestDate = entityPOCO.RequestDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedProfitInLocal))
            {
					entityPM.EstimatedProfitInLocal = entityPOCO.EstimatedProfitInLocal;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EstimatedProfitInProfit))
            {
					entityPM.EstimatedProfitInProfit = entityPOCO.EstimatedProfitInProfit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfitCurrencyId))
            {
					entityPM.ProfitCurrencyId = entityPOCO.ProfitCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfitExchangeRate))
            {
					entityPM.ProfitExchangeRate = entityPOCO.ProfitExchangeRate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryForStatisticsId))
            {
					entityPM.CountryForStatisticsId = entityPOCO.CountryForStatisticsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteHTMLDocumentId))
            {
					entityPM.QuoteHTMLDocumentId = entityPOCO.QuoteHTMLDocumentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteClosingReasonId))
            {
					entityPM.QuoteClosingReasonId = entityPOCO.QuoteClosingReasonId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentSubTypeId))
            {
					entityPM.ShipmentSubTypeId = entityPOCO.ShipmentSubTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDeliveryRatio))
            {
					entityPM.PickupDeliveryRatio = entityPOCO.PickupDeliveryRatio;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDeliveryChargeableWeight))
            {
					entityPM.PickupDeliveryChargeableWeight = entityPOCO.PickupDeliveryChargeableWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDeliveryVolumetricWeight))
            {
					entityPM.PickupDeliveryVolumetricWeight = entityPOCO.PickupDeliveryVolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDeliveryCWeightUnitCode))
            {
					entityPM.PickupDeliveryCWeightUnitCode = entityPOCO.PickupDeliveryCWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegionalTaxId))
            {
					entityPM.RegionalTaxId = entityPOCO.RegionalTaxId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegionalTaxPercentage))
            {
					entityPM.RegionalTaxPercentage = entityPOCO.RegionalTaxPercentage;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionRightToLeft))
            {
					entityPM.DescriptionRightToLeft = entityPOCO.DescriptionRightToLeft;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMultiCurrency))
            {
					entityPM.IsMultiCurrency = entityPOCO.IsMultiCurrency;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialServiceId))
            {
					entityPM.SpecialServiceId = entityPOCO.SpecialServiceId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromAddressId))
            {
					entityPM.FromAddressId = entityPOCO.FromAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToAddressId))
            {
					entityPM.ToAddressId = entityPOCO.ToAddressId;
            }

		}

		public void PMToOldPM(QuoteOPPM entityPM, QuoteOPPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTemplateId))
            {
                oldEntityPM.QuoteTemplateId = entityPM.QuoteTemplateId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastVersionNumber))
            {
                oldEntityPM.LastVersionNumber = entityPM.LastVersionNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerId))
            {
                oldEntityPM.FreelancerId = entityPM.FreelancerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerAddressId))
            {
                oldEntityPM.FreelancerAddressId = entityPM.FreelancerAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FreelancerContactId))
            {
                oldEntityPM.FreelancerContactId = entityPM.FreelancerContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastModified))
            {
                oldEntityPM.LastModified = entityPM.LastModified;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field1))
            {
                oldEntityPM.Field1 = entityPM.Field1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field2))
            {
                oldEntityPM.Field2 = entityPM.Field2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field3))
            {
                oldEntityPM.Field3 = entityPM.Field3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field4))
            {
                oldEntityPM.Field4 = entityPM.Field4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field5))
            {
                oldEntityPM.Field5 = entityPM.Field5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field6))
            {
                oldEntityPM.Field6 = entityPM.Field6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field7))
            {
                oldEntityPM.Field7 = entityPM.Field7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field8))
            {
                oldEntityPM.Field8 = entityPM.Field8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field9))
            {
                oldEntityPM.Field9 = entityPM.Field9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field10))
            {
                oldEntityPM.Field10 = entityPM.Field10;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsByKG))
            {
                oldEntityPM.IsByKG = entityPM.IsByKG;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsByContainer))
            {
                oldEntityPM.IsByContainer = entityPM.IsByContainer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimateProfitEdited))
            {
                oldEntityPM.EstimateProfitEdited = entityPM.EstimateProfitEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
                oldEntityPM.OpportunityId = entityPM.OpportunityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastStageDate))
            {
                oldEntityPM.LastStageDate = entityPM.LastStageDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentReference1))
            {
                oldEntityPM.AgentReference1 = entityPM.AgentReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentReference2))
            {
                oldEntityPM.AgentReference2 = entityPM.AgentReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSaleCurrencySameAsCost))
            {
                oldEntityPM.IsSaleCurrencySameAsCost = entityPM.IsSaleCurrencySameAsCost;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimateProfit))
            {
                oldEntityPM.EstimateProfit = entityPM.EstimateProfit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFixedPrice))
            {
                oldEntityPM.IsFixedPrice = entityPM.IsFixedPrice;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerContactId))
            {
                oldEntityPM.CustomerContactId = entityPM.CustomerContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
                oldEntityPM.ShipperName = entityPM.ShipperName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
                oldEntityPM.ConsigneeName = entityPM.ConsigneeName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryAddress))
            {
                oldEntityPM.DeliveryAddress = entityPM.DeliveryAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickUpAddress))
            {
                oldEntityPM.PickUpAddress = entityPM.PickUpAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SaleCurrencyId))
            {
                oldEntityPM.SaleCurrencyId = entityPM.SaleCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExchangeRate))
            {
                oldEntityPM.ExchangeRate = entityPM.ExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerName))
            {
                oldEntityPM.CustomerName = entityPM.CustomerName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteNumber))
            {
                oldEntityPM.QuoteNumber = entityPM.QuoteNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
                oldEntityPM.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
                oldEntityPM.DirectionId = entityPM.DirectionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartmentId))
            {
                oldEntityPM.DepartmentId = entityPM.DepartmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BranchId))
            {
                oldEntityPM.BranchId = entityPM.BranchId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeId))
            {
                oldEntityPM.ShipmentTypeId = entityPM.ShipmentTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteCustomerTypeCode))
            {
                oldEntityPM.QuoteCustomerTypeCode = entityPM.QuoteCustomerTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperContactId))
            {
                oldEntityPM.ShipperContactId = entityPM.ShipperContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference1))
            {
                oldEntityPM.ShipperReference1 = entityPM.ShipperReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference2))
            {
                oldEntityPM.ShipperReference2 = entityPM.ShipperReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeContactId))
            {
                oldEntityPM.ConsigneeContactId = entityPM.ConsigneeContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference1))
            {
                oldEntityPM.ConsigneeReference1 = entityPM.ConsigneeReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference2))
            {
                oldEntityPM.ConsigneeReference2 = entityPM.ConsigneeReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
                oldEntityPM.FromPortId = entityPM.FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
                oldEntityPM.ToPortId = entityPM.ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
                oldEntityPM.IncotermId = entityPM.IncotermId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SalesmanUserId))
            {
                oldEntityPM.SalesmanUserId = entityPM.SalesmanUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpenDate))
            {
                oldEntityPM.OpenDate = entityPM.OpenDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
                oldEntityPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
                oldEntityPM.ChargeableWeight = entityPM.ChargeableWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsClosed))
            {
                oldEntityPM.IsClosed = entityPM.IsClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
                oldEntityPM.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
                oldEntityPM.Ratio = entityPM.Ratio;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPackages))
            {
                oldEntityPM.NumberOfPackages = entityPM.NumberOfPackages;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfContainers))
            {
                oldEntityPM.NumberOfContainers = entityPM.NumberOfContainers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
                oldEntityPM.VolumeUnitCode = entityPM.VolumeUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
                oldEntityPM.IsDangerous = entityPM.IsDangerous;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDays))
            {
                oldEntityPM.ExpirationDays = entityPM.ExpirationDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ExpirationDate))
            {
                oldEntityPM.ExpirationDate = entityPM.ExpirationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsFreightBySteps))
            {
                oldEntityPM.IsFreightBySteps = entityPM.IsFreightBySteps;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType1Id))
            {
                oldEntityPM.PackageType1Id = entityPM.PackageType1Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType2Id))
            {
                oldEntityPM.PackageType2Id = entityPM.PackageType2Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType3Id))
            {
                oldEntityPM.PackageType3Id = entityPM.PackageType3Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType4Id))
            {
                oldEntityPM.PackageType4Id = entityPM.PackageType4Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType5Id))
            {
                oldEntityPM.PackageType5Id = entityPM.PackageType5Id;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType1Quantity))
            {
                oldEntityPM.PackageType1Quantity = entityPM.PackageType1Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType2Quantity))
            {
                oldEntityPM.PackageType2Quantity = entityPM.PackageType2Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType3Quantity))
            {
                oldEntityPM.PackageType3Quantity = entityPM.PackageType3Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType4Quantity))
            {
                oldEntityPM.PackageType4Quantity = entityPM.PackageType4Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackageType5Quantity))
            {
                oldEntityPM.PackageType5Quantity = entityPM.PackageType5Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteTypeCode))
            {
                oldEntityPM.QuoteTypeCode = entityPM.QuoteTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
                oldEntityPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
                oldEntityPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
                oldEntityPM.VolumetricWeight = entityPM.VolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerId))
            {
                oldEntityPM.FromPartnerId = entityPM.FromPartnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerId))
            {
                oldEntityPM.ToPartnerId = entityPM.ToPartnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPartnerAddressId))
            {
                oldEntityPM.FromPartnerAddressId = entityPM.FromPartnerAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPartnerAddressId))
            {
                oldEntityPM.ToPartnerAddressId = entityPM.ToPartnerAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCity))
            {
                oldEntityPM.FromAddressCity = entityPM.FromAddressCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressCountryId))
            {
                oldEntityPM.FromAddressCountryId = entityPM.FromAddressCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressZipCode))
            {
                oldEntityPM.FromAddressZipCode = entityPM.FromAddressZipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCity))
            {
                oldEntityPM.ToAddressCity = entityPM.ToAddressCity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressCountryId))
            {
                oldEntityPM.ToAddressCountryId = entityPM.ToAddressCountryId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressZipCode))
            {
                oldEntityPM.ToAddressZipCode = entityPM.ToAddressZipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimFactor))
            {
                oldEntityPM.DimFactor = entityPM.DimFactor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncludePickUp))
            {
                oldEntityPM.IncludePickUp = entityPM.IncludePickUp;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncludeDelivery))
            {
                oldEntityPM.IncludeDelivery = entityPM.IncludeDelivery;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteClosingReasonCode))
            {
                oldEntityPM.QuoteClosingReasonCode = entityPM.QuoteClosingReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SentDate))
            {
                oldEntityPM.SentDate = entityPM.SentDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AcceptedDate))
            {
                oldEntityPM.AcceptedDate = entityPM.AcceptedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclinedDate))
            {
                oldEntityPM.DeclinedDate = entityPM.DeclinedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UsageCount))
            {
                oldEntityPM.UsageCount = entityPM.UsageCount;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUsageDate))
            {
                oldEntityPM.LastUsageDate = entityPM.LastUsageDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessUnitId))
            {
                oldEntityPM.BusinessUnitId = entityPM.BusinessUnitId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference1))
            {
                oldEntityPM.CustomerReference1 = entityPM.CustomerReference1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference2))
            {
                oldEntityPM.CustomerReference2 = entityPM.CustomerReference2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Subject))
            {
                oldEntityPM.Subject = entityPM.Subject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSubjectEdited))
            {
                oldEntityPM.IsSubjectEdited = entityPM.IsSubjectEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageId))
            {
                oldEntityPM.StageId = entityPM.StageId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StageDueDate))
            {
                oldEntityPM.StageDueDate = entityPM.StageDueDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RatingCode))
            {
                oldEntityPM.RatingCode = entityPM.RatingCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivityDate))
            {
                oldEntityPM.LastActivityDate = entityPM.LastActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivitySubject))
            {
                oldEntityPM.LastActivitySubject = entityPM.LastActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastActivityTypeCode))
            {
                oldEntityPM.LastActivityTypeCode = entityPM.LastActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityDate))
            {
                oldEntityPM.NextActivityDate = entityPM.NextActivityDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivitySubject))
            {
                oldEntityPM.NextActivitySubject = entityPM.NextActivitySubject;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NextActivityTypeCode))
            {
                oldEntityPM.NextActivityTypeCode = entityPM.NextActivityTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAutomaticallyClosed))
            {
                oldEntityPM.IsAutomaticallyClosed = entityPM.IsAutomaticallyClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDate))
            {
                oldEntityPM.AutomaticallyCloseDate = entityPM.AutomaticallyCloseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutomaticallyCloseDays))
            {
                oldEntityPM.AutomaticallyCloseDays = entityPM.AutomaticallyCloseDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProductCode))
            {
                oldEntityPM.ProductCode = entityPM.ProductCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransitTime))
            {
                oldEntityPM.TransitTime = entityPM.TransitTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureFrequency))
            {
                oldEntityPM.DepartureFrequency = entityPM.DepartureFrequency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
                oldEntityPM.ETA = entityPM.ETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
                oldEntityPM.AgentId = entityPM.AgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentAddressId))
            {
                oldEntityPM.AgentAddressId = entityPM.AgentAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentContactId))
            {
                oldEntityPM.AgentContactId = entityPM.AgentContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MoveTypeId))
            {
                oldEntityPM.MoveTypeId = entityPM.MoveTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TEU))
            {
                oldEntityPM.TEU = entityPM.TEU;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueOfGoods))
            {
                oldEntityPM.ValueOfGoods = entityPM.ValueOfGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ValueOfGoodsCurrencyId))
            {
                oldEntityPM.ValueOfGoodsCurrencyId = entityPM.ValueOfGoodsCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsChargesByVAT))
            {
                oldEntityPM.IsChargesByVAT = entityPM.IsChargesByVAT;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsQuoteDataExternal))
            {
                oldEntityPM.IsQuoteDataExternal = entityPM.IsQuoteDataExternal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsQuoteDocumentExternal))
            {
                oldEntityPM.IsQuoteDocumentExternal = entityPM.IsQuoteDocumentExternal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TotalPerContainer))
            {
                oldEntityPM.TotalPerContainer = entityPM.TotalPerContainer;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuotationSections))
            {
                oldEntityPM.QuotationSections = entityPM.QuotationSections;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightInKG))
            {
                oldEntityPM.GrossWeightInKG = entityPM.GrossWeightInKG;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightPerTon))
            {
                oldEntityPM.GrossWeightPerTon = entityPM.GrossWeightPerTon;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyId))
            {
                oldEntityPM.NotifyId = entityPM.NotifyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyAddressId))
            {
                oldEntityPM.NotifyAddressId = entityPM.NotifyAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotifyContactId))
            {
                oldEntityPM.NotifyContactId = entityPM.NotifyContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfFollowUps))
            {
                oldEntityPM.NumberOfFollowUps = entityPM.NumberOfFollowUps;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightEdited))
            {
                oldEntityPM.GrossWeightEdited = entityPM.GrossWeightEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightEdited))
            {
                oldEntityPM.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightInKG))
            {
                oldEntityPM.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeInCBM))
            {
                oldEntityPM.VolumeInCBM = entityPM.VolumeInCBM;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field11))
            {
                oldEntityPM.Field11 = entityPM.Field11;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field12))
            {
                oldEntityPM.Field12 = entityPM.Field12;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field13))
            {
                oldEntityPM.Field13 = entityPM.Field13;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field14))
            {
                oldEntityPM.Field14 = entityPM.Field14;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field15))
            {
                oldEntityPM.Field15 = entityPM.Field15;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field16))
            {
                oldEntityPM.Field16 = entityPM.Field16;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field17))
            {
                oldEntityPM.Field17 = entityPM.Field17;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field18))
            {
                oldEntityPM.Field18 = entityPM.Field18;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field19))
            {
                oldEntityPM.Field19 = entityPM.Field19;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Field20))
            {
                oldEntityPM.Field20 = entityPM.Field20;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestDate))
            {
                oldEntityPM.RequestDate = entityPM.RequestDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedProfitInLocal))
            {
                oldEntityPM.EstimatedProfitInLocal = entityPM.EstimatedProfitInLocal;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EstimatedProfitInProfit))
            {
                oldEntityPM.EstimatedProfitInProfit = entityPM.EstimatedProfitInProfit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitCurrencyId))
            {
                oldEntityPM.ProfitCurrencyId = entityPM.ProfitCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfitExchangeRate))
            {
                oldEntityPM.ProfitExchangeRate = entityPM.ProfitExchangeRate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryForStatisticsId))
            {
                oldEntityPM.CountryForStatisticsId = entityPM.CountryForStatisticsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteHTMLDocumentId))
            {
                oldEntityPM.QuoteHTMLDocumentId = entityPM.QuoteHTMLDocumentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteClosingReasonId))
            {
                oldEntityPM.QuoteClosingReasonId = entityPM.QuoteClosingReasonId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentSubTypeId))
            {
                oldEntityPM.ShipmentSubTypeId = entityPM.ShipmentSubTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryRatio))
            {
                oldEntityPM.PickupDeliveryRatio = entityPM.PickupDeliveryRatio;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryChargeableWeight))
            {
                oldEntityPM.PickupDeliveryChargeableWeight = entityPM.PickupDeliveryChargeableWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryVolumetricWeight))
            {
                oldEntityPM.PickupDeliveryVolumetricWeight = entityPM.PickupDeliveryVolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDeliveryCWeightUnitCode))
            {
                oldEntityPM.PickupDeliveryCWeightUnitCode = entityPM.PickupDeliveryCWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegionalTaxId))
            {
                oldEntityPM.RegionalTaxId = entityPM.RegionalTaxId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegionalTaxPercentage))
            {
                oldEntityPM.RegionalTaxPercentage = entityPM.RegionalTaxPercentage;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionRightToLeft))
            {
                oldEntityPM.DescriptionRightToLeft = entityPM.DescriptionRightToLeft;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMultiCurrency))
            {
                oldEntityPM.IsMultiCurrency = entityPM.IsMultiCurrency;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServiceId))
            {
                oldEntityPM.SpecialServiceId = entityPM.SpecialServiceId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromAddressId))
            {
                oldEntityPM.FromAddressId = entityPM.FromAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToAddressId))
            {
                oldEntityPM.ToAddressId = entityPM.ToAddressId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field1)) //T4 find type == nText 
            {
                entityPM.Field1 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field1));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field2)) //T4 find type == nText 
            {
                entityPM.Field2 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field2));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field3)) //T4 find type == nText 
            {
                entityPM.Field3 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field3));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field4)) //T4 find type == nText 
            {
                entityPM.Field4 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field4));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field5)) //T4 find type == nText 
            {
                entityPM.Field5 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field5));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field6)) //T4 find type == nText 
            {
                entityPM.Field6 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field6));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field7)) //T4 find type == nText 
            {
                entityPM.Field7 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field7));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field8)) //T4 find type == nText 
            {
                entityPM.Field8 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field8));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field9)) //T4 find type == nText 
            {
                entityPM.Field9 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field9));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field10)) //T4 find type == nText 
            {
                entityPM.Field10 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field10));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ShipperName)) //T4 find type == nText 
            {
                entityPM.ShipperName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ShipperName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ConsigneeName)) //T4 find type == nText 
            {
                entityPM.ConsigneeName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ConsigneeName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DeliveryAddress)) //T4 find type == nText 
            {
                entityPM.DeliveryAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DeliveryAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PickUpAddress)) //T4 find type == nText 
            {
                entityPM.PickUpAddress = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PickUpAddress));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CustomerName)) //T4 find type == nText 
            {
                entityPM.CustomerName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CustomerName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DescriptionOfGoods)) //T4 find type == nText 
            {
                entityPM.DescriptionOfGoods = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DescriptionOfGoods));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FromAddressCity)) //T4 find type == nText 
            {
                entityPM.FromAddressCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FromAddressCity));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ToAddressCity)) //T4 find type == nText 
            {
                entityPM.ToAddressCity = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ToAddressCity));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Subject)) //T4 find type == nText 
            {
                entityPM.Subject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Subject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastActivitySubject)) //T4 find type == nText 
            {
                entityPM.LastActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.NextActivitySubject)) //T4 find type == nText 
            {
                entityPM.NextActivitySubject = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.NextActivitySubject));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TransitTime)) //T4 find type == nText 
            {
                entityPM.TransitTime = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TransitTime));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DepartureFrequency)) //T4 find type == nText 
            {
                entityPM.DepartureFrequency = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DepartureFrequency));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field11)) //T4 find type == nText 
            {
                entityPM.Field11 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field11));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field12)) //T4 find type == nText 
            {
                entityPM.Field12 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field12));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field13)) //T4 find type == nText 
            {
                entityPM.Field13 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field13));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field14)) //T4 find type == nText 
            {
                entityPM.Field14 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field14));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field15)) //T4 find type == nText 
            {
                entityPM.Field15 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field15));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field16)) //T4 find type == nText 
            {
                entityPM.Field16 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field16));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field17)) //T4 find type == nText 
            {
                entityPM.Field17 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field17));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field18)) //T4 find type == nText 
            {
                entityPM.Field18 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field18));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field19)) //T4 find type == nText 
            {
                entityPM.Field19 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field19));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Field20)) //T4 find type == nText 
            {
                entityPM.Field20 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Field20));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FromAddressId)) //T4 find type == nText 
            {
                entityPM.FromAddressId = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FromAddressId));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ToAddressId)) //T4 find type == nText 
            {
                entityPM.ToAddressId = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ToAddressId));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(QuoteOPPM entityPM, QuoteOP entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 