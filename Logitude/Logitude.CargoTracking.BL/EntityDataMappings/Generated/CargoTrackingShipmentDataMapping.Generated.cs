
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
	 