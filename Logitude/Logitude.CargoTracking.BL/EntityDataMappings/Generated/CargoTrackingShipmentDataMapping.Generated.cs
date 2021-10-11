
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
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Def.EntityPMs; 
using Logitude.CargoTracking.Data;

namespace Logitude.CargoTracking.BL.EntityDataMappings
{
   
   public partial class CargoTrackingShipmentDataMapping: IMapping<CargoTrackingShipmentPM, CargoTrackingShipment>,IMappingEncodeBase64NVARCHARFields<CargoTrackingShipmentPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         EntityId, 
	         ForwardingShipmentHeaderId, 
	         CustomsShipmentHeaderId, 
	         EntityType, 
	         CurrentMilestoneCode, 
	         CurrentMilestoneDate, 
	         CustomerId, 
	         TransportModeId, 
	         Master, 
	         House, 
	         ShipmentNumber, 
	         FromPortId, 
	         ToPortId, 
	         ShipperId, 
	         ConsigneeId, 
	         GrossWeight, 
	         Volume, 
	         PickupDone, 
	         PickupDate, 
	         CreateDate, 
	         SecurityKey, 
	         ConsigneeName, 
	         ShipperName, 
	         CustomerReference, 
	         IsMainRecord, 
	         PickupEstimationDate, 
	         FromWarehouseDate, 
	         FromWarehouseEstimationDate, 
	         FromWarehouseNotes, 
	         DepartureDone, 
	         DepartureDate, 
	         DepartureEstimationDate, 
	         ArrivalDone, 
	         ArrivalDate, 
	         ArrivalEstimationDate, 
	         ToWarehouseDone, 
	         ToWarehouseDate, 
	         ToWarehouseEstimationDate, 
	         ToWarehouseNotes, 
	         CustomsPaymentDone, 
	         CustomsPaymentDate, 
	         ClearanceDone, 
	         ClearanceDate, 
	         DeliveredDone, 
	         DeliveredDate, 
	         DeliveredEstimationDate, 
	         FromWarehouseDone, 
	         FirstPickupETD, 
	         WarehouseLegActualEntryDate, 
	         WarehouseLegExpectedEntryDate, 
	         WarehouseLegRemarks, 
	         DeclarationDate, 
	         CustomsClearanceDate, 
	         Id, 
	         ContainersNumbers, 
	         PackagesQuantity, 
	         DirectionId, 
	         ShipmentLevelCode, 
	         AssignedTruckerDone, 
	         AssignedTruckerDate, 
	         AssignedTruckerEstimationDate, 
	         AssignedTruckerNotes, 
	         AssignedCustomsAgentDone, 
	         AssignedCustomsAgentDate, 
	         AssignedCustomsAgentEstDate, 
	         AssignedCustomsAgentNotes, 
	         AssignedCustomsAgentExcReason, 
	         DeliveryDone, 
	         DeliveryDate, 
	         DeliveryEstimationDate, 
	         DeliveryNotes, 
	         DeliveryExceptionReason, 
	         GrossWeightUnitCode, 
	         ForwardingShipmentNumber, 
	         ShipmentTypeCode, 
	         CurrentMilestoneExceptions, 
	         ForwardingHouse, 
	         ForwardingMaster, 
	         ForwardingShipmentLevelCode, 
	         GoodsClassificationDate, 
	         GoodsClassificationEstDate, 
	         GoodsClassificationNotes, 
	         DocumentInspectionDate, 
	         DocumentInspectionEstDate, 
	         DocumentInspectionNotes, 
	         DocumentInspectionDone, 
	         GoodsClassificationDone, 
	         GatepassArrivedDate, 
	         GatepassArrivedEstDate, 
	         GatepassArrivedNotes, 
	         GatepassArrivedDone, 
	         ImportManifest, 
	         BookingDone, 
	         BookingDate, 
	         BookingEstimationDate, 
	         BookingNotes, 
	         BookingExceptionReason,
	         CreatedDone, 
	         PrevForwardingShipmentId,
	         PaymentRequiredDone, 
	         PaymentRequiredEstimationDate, 
	         PaymentRequiredDate, 
	         PaymentRequiredNotes, 
	         PaymentReceivedDone, 
	         PaymentReceivedEstomationDate, 
	         PaymentReceivedDate, 
	         PaymentReceivedNotes,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         EntityId, 
	         ForwardingShipmentHeaderId, 
	         CustomsShipmentHeaderId, 
	         EntityType, 
	         CurrentMilestoneCode, 
	         CurrentMilestoneDate, 
	         CustomerId, 
	         TransportModeId, 
	         Master, 
	         House, 
	         ShipmentNumber, 
	         FromPortId, 
	         ToPortId, 
	         ShipperId, 
	         ConsigneeId, 
	         GrossWeight, 
	         Volume, 
	         PickupDone, 
	         PickupDate, 
	         CreateDate, 
	         SecurityKey, 
	         ConsigneeName, 
	         ShipperName, 
	         CustomerReference, 
	         IsMainRecord, 
	         PickupEstimationDate, 
	         FromWarehouseDate, 
	         FromWarehouseEstimationDate, 
	         FromWarehouseNotes, 
	         DepartureDone, 
	         DepartureDate, 
	         DepartureEstimationDate, 
	         ArrivalDone, 
	         ArrivalDate, 
	         ArrivalEstimationDate, 
	         ToWarehouseDone, 
	         ToWarehouseDate, 
	         ToWarehouseEstimationDate, 
	         ToWarehouseNotes, 
	         CustomsPaymentDone, 
	         CustomsPaymentDate, 
	         ClearanceDone, 
	         ClearanceDate, 
	         DeliveredDone, 
	         DeliveredDate, 
	         DeliveredEstimationDate, 
	         FromWarehouseDone, 
	         FirstPickupETD, 
	         WarehouseLegActualEntryDate, 
	         WarehouseLegExpectedEntryDate, 
	         WarehouseLegRemarks, 
	         DeclarationDate, 
	         CustomsClearanceDate, 
	         Id, 
	         SearchReferences, 
	         FromPortName, 
	         ToPortName, 
	         CurrentMilestoneName, 
	         TransportModeName, 
	         FromPortCountryCode, 
	         ToPortCountryCode, 
	         IsFavorite, 
	         ContainersNumbers, 
	         PackagesQuantity, 
	         DirectionId, 
	         ShipmentLevelCode, 
	         AssignedTruckerDone, 
	         AssignedTruckerDate, 
	         AssignedTruckerEstimationDate, 
	         AssignedTruckerNotes, 
	         AssignedCustomsAgentDone, 
	         AssignedCustomsAgentDate, 
	         AssignedCustomsAgentEstDate, 
	         AssignedCustomsAgentNotes, 
	         AssignedCustomsAgentExcReason, 
	         DeliveryDone, 
	         DeliveryDate, 
	         DeliveryEstimationDate, 
	         DeliveryNotes, 
	         DeliveryExceptionReason, 
	         GrossWeightUnitCode, 
	         ForwardingShipmentNumber, 
	         ShipmentTypeCode, 
	         CustomerEnglishName, 
	         CustomerLocalName, 
	         FromPortCode, 
	         ToPortCode, 
	         NumberOfPackages, 
	         CurrentMilestoneExceptions, 
	         ForwardingShipmentLevelCode, 
	         GoodsClassificationDate, 
	         GoodsClassificationEstDate, 
	         GoodsClassificationNotes, 
	         DocumentInspectionDate, 
	         DocumentInspectionEstDate, 
	         DocumentInspectionNotes, 
	         DocumentInspectionDone, 
	         GoodsClassificationDone, 
	         GatepassArrivedDate, 
	         GatepassArrivedEstDate, 
	         GatepassArrivedNotes, 
	         GatepassArrivedDone, 
	         ImportManifest, 
	         CreatedDone, 
	         PrevForwardingShipmentId, 
	         BookingDone, 
	         BookingDate, 
	         BookingEstimationDate, 
	         BookingNotes, 
	         BookingExceptionReason,
            PaymentRequiredDone,
            PaymentRequiredEstimationDate,
            PaymentRequiredDate,
            PaymentRequiredNotes,
            PaymentReceivedDone,
            PaymentReceivedEstomationDate,
            PaymentReceivedDate,
            PaymentReceivedNotes,
        }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoTrackingShipmentPM entityPM, CargoTrackingShipment entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
				entityPOCO.EntityId = entityPM.EntityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentHeaderId))
            {
				entityPOCO.ForwardingShipmentHeaderId = entityPM.ForwardingShipmentHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsShipmentHeaderId))
            {
				entityPOCO.CustomsShipmentHeaderId = entityPM.CustomsShipmentHeaderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityType))
            {
				entityPOCO.EntityType = entityPM.EntityType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneCode))
            {
				entityPOCO.CurrentMilestoneCode = entityPM.CurrentMilestoneCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneDate))
            {
				entityPOCO.CurrentMilestoneDate = entityPM.CurrentMilestoneDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
				entityPOCO.Master = entityPM.Master;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
				entityPOCO.House = entityPM.House;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
				entityPOCO.ShipmentNumber = entityPM.ShipmentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
				entityPOCO.FromPortId = entityPM.FromPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
				entityPOCO.ToPortId = entityPM.ToPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDone))
            {
				entityPOCO.PickupDone = entityPM.PickupDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDate))
            {
				entityPOCO.PickupDate = entityPM.PickupDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityKey))
            {
				entityPOCO.SecurityKey = entityPM.SecurityKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
				entityPOCO.ConsigneeName = entityPM.ConsigneeName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
				entityPOCO.ShipperName = entityPM.ShipperName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference))
            {
				entityPOCO.CustomerReference = entityPM.CustomerReference;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMainRecord))
            {
				entityPOCO.IsMainRecord = entityPM.IsMainRecord;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupEstimationDate))
            {
				entityPOCO.PickupEstimationDate = entityPM.PickupEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseDate))
            {
				entityPOCO.FromWarehouseDate = entityPM.FromWarehouseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseEstimationDate))
            {
				entityPOCO.FromWarehouseEstimationDate = entityPM.FromWarehouseEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseNotes))
            {
				entityPOCO.FromWarehouseNotes = entityPM.FromWarehouseNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureDone))
            {
				entityPOCO.DepartureDone = entityPM.DepartureDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureDate))
            {
				entityPOCO.DepartureDate = entityPM.DepartureDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureEstimationDate))
            {
				entityPOCO.DepartureEstimationDate = entityPM.DepartureEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDone))
            {
				entityPOCO.ArrivalDone = entityPM.ArrivalDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDate))
            {
				entityPOCO.ArrivalDate = entityPM.ArrivalDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalEstimationDate))
            {
				entityPOCO.ArrivalEstimationDate = entityPM.ArrivalEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseDone))
            {
				entityPOCO.ToWarehouseDone = entityPM.ToWarehouseDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseDate))
            {
				entityPOCO.ToWarehouseDate = entityPM.ToWarehouseDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseEstimationDate))
            {
				entityPOCO.ToWarehouseEstimationDate = entityPM.ToWarehouseEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseNotes))
            {
				entityPOCO.ToWarehouseNotes = entityPM.ToWarehouseNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsPaymentDone))
            {
				entityPOCO.CustomsPaymentDone = entityPM.CustomsPaymentDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsPaymentDate))
            {
				entityPOCO.CustomsPaymentDate = entityPM.CustomsPaymentDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDone))
            {
				entityPOCO.ClearanceDone = entityPM.ClearanceDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDate))
            {
				entityPOCO.ClearanceDate = entityPM.ClearanceDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredDone))
            {
				entityPOCO.DeliveredDone = entityPM.DeliveredDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredDate))
            {
				entityPOCO.DeliveredDate = entityPM.DeliveredDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredEstimationDate))
            {
				entityPOCO.DeliveredEstimationDate = entityPM.DeliveredEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseDone))
            {
				entityPOCO.FromWarehouseDone = entityPM.FromWarehouseDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstPickupETD))
            {
				entityPOCO.FirstPickupETD = entityPM.FirstPickupETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegActualEntryDate))
            {
				entityPOCO.WarehouseLegActualEntryDate = entityPM.WarehouseLegActualEntryDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegExpectedEntryDate))
            {
				entityPOCO.WarehouseLegExpectedEntryDate = entityPM.WarehouseLegExpectedEntryDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegRemarks))
            {
				entityPOCO.WarehouseLegRemarks = entityPM.WarehouseLegRemarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDate))
            {
				entityPOCO.DeclarationDate = entityPM.DeclarationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsClearanceDate))
            {
				entityPOCO.CustomsClearanceDate = entityPM.CustomsClearanceDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainersNumbers))
            {
				entityPOCO.ContainersNumbers = entityPM.ContainersNumbers;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackagesQuantity))
            {
				entityPOCO.PackagesQuantity = entityPM.PackagesQuantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
				entityPOCO.DirectionId = entityPM.DirectionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
				entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerDone))
            {
				entityPOCO.AssignedTruckerDone = entityPM.AssignedTruckerDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerDate))
            {
				entityPOCO.AssignedTruckerDate = entityPM.AssignedTruckerDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerEstimationDate))
            {
				entityPOCO.AssignedTruckerEstimationDate = entityPM.AssignedTruckerEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerNotes))
            {
				entityPOCO.AssignedTruckerNotes = entityPM.AssignedTruckerNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentDone))
            {
				entityPOCO.AssignedCustomsAgentDone = entityPM.AssignedCustomsAgentDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentDate))
            {
				entityPOCO.AssignedCustomsAgentDate = entityPM.AssignedCustomsAgentDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentEstDate))
            {
				entityPOCO.AssignedCustomsAgentEstDate = entityPM.AssignedCustomsAgentEstDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentNotes))
            {
				entityPOCO.AssignedCustomsAgentNotes = entityPM.AssignedCustomsAgentNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentExcReason))
            {
				entityPOCO.AssignedCustomsAgentExcReason = entityPM.AssignedCustomsAgentExcReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryDone))
            {
				entityPOCO.DeliveryDone = entityPM.DeliveryDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryDate))
            {
				entityPOCO.DeliveryDate = entityPM.DeliveryDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryEstimationDate))
            {
				entityPOCO.DeliveryEstimationDate = entityPM.DeliveryEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryNotes))
            {
				entityPOCO.DeliveryNotes = entityPM.DeliveryNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryExceptionReason))
            {
				entityPOCO.DeliveryExceptionReason = entityPM.DeliveryExceptionReason;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
				entityPOCO.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentNumber))
            {
				entityPOCO.ForwardingShipmentNumber = entityPM.ForwardingShipmentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeCode))
            {
				entityPOCO.ShipmentTypeCode = entityPM.ShipmentTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneExceptions))
            {
				entityPOCO.CurrentMilestoneExceptions = entityPM.CurrentMilestoneExceptions;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentLevelCode))
            {
				entityPOCO.ForwardingShipmentLevelCode = entityPM.ForwardingShipmentLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationDate))
            {
				entityPOCO.GoodsClassificationDate = entityPM.GoodsClassificationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationEstDate))
            {
				entityPOCO.GoodsClassificationEstDate = entityPM.GoodsClassificationEstDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationNotes))
            {
				entityPOCO.GoodsClassificationNotes = entityPM.GoodsClassificationNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionDate))
            {
				entityPOCO.DocumentInspectionDate = entityPM.DocumentInspectionDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionEstDate))
            {
				entityPOCO.DocumentInspectionEstDate = entityPM.DocumentInspectionEstDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionNotes))
            {
				entityPOCO.DocumentInspectionNotes = entityPM.DocumentInspectionNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionDone))
            {
				entityPOCO.DocumentInspectionDone = entityPM.DocumentInspectionDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationDone))
            {
				entityPOCO.GoodsClassificationDone = entityPM.GoodsClassificationDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedDate))
            {
				entityPOCO.GatepassArrivedDate = entityPM.GatepassArrivedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedEstDate))
            {
				entityPOCO.GatepassArrivedEstDate = entityPM.GatepassArrivedEstDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedNotes))
            {
				entityPOCO.GatepassArrivedNotes = entityPM.GatepassArrivedNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedDone))
            {
				entityPOCO.GatepassArrivedDone = entityPM.GatepassArrivedDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImportManifest))
            {
				entityPOCO.ImportManifest = entityPM.ImportManifest;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedDone))
            {
				entityPOCO.CreatedDone = entityPM.CreatedDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrevForwardingShipmentId))
            {
				entityPOCO.PrevForwardingShipmentId = entityPM.PrevForwardingShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingDone))
            {
				entityPOCO.BookingDone = entityPM.BookingDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingDate))
            {
				entityPOCO.BookingDate = entityPM.BookingDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingEstimationDate))
            {
				entityPOCO.BookingEstimationDate = entityPM.BookingEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNotes))
            {
				entityPOCO.BookingNotes = entityPM.BookingNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingExceptionReason))
            {
				entityPOCO.BookingExceptionReason = entityPM.BookingExceptionReason;
			}

			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedDone))
            {
				entityPOCO.CreatedDone = entityPM.CreatedDone;
			}
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrevForwardingShipmentId))
            {
				entityPOCO.PrevForwardingShipmentId = entityPM.PrevForwardingShipmentId;
			}			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredDone))
            {
				entityPOCO.PaymentRequiredDone = entityPM.PaymentRequiredDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredEstimationDate))
            {
				entityPOCO.PaymentRequiredEstimationDate = entityPM.PaymentRequiredEstimationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredDate))
            {
				entityPOCO.PaymentRequiredDate = entityPM.PaymentRequiredDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredNotes))
            {
				entityPOCO.PaymentRequiredNotes = entityPM.PaymentRequiredNotes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedDone))
            {
				entityPOCO.PaymentReceivedDone = entityPM.PaymentReceivedDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedEstomationDate))
            {
				entityPOCO.PaymentReceivedEstomationDate = entityPM.PaymentReceivedEstomationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedDate))
            {
				entityPOCO.PaymentReceivedDate = entityPM.PaymentReceivedDate;
			}
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedNotes))
            {
				entityPOCO.PaymentReceivedNotes = entityPM.PaymentReceivedNotes;
			}
		}

		public void POCOToPM(CargoTrackingShipmentPM entityPM, CargoTrackingShipment entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityId))
            {
					entityPM.EntityId = entityPOCO.EntityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwardingShipmentHeaderId))
            {
					entityPM.ForwardingShipmentHeaderId = entityPOCO.ForwardingShipmentHeaderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsShipmentHeaderId))
            {
					entityPM.CustomsShipmentHeaderId = entityPOCO.CustomsShipmentHeaderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityType))
            {
					entityPM.EntityType = entityPOCO.EntityType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrentMilestoneCode))
            {
					entityPM.CurrentMilestoneCode = entityPOCO.CurrentMilestoneCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrentMilestoneDate))
            {
					entityPM.CurrentMilestoneDate = entityPOCO.CurrentMilestoneDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Master))
            {
					entityPM.Master = entityPOCO.Master;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.House))
            {
					entityPM.House = entityPOCO.House;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentNumber))
            {
					entityPM.ShipmentNumber = entityPOCO.ShipmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromPortId))
            {
					entityPM.FromPortId = entityPOCO.FromPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToPortId))
            {
					entityPM.ToPortId = entityPOCO.ToPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDone))
            {
					entityPM.PickupDone = entityPOCO.PickupDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDate))
            {
					entityPM.PickupDate = entityPOCO.PickupDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecurityKey))
            {
					entityPM.SecurityKey = entityPOCO.SecurityKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeName))
            {
					entityPM.ConsigneeName = entityPOCO.ConsigneeName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperName))
            {
					entityPM.ShipperName = entityPOCO.ShipperName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerReference))
            {
					entityPM.CustomerReference = entityPOCO.CustomerReference;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsMainRecord))
            {
					entityPM.IsMainRecord = entityPOCO.IsMainRecord;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupEstimationDate))
            {
					entityPM.PickupEstimationDate = entityPOCO.PickupEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromWarehouseDate))
            {
					entityPM.FromWarehouseDate = entityPOCO.FromWarehouseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromWarehouseEstimationDate))
            {
					entityPM.FromWarehouseEstimationDate = entityPOCO.FromWarehouseEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromWarehouseNotes))
            {
					entityPM.FromWarehouseNotes = entityPOCO.FromWarehouseNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartureDone))
            {
					entityPM.DepartureDone = entityPOCO.DepartureDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartureDate))
            {
					entityPM.DepartureDate = entityPOCO.DepartureDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DepartureEstimationDate))
            {
					entityPM.DepartureEstimationDate = entityPOCO.DepartureEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ArrivalDone))
            {
					entityPM.ArrivalDone = entityPOCO.ArrivalDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ArrivalDate))
            {
					entityPM.ArrivalDate = entityPOCO.ArrivalDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ArrivalEstimationDate))
            {
					entityPM.ArrivalEstimationDate = entityPOCO.ArrivalEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToWarehouseDone))
            {
					entityPM.ToWarehouseDone = entityPOCO.ToWarehouseDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToWarehouseDate))
            {
					entityPM.ToWarehouseDate = entityPOCO.ToWarehouseDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToWarehouseEstimationDate))
            {
					entityPM.ToWarehouseEstimationDate = entityPOCO.ToWarehouseEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToWarehouseNotes))
            {
					entityPM.ToWarehouseNotes = entityPOCO.ToWarehouseNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsPaymentDone))
            {
					entityPM.CustomsPaymentDone = entityPOCO.CustomsPaymentDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsPaymentDate))
            {
					entityPM.CustomsPaymentDate = entityPOCO.CustomsPaymentDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClearanceDone))
            {
					entityPM.ClearanceDone = entityPOCO.ClearanceDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClearanceDate))
            {
					entityPM.ClearanceDate = entityPOCO.ClearanceDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveredDone))
            {
					entityPM.DeliveredDone = entityPOCO.DeliveredDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveredDate))
            {
					entityPM.DeliveredDate = entityPOCO.DeliveredDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveredEstimationDate))
            {
					entityPM.DeliveredEstimationDate = entityPOCO.DeliveredEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromWarehouseDone))
            {
					entityPM.FromWarehouseDone = entityPOCO.FromWarehouseDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FirstPickupETD))
            {
					entityPM.FirstPickupETD = entityPOCO.FirstPickupETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WarehouseLegActualEntryDate))
            {
					entityPM.WarehouseLegActualEntryDate = entityPOCO.WarehouseLegActualEntryDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WarehouseLegExpectedEntryDate))
            {
					entityPM.WarehouseLegExpectedEntryDate = entityPOCO.WarehouseLegExpectedEntryDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.WarehouseLegRemarks))
            {
					entityPM.WarehouseLegRemarks = entityPOCO.WarehouseLegRemarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationDate))
            {
					entityPM.DeclarationDate = entityPOCO.DeclarationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsClearanceDate))
            {
					entityPM.CustomsClearanceDate = entityPOCO.CustomsClearanceDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainersNumbers))
            {
					entityPM.ContainersNumbers = entityPOCO.ContainersNumbers;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PackagesQuantity))
            {
					entityPM.PackagesQuantity = entityPOCO.PackagesQuantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionId))
            {
					entityPM.DirectionId = entityPOCO.DirectionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentLevelCode))
            {
					entityPM.ShipmentLevelCode = entityPOCO.ShipmentLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedTruckerDone))
            {
					entityPM.AssignedTruckerDone = entityPOCO.AssignedTruckerDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedTruckerDate))
            {
					entityPM.AssignedTruckerDate = entityPOCO.AssignedTruckerDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedTruckerEstimationDate))
            {
					entityPM.AssignedTruckerEstimationDate = entityPOCO.AssignedTruckerEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedTruckerNotes))
            {
					entityPM.AssignedTruckerNotes = entityPOCO.AssignedTruckerNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedCustomsAgentDone))
            {
					entityPM.AssignedCustomsAgentDone = entityPOCO.AssignedCustomsAgentDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedCustomsAgentDate))
            {
					entityPM.AssignedCustomsAgentDate = entityPOCO.AssignedCustomsAgentDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedCustomsAgentEstDate))
            {
					entityPM.AssignedCustomsAgentEstDate = entityPOCO.AssignedCustomsAgentEstDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedCustomsAgentNotes))
            {
					entityPM.AssignedCustomsAgentNotes = entityPOCO.AssignedCustomsAgentNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AssignedCustomsAgentExcReason))
            {
					entityPM.AssignedCustomsAgentExcReason = entityPOCO.AssignedCustomsAgentExcReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryDone))
            {
					entityPM.DeliveryDone = entityPOCO.DeliveryDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryDate))
            {
					entityPM.DeliveryDate = entityPOCO.DeliveryDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryEstimationDate))
            {
					entityPM.DeliveryEstimationDate = entityPOCO.DeliveryEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryNotes))
            {
					entityPM.DeliveryNotes = entityPOCO.DeliveryNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeliveryExceptionReason))
            {
					entityPM.DeliveryExceptionReason = entityPOCO.DeliveryExceptionReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeightUnitCode))
            {
					entityPM.GrossWeightUnitCode = entityPOCO.GrossWeightUnitCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwardingShipmentNumber))
            {
					entityPM.ForwardingShipmentNumber = entityPOCO.ForwardingShipmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentTypeCode))
            {
					entityPM.ShipmentTypeCode = entityPOCO.ShipmentTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CurrentMilestoneExceptions))
            {
					entityPM.CurrentMilestoneExceptions = entityPOCO.CurrentMilestoneExceptions;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwardingShipmentLevelCode))
            {
					entityPM.ForwardingShipmentLevelCode = entityPOCO.ForwardingShipmentLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsClassificationDate))
            {
					entityPM.GoodsClassificationDate = entityPOCO.GoodsClassificationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsClassificationEstDate))
            {
					entityPM.GoodsClassificationEstDate = entityPOCO.GoodsClassificationEstDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsClassificationNotes))
            {
					entityPM.GoodsClassificationNotes = entityPOCO.GoodsClassificationNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentInspectionDate))
            {
					entityPM.DocumentInspectionDate = entityPOCO.DocumentInspectionDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentInspectionEstDate))
            {
					entityPM.DocumentInspectionEstDate = entityPOCO.DocumentInspectionEstDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentInspectionNotes))
            {
					entityPM.DocumentInspectionNotes = entityPOCO.DocumentInspectionNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentInspectionDone))
            {
					entityPM.DocumentInspectionDone = entityPOCO.DocumentInspectionDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsClassificationDone))
            {
					entityPM.GoodsClassificationDone = entityPOCO.GoodsClassificationDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassArrivedDate))
            {
					entityPM.GatepassArrivedDate = entityPOCO.GatepassArrivedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassArrivedEstDate))
            {
					entityPM.GatepassArrivedEstDate = entityPOCO.GatepassArrivedEstDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassArrivedNotes))
            {
					entityPM.GatepassArrivedNotes = entityPOCO.GatepassArrivedNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatepassArrivedDone))
            {
					entityPM.GatepassArrivedDone = entityPOCO.GatepassArrivedDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImportManifest))
            {
					entityPM.ImportManifest = entityPOCO.ImportManifest;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedDone))
            {
					entityPM.CreatedDone = entityPOCO.CreatedDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PrevForwardingShipmentId))
            {
					entityPM.PrevForwardingShipmentId = entityPOCO.PrevForwardingShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingDone))
            {
					entityPM.BookingDone = entityPOCO.BookingDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingDate))
            {
					entityPM.BookingDate = entityPOCO.BookingDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingEstimationDate))
            {
					entityPM.BookingEstimationDate = entityPOCO.BookingEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentRequiredDone))
            {
					entityPM.PaymentRequiredDone = entityPOCO.PaymentRequiredDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentRequiredEstimationDate))
            {
					entityPM.PaymentRequiredEstimationDate = entityPOCO.PaymentRequiredEstimationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentRequiredDate))
            {
					entityPM.PaymentRequiredDate = entityPOCO.PaymentRequiredDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentRequiredNotes))
            {
					entityPM.PaymentRequiredNotes = entityPOCO.PaymentRequiredNotes;
            }


			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentReceivedDone))
            {
					entityPM.PaymentReceivedDone = entityPOCO.PaymentReceivedDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentReceivedEstomationDate))
            {
					entityPM.PaymentReceivedEstomationDate = entityPOCO.PaymentReceivedEstomationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentReceivedDate))
            {
					entityPM.PaymentReceivedDate = entityPOCO.PaymentReceivedDate;
			}
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingNotes))
            {
					entityPM.BookingNotes = entityPOCO.BookingNotes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingExceptionReason))
            {
					entityPM.BookingExceptionReason = entityPOCO.BookingExceptionReason;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PaymentReceivedNotes))
            {
					entityPM.PaymentReceivedNotes = entityPOCO.PaymentReceivedNotes;
            }

		}

		public void PMToOldPM(CargoTrackingShipmentPM entityPM, CargoTrackingShipmentPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityId))
            {
                oldEntityPM.EntityId = entityPM.EntityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentHeaderId))
            {
                oldEntityPM.ForwardingShipmentHeaderId = entityPM.ForwardingShipmentHeaderId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsShipmentHeaderId))
            {
                oldEntityPM.CustomsShipmentHeaderId = entityPM.CustomsShipmentHeaderId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityType))
            {
                oldEntityPM.EntityType = entityPM.EntityType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneCode))
            {
                oldEntityPM.CurrentMilestoneCode = entityPM.CurrentMilestoneCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneDate))
            {
                oldEntityPM.CurrentMilestoneDate = entityPM.CurrentMilestoneDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
                oldEntityPM.Master = entityPM.Master;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
                oldEntityPM.House = entityPM.House;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
                oldEntityPM.ShipmentNumber = entityPM.ShipmentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromPortId))
            {
                oldEntityPM.FromPortId = entityPM.FromPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToPortId))
            {
                oldEntityPM.ToPortId = entityPM.ToPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDone))
            {
                oldEntityPM.PickupDone = entityPM.PickupDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDate))
            {
                oldEntityPM.PickupDate = entityPM.PickupDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityKey))
            {
                oldEntityPM.SecurityKey = entityPM.SecurityKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeName))
            {
                oldEntityPM.ConsigneeName = entityPM.ConsigneeName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperName))
            {
                oldEntityPM.ShipperName = entityPM.ShipperName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReference))
            {
                oldEntityPM.CustomerReference = entityPM.CustomerReference;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsMainRecord))
            {
                oldEntityPM.IsMainRecord = entityPM.IsMainRecord;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupEstimationDate))
            {
                oldEntityPM.PickupEstimationDate = entityPM.PickupEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseDate))
            {
                oldEntityPM.FromWarehouseDate = entityPM.FromWarehouseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseEstimationDate))
            {
                oldEntityPM.FromWarehouseEstimationDate = entityPM.FromWarehouseEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseNotes))
            {
                oldEntityPM.FromWarehouseNotes = entityPM.FromWarehouseNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureDone))
            {
                oldEntityPM.DepartureDone = entityPM.DepartureDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureDate))
            {
                oldEntityPM.DepartureDate = entityPM.DepartureDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DepartureEstimationDate))
            {
                oldEntityPM.DepartureEstimationDate = entityPM.DepartureEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDone))
            {
                oldEntityPM.ArrivalDone = entityPM.ArrivalDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalDate))
            {
                oldEntityPM.ArrivalDate = entityPM.ArrivalDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ArrivalEstimationDate))
            {
                oldEntityPM.ArrivalEstimationDate = entityPM.ArrivalEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseDone))
            {
                oldEntityPM.ToWarehouseDone = entityPM.ToWarehouseDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseDate))
            {
                oldEntityPM.ToWarehouseDate = entityPM.ToWarehouseDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseEstimationDate))
            {
                oldEntityPM.ToWarehouseEstimationDate = entityPM.ToWarehouseEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToWarehouseNotes))
            {
                oldEntityPM.ToWarehouseNotes = entityPM.ToWarehouseNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsPaymentDone))
            {
                oldEntityPM.CustomsPaymentDone = entityPM.CustomsPaymentDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsPaymentDate))
            {
                oldEntityPM.CustomsPaymentDate = entityPM.CustomsPaymentDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDone))
            {
                oldEntityPM.ClearanceDone = entityPM.ClearanceDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDate))
            {
                oldEntityPM.ClearanceDate = entityPM.ClearanceDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredDone))
            {
                oldEntityPM.DeliveredDone = entityPM.DeliveredDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredDate))
            {
                oldEntityPM.DeliveredDate = entityPM.DeliveredDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveredEstimationDate))
            {
                oldEntityPM.DeliveredEstimationDate = entityPM.DeliveredEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromWarehouseDone))
            {
                oldEntityPM.FromWarehouseDone = entityPM.FromWarehouseDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FirstPickupETD))
            {
                oldEntityPM.FirstPickupETD = entityPM.FirstPickupETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegActualEntryDate))
            {
                oldEntityPM.WarehouseLegActualEntryDate = entityPM.WarehouseLegActualEntryDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegExpectedEntryDate))
            {
                oldEntityPM.WarehouseLegExpectedEntryDate = entityPM.WarehouseLegExpectedEntryDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.WarehouseLegRemarks))
            {
                oldEntityPM.WarehouseLegRemarks = entityPM.WarehouseLegRemarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeclarationDate))
            {
                oldEntityPM.DeclarationDate = entityPM.DeclarationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsClearanceDate))
            {
                oldEntityPM.CustomsClearanceDate = entityPM.CustomsClearanceDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainersNumbers))
            {
                oldEntityPM.ContainersNumbers = entityPM.ContainersNumbers;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PackagesQuantity))
            {
                oldEntityPM.PackagesQuantity = entityPM.PackagesQuantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
                oldEntityPM.DirectionId = entityPM.DirectionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
                oldEntityPM.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerDone))
            {
                oldEntityPM.AssignedTruckerDone = entityPM.AssignedTruckerDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerDate))
            {
                oldEntityPM.AssignedTruckerDate = entityPM.AssignedTruckerDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerEstimationDate))
            {
                oldEntityPM.AssignedTruckerEstimationDate = entityPM.AssignedTruckerEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedTruckerNotes))
            {
                oldEntityPM.AssignedTruckerNotes = entityPM.AssignedTruckerNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentDone))
            {
                oldEntityPM.AssignedCustomsAgentDone = entityPM.AssignedCustomsAgentDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentDate))
            {
                oldEntityPM.AssignedCustomsAgentDate = entityPM.AssignedCustomsAgentDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentEstDate))
            {
                oldEntityPM.AssignedCustomsAgentEstDate = entityPM.AssignedCustomsAgentEstDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentNotes))
            {
                oldEntityPM.AssignedCustomsAgentNotes = entityPM.AssignedCustomsAgentNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AssignedCustomsAgentExcReason))
            {
                oldEntityPM.AssignedCustomsAgentExcReason = entityPM.AssignedCustomsAgentExcReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryDone))
            {
                oldEntityPM.DeliveryDone = entityPM.DeliveryDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryDate))
            {
                oldEntityPM.DeliveryDate = entityPM.DeliveryDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryEstimationDate))
            {
                oldEntityPM.DeliveryEstimationDate = entityPM.DeliveryEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryNotes))
            {
                oldEntityPM.DeliveryNotes = entityPM.DeliveryNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DeliveryExceptionReason))
            {
                oldEntityPM.DeliveryExceptionReason = entityPM.DeliveryExceptionReason;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeightUnitCode))
            {
                oldEntityPM.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentNumber))
            {
                oldEntityPM.ForwardingShipmentNumber = entityPM.ForwardingShipmentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentTypeCode))
            {
                oldEntityPM.ShipmentTypeCode = entityPM.ShipmentTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CurrentMilestoneExceptions))
            {
                oldEntityPM.CurrentMilestoneExceptions = entityPM.CurrentMilestoneExceptions;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentLevelCode))
            {
                oldEntityPM.ForwardingShipmentLevelCode = entityPM.ForwardingShipmentLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationDate))
            {
                oldEntityPM.GoodsClassificationDate = entityPM.GoodsClassificationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationEstDate))
            {
                oldEntityPM.GoodsClassificationEstDate = entityPM.GoodsClassificationEstDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationNotes))
            {
                oldEntityPM.GoodsClassificationNotes = entityPM.GoodsClassificationNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionDate))
            {
                oldEntityPM.DocumentInspectionDate = entityPM.DocumentInspectionDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionEstDate))
            {
                oldEntityPM.DocumentInspectionEstDate = entityPM.DocumentInspectionEstDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionNotes))
            {
                oldEntityPM.DocumentInspectionNotes = entityPM.DocumentInspectionNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentInspectionDone))
            {
                oldEntityPM.DocumentInspectionDone = entityPM.DocumentInspectionDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsClassificationDone))
            {
                oldEntityPM.GoodsClassificationDone = entityPM.GoodsClassificationDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedDate))
            {
                oldEntityPM.GatepassArrivedDate = entityPM.GatepassArrivedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedEstDate))
            {
                oldEntityPM.GatepassArrivedEstDate = entityPM.GatepassArrivedEstDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedNotes))
            {
                oldEntityPM.GatepassArrivedNotes = entityPM.GatepassArrivedNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatepassArrivedDone))
            {
                oldEntityPM.GatepassArrivedDone = entityPM.GatepassArrivedDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImportManifest))
            {
                oldEntityPM.ImportManifest = entityPM.ImportManifest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedDone))
            {
                oldEntityPM.CreatedDone = entityPM.CreatedDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrevForwardingShipmentId))
            {
                oldEntityPM.PrevForwardingShipmentId = entityPM.PrevForwardingShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingDone))
            {
                oldEntityPM.BookingDone = entityPM.BookingDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingDate))
            {
                oldEntityPM.BookingDate = entityPM.BookingDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingEstimationDate))
            {
                oldEntityPM.BookingEstimationDate = entityPM.BookingEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingNotes))
            {
                oldEntityPM.BookingNotes = entityPM.BookingNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingExceptionReason))
            {
                oldEntityPM.BookingExceptionReason = entityPM.BookingExceptionReason;
            }

			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedDone))
			{
				oldEntityPM.CreatedDone = entityPM.CreatedDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredDone))
            {
                oldEntityPM.PaymentRequiredDone = entityPM.PaymentRequiredDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredEstimationDate))
            {
                oldEntityPM.PaymentRequiredEstimationDate = entityPM.PaymentRequiredEstimationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredDate))
            {
                oldEntityPM.PaymentRequiredDate = entityPM.PaymentRequiredDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentRequiredNotes))
            {
                oldEntityPM.PaymentRequiredNotes = entityPM.PaymentRequiredNotes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedDone))
            {
                oldEntityPM.PaymentReceivedDone = entityPM.PaymentReceivedDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedEstomationDate))
            {
                oldEntityPM.PaymentReceivedEstomationDate = entityPM.PaymentReceivedEstomationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedDate))
            {
                oldEntityPM.PaymentReceivedDate = entityPM.PaymentReceivedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PaymentReceivedNotes))
            {
                oldEntityPM.PaymentReceivedNotes = entityPM.PaymentReceivedNotes;
			}
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImportManifest))
            {
                oldEntityPM.ImportManifest = entityPM.ImportManifest;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PrevForwardingShipmentId))
            {
                oldEntityPM.PrevForwardingShipmentId = entityPM.PrevForwardingShipmentId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingShipmentPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.FromWarehouseNotes)) //T4 find type == nText 
            {
                entityPM.FromWarehouseNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FromWarehouseNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ToWarehouseNotes)) //T4 find type == nText 
            {
                entityPM.ToWarehouseNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ToWarehouseNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.WarehouseLegRemarks)) //T4 find type == nText 
            {
                entityPM.WarehouseLegRemarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.WarehouseLegRemarks));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.ContainersNumbers)) //T4 find type == nText 
            {
                entityPM.ContainersNumbers = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ContainersNumbers));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AssignedTruckerNotes)) //T4 find type == nText 
            {
                entityPM.AssignedTruckerNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AssignedTruckerNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AssignedCustomsAgentNotes)) //T4 find type == nText 
            {
                entityPM.AssignedCustomsAgentNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AssignedCustomsAgentNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.AssignedCustomsAgentExcReason)) //T4 find type == nText 
            {
                entityPM.AssignedCustomsAgentExcReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.AssignedCustomsAgentExcReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DeliveryNotes)) //T4 find type == nText 
            {
                entityPM.DeliveryNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DeliveryNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DeliveryExceptionReason)) //T4 find type == nText 
            {
                entityPM.DeliveryExceptionReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DeliveryExceptionReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.CurrentMilestoneExceptions)) //T4 find type == nText 
            {
                entityPM.CurrentMilestoneExceptions = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.CurrentMilestoneExceptions));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.GoodsClassificationNotes)) //T4 find type == nText 
            {
                entityPM.GoodsClassificationNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.GoodsClassificationNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DocumentInspectionNotes)) //T4 find type == nText 
            {
                entityPM.DocumentInspectionNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DocumentInspectionNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.GatepassArrivedNotes)) //T4 find type == nText 
            {
                entityPM.GatepassArrivedNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.GatepassArrivedNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.BookingNotes)) //T4 find type == nText 
            {
                entityPM.BookingNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BookingNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.BookingExceptionReason)) //T4 find type == nText 
            {
                entityPM.BookingExceptionReason = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BookingExceptionReason));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PaymentRequiredNotes)) //T4 find type == nText 
            {
                entityPM.PaymentRequiredNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PaymentRequiredNotes));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.PaymentReceivedNotes)) //T4 find type == nText 
            {
                entityPM.PaymentReceivedNotes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.PaymentReceivedNotes));
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
			  
   }
}
	 