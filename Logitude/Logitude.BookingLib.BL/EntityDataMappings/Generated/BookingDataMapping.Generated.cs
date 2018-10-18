
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
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs; 
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.EntityDataMappings
{
   
   public partial class BookingDataMapping: IMapping<BookingPM, Booking>,IMappingEncodeBase64NVARCHARFields<BookingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingNumber, 
	         DirectionCode, 
	         TransportModeCode, 
	         ShipmentId, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         BookingStatusCode, 
	         Master, 
	         SpaceAllocationCode, 
	         MainCarriageCarrierId, 
	         MainCarriageIsFromStack, 
	         MainCarriageSpaceAllocationCode, 
	         MainCarriageAllotmentIdentification, 
	         MainCarriageFromPortId, 
	         MainCarriageToPortId, 
	         MainCarriageCarrierPrefix, 
	         MainCarriageCarrierNumber, 
	         MainCarriageETD, 
	         Transshipment1FromPortId, 
	         Transshipment1ToPortId, 
	         Transshipment1CarrierId, 
	         Transshipment1ETD, 
	         Transshipment1AllotmentIdentification, 
	         Transshipment1CarrierPrefix, 
	         Transshipment1CarrierNumber, 
	         Transshipment1SpaceAllocationCode, 
	         Transshipment2FromPortId, 
	         Transshipment2ToPortId, 
	         Transshipment2CarrierId, 
	         Transshipment2ETD, 
	         Transshipment2AllotmentIdentification, 
	         Transshipment2CarrierPrefix, 
	         Transshipment2CarrierNumber, 
	         Transshipment2SpaceAllocationCode, 
	         FFRStatusCode, 
	         FFRStatusDate, 
	         FNAReason, 
	         ShipperId, 
	         ShipperAddressId, 
	         ShipperReference, 
	         ConsigneeId, 
	         ConsigneeAddressId, 
	         ConsigneeReference, 
	         IssuingCarrierAgentId, 
	         IssuingCarrierAddressId, 
	         IATACodeId, 
	         CASSCode, 
	         AWBSpecialHandlingCodeId1, 
	         AWBSpecialHandlingCodeId2, 
	         AWBSpecialHandlingCodeId3, 
	         AWBSpecialHandlingCodeId4, 
	         AWBSpecialHandlingCodeId5, 
	         AWBSpecialHandlingCodeId6, 
	         AWBSpecialHandlingCodeId7, 
	         AWBSpecialHandlingCodeId8, 
	         AWBSpecialHandlingCodeId9, 
	         AWBCarrierTarrifReference, 
	         DescriptionOfGoods, 
	         Notes, 
	         SpecialServicesRequest, 
	         OtherServicesInformation, 
	         Routing, 
	         SearchFields, 
	         IssuingCarrierIATACode, 
	         GrossWeightEdited, 
	         ChargeableWeightEdited, 
	         NumberOfPackages, 
	         GrossWeight, 
	         Volume, 
	         VolumetricWeight, 
	         ChargeableWeight, 
	         AWBCommodityItemNumber, 
	         GrossWeightUnitCode, 
	         ChargeableWeightUnitCode, 
	         DimensionsUnitCode, 
	         VolumeUnitCode, 
	         GrossWeightInKG, 
	         ChargeableWeightInKG, 
	         Ratio, 
	         DimFactor, 
	         MainCarriageFinalDestinationPortId, 
	         IsDangerous, 
	         DangerousClassNumber, 
	         DangerousUnNumber, 
	         DangerousPackagingGroup, 
	         DangerousIMDGCode, 
	         DangerousFlashPoint, 
	         DangerousMaterialDescription, 
	         MainHarmonize, 
	         AWBHandlingInformation, 
	         AnswerOtherServicesInformation, 
	         HasResponse, 
	         FMAAcknowledgementReason, 
	         IsCancelled, 
	         HasErrors, 
	         WaitingForResponse, 
	         InterlineId, 
	         BookingLevelCode, 
	         AirlinePrefix, 
	         BookingProductId, 
	         LastSentByUserId, 
	         DescriptionOfGoodsId, 
	         ConcurrencyGUID, 
	         AccountNumber, 
	         IsTemperatureSensitive, 
	         LastFSRStatusRequestDate, 
	         UpdatedByPartner,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BookingNumber, 
	         DirectionCode, 
	         TransportModeCode, 
	         ShipmentId, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         BookingStatusCode, 
	         Master, 
	         SpaceAllocationCode, 
	         MainCarriageCarrierId, 
	         MainCarriageIsFromStack, 
	         MainCarriageSpaceAllocationCode, 
	         MainCarriageAllotmentIdentification, 
	         MainCarriageFromPortId, 
	         MainCarriageToPortId, 
	         MainCarriageCarrierPrefix, 
	         MainCarriageCarrierNumber, 
	         MainCarriageETD, 
	         Transshipment1FromPortId, 
	         Transshipment1ToPortId, 
	         Transshipment1CarrierId, 
	         Transshipment1ETD, 
	         Transshipment1AllotmentIdentification, 
	         Transshipment1CarrierPrefix, 
	         Transshipment1CarrierNumber, 
	         Transshipment1SpaceAllocationCode, 
	         Transshipment2FromPortId, 
	         Transshipment2ToPortId, 
	         Transshipment2CarrierId, 
	         Transshipment2ETD, 
	         Transshipment2AllotmentIdentification, 
	         Transshipment2CarrierPrefix, 
	         Transshipment2CarrierNumber, 
	         Transshipment2SpaceAllocationCode, 
	         FFRStatusCode, 
	         FFRStatusDate, 
	         FNAReason, 
	         ShipperId, 
	         ShipperAddressId, 
	         ShipperReference, 
	         ConsigneeId, 
	         ConsigneeAddressId, 
	         ConsigneeReference, 
	         IssuingCarrierAgentId, 
	         IssuingCarrierAddressId, 
	         IATACodeId, 
	         CASSCode, 
	         AWBSpecialHandlingCodeId1, 
	         AWBSpecialHandlingCodeId2, 
	         AWBSpecialHandlingCodeId3, 
	         AWBSpecialHandlingCodeId4, 
	         AWBSpecialHandlingCodeId5, 
	         AWBSpecialHandlingCodeId6, 
	         AWBSpecialHandlingCodeId7, 
	         AWBSpecialHandlingCodeId8, 
	         AWBSpecialHandlingCodeId9, 
	         AWBCarrierTarrifReference, 
	         DescriptionOfGoods, 
	         Notes, 
	         SpecialServicesRequest, 
	         OtherServicesInformation, 
	         BookingStatusName, 
	         SpaceAllocationName, 
	         FFRStatusName, 
	         Routing, 
	         FirstFlight, 
	         Airline, 
	         SearchFields, 
	         DirectionName, 
	         TransportModeName, 
	         IssuingCarrierIATACode, 
	         ShipperName, 
	         ConsigneeName, 
	         IssuingCarrierAgentName, 
	         GrossWeightEdited, 
	         ChargeableWeightEdited, 
	         NumberOfPackages, 
	         GrossWeight, 
	         Volume, 
	         VolumetricWeight, 
	         ChargeableWeight, 
	         AWBCommodityItemNumber, 
	         GrossWeightUnitCode, 
	         ChargeableWeightUnitCode, 
	         DimensionsUnitCode, 
	         VolumeUnitCode, 
	         GrossWeightInKG, 
	         ChargeableWeightInKG, 
	         Ratio, 
	         DimFactor, 
	         MainCarriageFinalDestinationPortId, 
	         LongMaster, 
	         IsDangerous, 
	         DangerousClassNumber, 
	         DangerousUnNumber, 
	         DangerousPackagingGroup, 
	         DangerousIMDGCode, 
	         DangerousFlashPoint, 
	         DangerousMaterialDescription, 
	         MainHarmonize, 
	         MainCarriageCarrierName, 
	         AWBHandlingInformation, 
	         AnswerOtherServicesInformation, 
	         HasResponse, 
	         FMAAcknowledgementReason, 
	         IsCancelled, 
	         HasErrors, 
	         WaitingForResponse, 
	         InterlineId, 
	         BookingLevelCode, 
	         BookingLevelName, 
	         AirlinePrefix, 
	         CreatedByUserName, 
	         BookingProductId, 
	         BookingProductName, 
	         LastSentByUserId, 
	         ShipmentNumber, 
	         DescriptionOfGoodsId, 
	         MainCarriageCarrierCode, 
	         DescriptionOfGoodsService, 
	         ConcurrencyGUID, 
	         AccountNumber, 
	         AWBSpecialHandlingCodeId, 
	         IsTemperatureSensitive, 
	         ShipperAddress1, 
	         ShipperAddress2, 
	         ShipperCity, 
	         ShipperCountryId, 
	         ShipperStateId, 
	         ShipperZipCode, 
	         ConsigneeAddress1, 
	         ConsigneeAddress2, 
	         ConsigneeCity, 
	         ConsigneeCountryId, 
	         ConsigneeStateId, 
	         ConsigneeZipCode, 
	         MainFromPortCountryCode, 
	         MainFromPortCountryName, 
	         MainFromPortCode, 
	         MainFromPortName, 
	         MainToPortCountryCode, 
	         MainToPortCountryName, 
	         MainToPortCode, 
	         MainToPortName, 
	         LastFSRStatusRequestDate, 
	         ZeroIsDescOfGoodsFromList, 
	         TenantZeroAirlineId, 
	         TenantZeroIsProductMandatory, 
	         TenantZeroIsManagingProduct, 
	         TenantZeroAirlineGLSHKFFR, 
	         TenantZeroAirlinePIMA, 
	         ZeroGLSHKNeedsRegistration, 
	         CarrierIsGLSHKRegistered, 
	         TenantZeroAirlineChampFFR, 
	         TenantZeroAirlineTTY, 
	         ZeroChampNeedsRegistration, 
	         CarrierIsChampRegistered, 
	         MAWBTakenFromStack, 
	         Trans1FromPortCode, 
	         Trans1FromPortName, 
	         Trans1FromPortCountryCode, 
	         Trans1FromPortCountryName, 
	         Trans1ToPortCode, 
	         Trans1ToPortName, 
	         Trans1ToPortCountryCode, 
	         Trans1ToPortCountryName, 
	         Trans2FromPortCode, 
	         Trans2FromPortName, 
	         Trans2FromPortCountryCode, 
	         Trans2FromPortCountryName, 
	         Trans2ToPortCode, 
	         Trans2ToPortName, 
	         Trans2ToPortCountryCode, 
	         Trans2ToPortCountryName, 
	         CarrierIsCheckDigit, 
	         CarrierIsLimitedLength, 
	         Transshipment1CarrierName, 
	         Transshipment2CarrierName, 
	         MAWBStackAirlineId, 
	         MAWBStackNumber, 
	         MAWBReturnedToStack, 
	         MAWBReturnedToStackWithCancel, 
	         IsCopyMode, 
	         FinalDestinationPortCode, 
	         TenantZeroAirlineChampFVR, 
	         TenantZeroAirlineGLSHKFVR, 
	         FinalDestinationPortName, 
	         TenantZeroAirlineGLSHKFSRFSA, 
	         TenantZeroAirlineChampFSRFSA, 
	         UpdatedByPartner,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(BookingPM entityPM, Booking entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNumber))
            {
				entityPOCO.BookingNumber = entityPM.BookingNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionCode))
            {
				entityPOCO.DirectionCode = entityPM.DirectionCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeCode))
            {
				entityPOCO.TransportModeCode = entityPM.TransportModeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingStatusCode))
            {
				entityPOCO.BookingStatusCode = entityPM.BookingStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
				entityPOCO.Master = entityPM.Master;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpaceAllocationCode))
            {
				entityPOCO.SpaceAllocationCode = entityPM.SpaceAllocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
				entityPOCO.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageIsFromStack))
            {
				entityPOCO.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageSpaceAllocationCode))
            {
				entityPOCO.MainCarriageSpaceAllocationCode = entityPM.MainCarriageSpaceAllocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageAllotmentIdentification))
            {
				entityPOCO.MainCarriageAllotmentIdentification = entityPM.MainCarriageAllotmentIdentification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageFromPortId))
            {
				entityPOCO.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageToPortId))
            {
				entityPOCO.MainCarriageToPortId = entityPM.MainCarriageToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierPrefix))
            {
				entityPOCO.MainCarriageCarrierPrefix = entityPM.MainCarriageCarrierPrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierNumber))
            {
				entityPOCO.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETD))
            {
				entityPOCO.MainCarriageETD = entityPM.MainCarriageETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1FromPortId))
            {
				entityPOCO.Transshipment1FromPortId = entityPM.Transshipment1FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1ToPortId))
            {
				entityPOCO.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierId))
            {
				entityPOCO.Transshipment1CarrierId = entityPM.Transshipment1CarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1ETD))
            {
				entityPOCO.Transshipment1ETD = entityPM.Transshipment1ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1AllotmentIdentification))
            {
				entityPOCO.Transshipment1AllotmentIdentification = entityPM.Transshipment1AllotmentIdentification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierPrefix))
            {
				entityPOCO.Transshipment1CarrierPrefix = entityPM.Transshipment1CarrierPrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierNumber))
            {
				entityPOCO.Transshipment1CarrierNumber = entityPM.Transshipment1CarrierNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1SpaceAllocationCode))
            {
				entityPOCO.Transshipment1SpaceAllocationCode = entityPM.Transshipment1SpaceAllocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2FromPortId))
            {
				entityPOCO.Transshipment2FromPortId = entityPM.Transshipment2FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2ToPortId))
            {
				entityPOCO.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierId))
            {
				entityPOCO.Transshipment2CarrierId = entityPM.Transshipment2CarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2ETD))
            {
				entityPOCO.Transshipment2ETD = entityPM.Transshipment2ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2AllotmentIdentification))
            {
				entityPOCO.Transshipment2AllotmentIdentification = entityPM.Transshipment2AllotmentIdentification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierPrefix))
            {
				entityPOCO.Transshipment2CarrierPrefix = entityPM.Transshipment2CarrierPrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierNumber))
            {
				entityPOCO.Transshipment2CarrierNumber = entityPM.Transshipment2CarrierNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2SpaceAllocationCode))
            {
				entityPOCO.Transshipment2SpaceAllocationCode = entityPM.Transshipment2SpaceAllocationCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FFRStatusCode))
            {
				entityPOCO.FFRStatusCode = entityPM.FFRStatusCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FFRStatusDate))
            {
				entityPOCO.FFRStatusDate = entityPM.FFRStatusDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FNAReason))
            {
				entityPOCO.FNAReason = entityPM.FNAReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperAddressId))
            {
				entityPOCO.ShipperAddressId = entityPM.ShipperAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference))
            {
				entityPOCO.ShipperReference = entityPM.ShipperReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeAddressId))
            {
				entityPOCO.ConsigneeAddressId = entityPM.ConsigneeAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference))
            {
				entityPOCO.ConsigneeReference = entityPM.ConsigneeReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierAgentId))
            {
				entityPOCO.IssuingCarrierAgentId = entityPM.IssuingCarrierAgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierAddressId))
            {
				entityPOCO.IssuingCarrierAddressId = entityPM.IssuingCarrierAddressId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IATACodeId))
            {
				entityPOCO.IATACodeId = entityPM.IATACodeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CASSCode))
            {
				entityPOCO.CASSCode = entityPM.CASSCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId1))
            {
				entityPOCO.AWBSpecialHandlingCodeId1 = entityPM.AWBSpecialHandlingCodeId1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId2))
            {
				entityPOCO.AWBSpecialHandlingCodeId2 = entityPM.AWBSpecialHandlingCodeId2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId3))
            {
				entityPOCO.AWBSpecialHandlingCodeId3 = entityPM.AWBSpecialHandlingCodeId3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId4))
            {
				entityPOCO.AWBSpecialHandlingCodeId4 = entityPM.AWBSpecialHandlingCodeId4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId5))
            {
				entityPOCO.AWBSpecialHandlingCodeId5 = entityPM.AWBSpecialHandlingCodeId5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId6))
            {
				entityPOCO.AWBSpecialHandlingCodeId6 = entityPM.AWBSpecialHandlingCodeId6;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId7))
            {
				entityPOCO.AWBSpecialHandlingCodeId7 = entityPM.AWBSpecialHandlingCodeId7;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId8))
            {
				entityPOCO.AWBSpecialHandlingCodeId8 = entityPM.AWBSpecialHandlingCodeId8;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId9))
            {
				entityPOCO.AWBSpecialHandlingCodeId9 = entityPM.AWBSpecialHandlingCodeId9;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBCarrierTarrifReference))
            {
				entityPOCO.AWBCarrierTarrifReference = entityPM.AWBCarrierTarrifReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
				entityPOCO.DescriptionOfGoods = entityPM.DescriptionOfGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesRequest))
            {
				entityPOCO.SpecialServicesRequest = entityPM.SpecialServicesRequest;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherServicesInformation))
            {
				entityPOCO.OtherServicesInformation = entityPM.OtherServicesInformation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Routing))
            {
				entityPOCO.Routing = entityPM.Routing;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierIATACode))
            {
				entityPOCO.IssuingCarrierIATACode = entityPM.IssuingCarrierIATACode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightEdited))
            {
				entityPOCO.GrossWeightEdited = entityPM.GrossWeightEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightEdited))
            {
				entityPOCO.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPackages))
            {
				entityPOCO.NumberOfPackages = entityPM.NumberOfPackages;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
				entityPOCO.VolumetricWeight = entityPM.VolumetricWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
				entityPOCO.ChargeableWeight = entityPM.ChargeableWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBCommodityItemNumber))
            {
				entityPOCO.AWBCommodityItemNumber = entityPM.AWBCommodityItemNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
				entityPOCO.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
				entityPOCO.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
				entityPOCO.DimensionsUnitCode = entityPM.DimensionsUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
				entityPOCO.VolumeUnitCode = entityPM.VolumeUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightInKG))
            {
				entityPOCO.GrossWeightInKG = entityPM.GrossWeightInKG;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightInKG))
            {
				entityPOCO.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
				entityPOCO.Ratio = entityPM.Ratio;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimFactor))
            {
				entityPOCO.DimFactor = entityPM.DimFactor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageFinalDestinationPortId))
            {
				entityPOCO.MainCarriageFinalDestinationPortId = entityPM.MainCarriageFinalDestinationPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
				entityPOCO.IsDangerous = entityPM.IsDangerous;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousClassNumber))
            {
				entityPOCO.DangerousClassNumber = entityPM.DangerousClassNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousUnNumber))
            {
				entityPOCO.DangerousUnNumber = entityPM.DangerousUnNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousPackagingGroup))
            {
				entityPOCO.DangerousPackagingGroup = entityPM.DangerousPackagingGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousIMDGCode))
            {
				entityPOCO.DangerousIMDGCode = entityPM.DangerousIMDGCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousFlashPoint))
            {
				entityPOCO.DangerousFlashPoint = entityPM.DangerousFlashPoint;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousMaterialDescription))
            {
				entityPOCO.DangerousMaterialDescription = entityPM.DangerousMaterialDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainHarmonize))
            {
				entityPOCO.MainHarmonize = entityPM.MainHarmonize;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBHandlingInformation))
            {
				entityPOCO.AWBHandlingInformation = entityPM.AWBHandlingInformation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerOtherServicesInformation))
            {
				entityPOCO.AnswerOtherServicesInformation = entityPM.AnswerOtherServicesInformation;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasResponse))
            {
				entityPOCO.HasResponse = entityPM.HasResponse;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FMAAcknowledgementReason))
            {
				entityPOCO.FMAAcknowledgementReason = entityPM.FMAAcknowledgementReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasErrors))
            {
				entityPOCO.HasErrors = entityPM.HasErrors;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WaitingForResponse))
            {
				entityPOCO.WaitingForResponse = entityPM.WaitingForResponse;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterlineId))
            {
				entityPOCO.InterlineId = entityPM.InterlineId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingLevelCode))
            {
				entityPOCO.BookingLevelCode = entityPM.BookingLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlinePrefix))
            {
				entityPOCO.AirlinePrefix = entityPM.AirlinePrefix;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingProductId))
            {
				entityPOCO.BookingProductId = entityPM.BookingProductId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastSentByUserId))
            {
				entityPOCO.LastSentByUserId = entityPM.LastSentByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoodsId))
            {
				entityPOCO.DescriptionOfGoodsId = entityPM.DescriptionOfGoodsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
				entityPOCO.ConcurrencyGUID = entityPM.ConcurrencyGUID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
				entityPOCO.AccountNumber = entityPM.AccountNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsTemperatureSensitive))
            {
				entityPOCO.IsTemperatureSensitive = entityPM.IsTemperatureSensitive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastFSRStatusRequestDate))
            {
				entityPOCO.LastFSRStatusRequestDate = entityPM.LastFSRStatusRequestDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByPartner))
            {
				entityPOCO.UpdatedByPartner = entityPM.UpdatedByPartner;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(BookingPM entityPM, Booking entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingNumber))
            {
					entityPM.BookingNumber = entityPOCO.BookingNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionCode))
            {
					entityPM.DirectionCode = entityPOCO.DirectionCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeCode))
            {
					entityPM.TransportModeCode = entityPOCO.TransportModeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingStatusCode))
            {
					entityPM.BookingStatusCode = entityPOCO.BookingStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Master))
            {
					entityPM.Master = entityPOCO.Master;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpaceAllocationCode))
            {
					entityPM.SpaceAllocationCode = entityPOCO.SpaceAllocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageCarrierId))
            {
					entityPM.MainCarriageCarrierId = entityPOCO.MainCarriageCarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageIsFromStack))
            {
					entityPM.MainCarriageIsFromStack = entityPOCO.MainCarriageIsFromStack;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageSpaceAllocationCode))
            {
					entityPM.MainCarriageSpaceAllocationCode = entityPOCO.MainCarriageSpaceAllocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageAllotmentIdentification))
            {
					entityPM.MainCarriageAllotmentIdentification = entityPOCO.MainCarriageAllotmentIdentification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageFromPortId))
            {
					entityPM.MainCarriageFromPortId = entityPOCO.MainCarriageFromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageToPortId))
            {
					entityPM.MainCarriageToPortId = entityPOCO.MainCarriageToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageCarrierPrefix))
            {
					entityPM.MainCarriageCarrierPrefix = entityPOCO.MainCarriageCarrierPrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageCarrierNumber))
            {
					entityPM.MainCarriageCarrierNumber = entityPOCO.MainCarriageCarrierNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageETD))
            {
					entityPM.MainCarriageETD = entityPOCO.MainCarriageETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1FromPortId))
            {
					entityPM.Transshipment1FromPortId = entityPOCO.Transshipment1FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1ToPortId))
            {
					entityPM.Transshipment1ToPortId = entityPOCO.Transshipment1ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1CarrierId))
            {
					entityPM.Transshipment1CarrierId = entityPOCO.Transshipment1CarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1ETD))
            {
					entityPM.Transshipment1ETD = entityPOCO.Transshipment1ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1AllotmentIdentification))
            {
					entityPM.Transshipment1AllotmentIdentification = entityPOCO.Transshipment1AllotmentIdentification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1CarrierPrefix))
            {
					entityPM.Transshipment1CarrierPrefix = entityPOCO.Transshipment1CarrierPrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1CarrierNumber))
            {
					entityPM.Transshipment1CarrierNumber = entityPOCO.Transshipment1CarrierNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment1SpaceAllocationCode))
            {
					entityPM.Transshipment1SpaceAllocationCode = entityPOCO.Transshipment1SpaceAllocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2FromPortId))
            {
					entityPM.Transshipment2FromPortId = entityPOCO.Transshipment2FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2ToPortId))
            {
					entityPM.Transshipment2ToPortId = entityPOCO.Transshipment2ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2CarrierId))
            {
					entityPM.Transshipment2CarrierId = entityPOCO.Transshipment2CarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2ETD))
            {
					entityPM.Transshipment2ETD = entityPOCO.Transshipment2ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2AllotmentIdentification))
            {
					entityPM.Transshipment2AllotmentIdentification = entityPOCO.Transshipment2AllotmentIdentification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2CarrierPrefix))
            {
					entityPM.Transshipment2CarrierPrefix = entityPOCO.Transshipment2CarrierPrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2CarrierNumber))
            {
					entityPM.Transshipment2CarrierNumber = entityPOCO.Transshipment2CarrierNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Transshipment2SpaceAllocationCode))
            {
					entityPM.Transshipment2SpaceAllocationCode = entityPOCO.Transshipment2SpaceAllocationCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FFRStatusCode))
            {
					entityPM.FFRStatusCode = entityPOCO.FFRStatusCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FFRStatusDate))
            {
					entityPM.FFRStatusDate = entityPOCO.FFRStatusDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FNAReason))
            {
					entityPM.FNAReason = entityPOCO.FNAReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperAddressId))
            {
					entityPM.ShipperAddressId = entityPOCO.ShipperAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperReference))
            {
					entityPM.ShipperReference = entityPOCO.ShipperReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeAddressId))
            {
					entityPM.ConsigneeAddressId = entityPOCO.ConsigneeAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeReference))
            {
					entityPM.ConsigneeReference = entityPOCO.ConsigneeReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssuingCarrierAgentId))
            {
					entityPM.IssuingCarrierAgentId = entityPOCO.IssuingCarrierAgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssuingCarrierAddressId))
            {
					entityPM.IssuingCarrierAddressId = entityPOCO.IssuingCarrierAddressId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IATACodeId))
            {
					entityPM.IATACodeId = entityPOCO.IATACodeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CASSCode))
            {
					entityPM.CASSCode = entityPOCO.CASSCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId1))
            {
					entityPM.AWBSpecialHandlingCodeId1 = entityPOCO.AWBSpecialHandlingCodeId1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId2))
            {
					entityPM.AWBSpecialHandlingCodeId2 = entityPOCO.AWBSpecialHandlingCodeId2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId3))
            {
					entityPM.AWBSpecialHandlingCodeId3 = entityPOCO.AWBSpecialHandlingCodeId3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId4))
            {
					entityPM.AWBSpecialHandlingCodeId4 = entityPOCO.AWBSpecialHandlingCodeId4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId5))
            {
					entityPM.AWBSpecialHandlingCodeId5 = entityPOCO.AWBSpecialHandlingCodeId5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId6))
            {
					entityPM.AWBSpecialHandlingCodeId6 = entityPOCO.AWBSpecialHandlingCodeId6;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId7))
            {
					entityPM.AWBSpecialHandlingCodeId7 = entityPOCO.AWBSpecialHandlingCodeId7;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId8))
            {
					entityPM.AWBSpecialHandlingCodeId8 = entityPOCO.AWBSpecialHandlingCodeId8;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBSpecialHandlingCodeId9))
            {
					entityPM.AWBSpecialHandlingCodeId9 = entityPOCO.AWBSpecialHandlingCodeId9;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBCarrierTarrifReference))
            {
					entityPM.AWBCarrierTarrifReference = entityPOCO.AWBCarrierTarrifReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionOfGoods))
            {
					entityPM.DescriptionOfGoods = entityPOCO.DescriptionOfGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialServicesRequest))
            {
					entityPM.SpecialServicesRequest = entityPOCO.SpecialServicesRequest;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OtherServicesInformation))
            {
					entityPM.OtherServicesInformation = entityPOCO.OtherServicesInformation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Routing))
            {
					entityPM.Routing = entityPOCO.Routing;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IssuingCarrierIATACode))
            {
					entityPM.IssuingCarrierIATACode = entityPOCO.IssuingCarrierIATACode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightEdited))
            {
					entityPM.GrossWeightEdited = entityPOCO.GrossWeightEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightEdited))
            {
					entityPM.ChargeableWeightEdited = entityPOCO.ChargeableWeightEdited;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NumberOfPackages))
            {
					entityPM.NumberOfPackages = entityPOCO.NumberOfPackages;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumetricWeight))
            {
					entityPM.VolumetricWeight = entityPOCO.VolumetricWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeight))
            {
					entityPM.ChargeableWeight = entityPOCO.ChargeableWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBCommodityItemNumber))
            {
					entityPM.AWBCommodityItemNumber = entityPOCO.AWBCommodityItemNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightUnitCode))
            {
					entityPM.GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightUnitCode))
            {
					entityPM.ChargeableWeightUnitCode = entityPOCO.ChargeableWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DimensionsUnitCode))
            {
					entityPM.DimensionsUnitCode = entityPOCO.DimensionsUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VolumeUnitCode))
            {
					entityPM.VolumeUnitCode = entityPOCO.VolumeUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightInKG))
            {
					entityPM.GrossWeightInKG = entityPOCO.GrossWeightInKG;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargeableWeightInKG))
            {
					entityPM.ChargeableWeightInKG = entityPOCO.ChargeableWeightInKG;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Ratio))
            {
					entityPM.Ratio = entityPOCO.Ratio;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DimFactor))
            {
					entityPM.DimFactor = entityPOCO.DimFactor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainCarriageFinalDestinationPortId))
            {
					entityPM.MainCarriageFinalDestinationPortId = entityPOCO.MainCarriageFinalDestinationPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDangerous))
            {
					entityPM.IsDangerous = entityPOCO.IsDangerous;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousClassNumber))
            {
					entityPM.DangerousClassNumber = entityPOCO.DangerousClassNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousUnNumber))
            {
					entityPM.DangerousUnNumber = entityPOCO.DangerousUnNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousPackagingGroup))
            {
					entityPM.DangerousPackagingGroup = entityPOCO.DangerousPackagingGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousIMDGCode))
            {
					entityPM.DangerousIMDGCode = entityPOCO.DangerousIMDGCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousFlashPoint))
            {
					entityPM.DangerousFlashPoint = entityPOCO.DangerousFlashPoint;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousMaterialDescription))
            {
					entityPM.DangerousMaterialDescription = entityPOCO.DangerousMaterialDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MainHarmonize))
            {
					entityPM.MainHarmonize = entityPOCO.MainHarmonize;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AWBHandlingInformation))
            {
					entityPM.AWBHandlingInformation = entityPOCO.AWBHandlingInformation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AnswerOtherServicesInformation))
            {
					entityPM.AnswerOtherServicesInformation = entityPOCO.AnswerOtherServicesInformation;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HasResponse))
            {
					entityPM.HasResponse = entityPOCO.HasResponse;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FMAAcknowledgementReason))
            {
					entityPM.FMAAcknowledgementReason = entityPOCO.FMAAcknowledgementReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.HasErrors))
            {
					entityPM.HasErrors = entityPOCO.HasErrors;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WaitingForResponse))
            {
					entityPM.WaitingForResponse = entityPOCO.WaitingForResponse;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterlineId))
            {
					entityPM.InterlineId = entityPOCO.InterlineId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingLevelCode))
            {
					entityPM.BookingLevelCode = entityPOCO.BookingLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirlinePrefix))
            {
					entityPM.AirlinePrefix = entityPOCO.AirlinePrefix;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingProductId))
            {
					entityPM.BookingProductId = entityPOCO.BookingProductId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastSentByUserId))
            {
					entityPM.LastSentByUserId = entityPOCO.LastSentByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionOfGoodsId))
            {
					entityPM.DescriptionOfGoodsId = entityPOCO.DescriptionOfGoodsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConcurrencyGUID))
            {
					entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountNumber))
            {
					entityPM.AccountNumber = entityPOCO.AccountNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsTemperatureSensitive))
            {
					entityPM.IsTemperatureSensitive = entityPOCO.IsTemperatureSensitive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastFSRStatusRequestDate))
            {
					entityPM.LastFSRStatusRequestDate = entityPOCO.LastFSRStatusRequestDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByPartner))
            {
					entityPM.UpdatedByPartner = entityPOCO.UpdatedByPartner;
            }

		}

		public void PMToOldPM(BookingPM entityPM, BookingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNumber))
            {
                oldEntityPM.BookingNumber = entityPM.BookingNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionCode))
            {
                oldEntityPM.DirectionCode = entityPM.DirectionCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeCode))
            {
                oldEntityPM.TransportModeCode = entityPM.TransportModeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingStatusCode))
            {
                oldEntityPM.BookingStatusCode = entityPM.BookingStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
                oldEntityPM.Master = entityPM.Master;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpaceAllocationCode))
            {
                oldEntityPM.SpaceAllocationCode = entityPM.SpaceAllocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierId))
            {
                oldEntityPM.MainCarriageCarrierId = entityPM.MainCarriageCarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageIsFromStack))
            {
                oldEntityPM.MainCarriageIsFromStack = entityPM.MainCarriageIsFromStack;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageSpaceAllocationCode))
            {
                oldEntityPM.MainCarriageSpaceAllocationCode = entityPM.MainCarriageSpaceAllocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageAllotmentIdentification))
            {
                oldEntityPM.MainCarriageAllotmentIdentification = entityPM.MainCarriageAllotmentIdentification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageFromPortId))
            {
                oldEntityPM.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageToPortId))
            {
                oldEntityPM.MainCarriageToPortId = entityPM.MainCarriageToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierPrefix))
            {
                oldEntityPM.MainCarriageCarrierPrefix = entityPM.MainCarriageCarrierPrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageCarrierNumber))
            {
                oldEntityPM.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageETD))
            {
                oldEntityPM.MainCarriageETD = entityPM.MainCarriageETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1FromPortId))
            {
                oldEntityPM.Transshipment1FromPortId = entityPM.Transshipment1FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1ToPortId))
            {
                oldEntityPM.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierId))
            {
                oldEntityPM.Transshipment1CarrierId = entityPM.Transshipment1CarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1ETD))
            {
                oldEntityPM.Transshipment1ETD = entityPM.Transshipment1ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1AllotmentIdentification))
            {
                oldEntityPM.Transshipment1AllotmentIdentification = entityPM.Transshipment1AllotmentIdentification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierPrefix))
            {
                oldEntityPM.Transshipment1CarrierPrefix = entityPM.Transshipment1CarrierPrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1CarrierNumber))
            {
                oldEntityPM.Transshipment1CarrierNumber = entityPM.Transshipment1CarrierNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment1SpaceAllocationCode))
            {
                oldEntityPM.Transshipment1SpaceAllocationCode = entityPM.Transshipment1SpaceAllocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2FromPortId))
            {
                oldEntityPM.Transshipment2FromPortId = entityPM.Transshipment2FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2ToPortId))
            {
                oldEntityPM.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierId))
            {
                oldEntityPM.Transshipment2CarrierId = entityPM.Transshipment2CarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2ETD))
            {
                oldEntityPM.Transshipment2ETD = entityPM.Transshipment2ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2AllotmentIdentification))
            {
                oldEntityPM.Transshipment2AllotmentIdentification = entityPM.Transshipment2AllotmentIdentification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierPrefix))
            {
                oldEntityPM.Transshipment2CarrierPrefix = entityPM.Transshipment2CarrierPrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2CarrierNumber))
            {
                oldEntityPM.Transshipment2CarrierNumber = entityPM.Transshipment2CarrierNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Transshipment2SpaceAllocationCode))
            {
                oldEntityPM.Transshipment2SpaceAllocationCode = entityPM.Transshipment2SpaceAllocationCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FFRStatusCode))
            {
                oldEntityPM.FFRStatusCode = entityPM.FFRStatusCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FFRStatusDate))
            {
                oldEntityPM.FFRStatusDate = entityPM.FFRStatusDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FNAReason))
            {
                oldEntityPM.FNAReason = entityPM.FNAReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperAddressId))
            {
                oldEntityPM.ShipperAddressId = entityPM.ShipperAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperReference))
            {
                oldEntityPM.ShipperReference = entityPM.ShipperReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeAddressId))
            {
                oldEntityPM.ConsigneeAddressId = entityPM.ConsigneeAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeReference))
            {
                oldEntityPM.ConsigneeReference = entityPM.ConsigneeReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierAgentId))
            {
                oldEntityPM.IssuingCarrierAgentId = entityPM.IssuingCarrierAgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierAddressId))
            {
                oldEntityPM.IssuingCarrierAddressId = entityPM.IssuingCarrierAddressId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IATACodeId))
            {
                oldEntityPM.IATACodeId = entityPM.IATACodeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CASSCode))
            {
                oldEntityPM.CASSCode = entityPM.CASSCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId1))
            {
                oldEntityPM.AWBSpecialHandlingCodeId1 = entityPM.AWBSpecialHandlingCodeId1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId2))
            {
                oldEntityPM.AWBSpecialHandlingCodeId2 = entityPM.AWBSpecialHandlingCodeId2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId3))
            {
                oldEntityPM.AWBSpecialHandlingCodeId3 = entityPM.AWBSpecialHandlingCodeId3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId4))
            {
                oldEntityPM.AWBSpecialHandlingCodeId4 = entityPM.AWBSpecialHandlingCodeId4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId5))
            {
                oldEntityPM.AWBSpecialHandlingCodeId5 = entityPM.AWBSpecialHandlingCodeId5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId6))
            {
                oldEntityPM.AWBSpecialHandlingCodeId6 = entityPM.AWBSpecialHandlingCodeId6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId7))
            {
                oldEntityPM.AWBSpecialHandlingCodeId7 = entityPM.AWBSpecialHandlingCodeId7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId8))
            {
                oldEntityPM.AWBSpecialHandlingCodeId8 = entityPM.AWBSpecialHandlingCodeId8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBSpecialHandlingCodeId9))
            {
                oldEntityPM.AWBSpecialHandlingCodeId9 = entityPM.AWBSpecialHandlingCodeId9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBCarrierTarrifReference))
            {
                oldEntityPM.AWBCarrierTarrifReference = entityPM.AWBCarrierTarrifReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
                oldEntityPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesRequest))
            {
                oldEntityPM.SpecialServicesRequest = entityPM.SpecialServicesRequest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OtherServicesInformation))
            {
                oldEntityPM.OtherServicesInformation = entityPM.OtherServicesInformation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Routing))
            {
                oldEntityPM.Routing = entityPM.Routing;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IssuingCarrierIATACode))
            {
                oldEntityPM.IssuingCarrierIATACode = entityPM.IssuingCarrierIATACode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightEdited))
            {
                oldEntityPM.GrossWeightEdited = entityPM.GrossWeightEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightEdited))
            {
                oldEntityPM.ChargeableWeightEdited = entityPM.ChargeableWeightEdited;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NumberOfPackages))
            {
                oldEntityPM.NumberOfPackages = entityPM.NumberOfPackages;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumetricWeight))
            {
                oldEntityPM.VolumetricWeight = entityPM.VolumetricWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeight))
            {
                oldEntityPM.ChargeableWeight = entityPM.ChargeableWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBCommodityItemNumber))
            {
                oldEntityPM.AWBCommodityItemNumber = entityPM.AWBCommodityItemNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
                oldEntityPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightUnitCode))
            {
                oldEntityPM.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimensionsUnitCode))
            {
                oldEntityPM.DimensionsUnitCode = entityPM.DimensionsUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VolumeUnitCode))
            {
                oldEntityPM.VolumeUnitCode = entityPM.VolumeUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightInKG))
            {
                oldEntityPM.GrossWeightInKG = entityPM.GrossWeightInKG;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargeableWeightInKG))
            {
                oldEntityPM.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Ratio))
            {
                oldEntityPM.Ratio = entityPM.Ratio;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DimFactor))
            {
                oldEntityPM.DimFactor = entityPM.DimFactor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainCarriageFinalDestinationPortId))
            {
                oldEntityPM.MainCarriageFinalDestinationPortId = entityPM.MainCarriageFinalDestinationPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDangerous))
            {
                oldEntityPM.IsDangerous = entityPM.IsDangerous;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousClassNumber))
            {
                oldEntityPM.DangerousClassNumber = entityPM.DangerousClassNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousUnNumber))
            {
                oldEntityPM.DangerousUnNumber = entityPM.DangerousUnNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousPackagingGroup))
            {
                oldEntityPM.DangerousPackagingGroup = entityPM.DangerousPackagingGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousIMDGCode))
            {
                oldEntityPM.DangerousIMDGCode = entityPM.DangerousIMDGCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousFlashPoint))
            {
                oldEntityPM.DangerousFlashPoint = entityPM.DangerousFlashPoint;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousMaterialDescription))
            {
                oldEntityPM.DangerousMaterialDescription = entityPM.DangerousMaterialDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MainHarmonize))
            {
                oldEntityPM.MainHarmonize = entityPM.MainHarmonize;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AWBHandlingInformation))
            {
                oldEntityPM.AWBHandlingInformation = entityPM.AWBHandlingInformation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AnswerOtherServicesInformation))
            {
                oldEntityPM.AnswerOtherServicesInformation = entityPM.AnswerOtherServicesInformation;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasResponse))
            {
                oldEntityPM.HasResponse = entityPM.HasResponse;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FMAAcknowledgementReason))
            {
                oldEntityPM.FMAAcknowledgementReason = entityPM.FMAAcknowledgementReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.HasErrors))
            {
                oldEntityPM.HasErrors = entityPM.HasErrors;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WaitingForResponse))
            {
                oldEntityPM.WaitingForResponse = entityPM.WaitingForResponse;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterlineId))
            {
                oldEntityPM.InterlineId = entityPM.InterlineId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingLevelCode))
            {
                oldEntityPM.BookingLevelCode = entityPM.BookingLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirlinePrefix))
            {
                oldEntityPM.AirlinePrefix = entityPM.AirlinePrefix;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingProductId))
            {
                oldEntityPM.BookingProductId = entityPM.BookingProductId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastSentByUserId))
            {
                oldEntityPM.LastSentByUserId = entityPM.LastSentByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoodsId))
            {
                oldEntityPM.DescriptionOfGoodsId = entityPM.DescriptionOfGoodsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConcurrencyGUID))
            {
                oldEntityPM.ConcurrencyGUID = entityPM.ConcurrencyGUID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountNumber))
            {
                oldEntityPM.AccountNumber = entityPM.AccountNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsTemperatureSensitive))
            {
                oldEntityPM.IsTemperatureSensitive = entityPM.IsTemperatureSensitive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastFSRStatusRequestDate))
            {
                oldEntityPM.LastFSRStatusRequestDate = entityPM.LastFSRStatusRequestDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByPartner))
            {
                oldEntityPM.UpdatedByPartner = entityPM.UpdatedByPartner;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(BookingPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AWBHandlingInformation)) //T4 find type == nText 
            {
                entityPM.AWBHandlingInformation = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AWBHandlingInformation));
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
		
		private void BuildSearchFieldsGenerated(BookingPM entityPM, Booking entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 