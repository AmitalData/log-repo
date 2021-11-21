
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
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs; 
using Logitude.ShipmentOrderModule.Data;

namespace Logitude.ShipmentOrderModule.BL.EntityDataMappings
{
   
   public partial class ShipmentOrderDataMapping: IMapping<ShipmentOrderPM, ShipmentOrder>,IMappingEncodeBase64NVARCHARFields<ShipmentOrderPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         OrderNumber, 
	         TransportModeId, 
	         ConsigneeId, 
	         ShipperId, 
	         AgentId, 
	         IncotermId, 
	         AccountManagerId, 
	         PONumber, 
	         DescriptionOfGoods, 
	         Master, 
	         House, 
	         CarrierNumber, 
	         VesselId, 
	         ETD, 
	         ETA, 
	         ATD, 
	         ATA, 
	         CustomsAgentId, 
	         SpecialServicesTypeId, 
	         CustomerReferences, 
	         IsReadyForPickup, 
	         PickupEstimatedDateTime, 
	         PickupActualDateTime, 
	         ForwarderId, 
	         BookingConfirmationDate, 
	         ShipmentNumber, 
	         SupplyDateTime, 
	         OriginPortId, 
	         DestinationPortId, 
	         GatewayId, 
	         CasualImporterName, 
	         CasualSupplierName, 
	         ShipmentLevelCode, 
	         PODate, 
	         BookingConfirmationNumber, 
	         DirectionId, 
	         CarrierId, 
	         IsCancelled, 
	         SecurityKey, 
	         ShipmentId, 
	         Quantity, 
	         GrossWeight, 
	         Volume, 
	         CustomerId, 
	         LastExceptionDescription, 
	         LastExceptionDate, 
	         OnHandDate, 
	         IsOperationalClosed, 
	         OnHandNumber,
	         AutomaticLastUpdateDate
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         CreatedByUserId, 
	         UpdateDate, 
	         UpdatedByUserId, 
	         SearchFields, 
	         OrderNumber, 
	         TransportModeId, 
	         ConsigneeId, 
	         ShipperId, 
	         AgentId, 
	         IncotermId, 
	         AccountManagerId, 
	         PONumber, 
	         DescriptionOfGoods, 
	         Master, 
	         House, 
	         CarrierNumber, 
	         VesselId, 
	         ETD, 
	         ETA, 
	         ATD, 
	         ATA, 
	         CustomsAgentId, 
	         SpecialServicesTypeId, 
	         CustomerReferences, 
	         TransportModeName, 
	         IsReadyForPickup, 
	         PickupEstimatedDateTime, 
	         PickupActualDateTime, 
	         ForwarderId, 
	         BookingConfirmationDate, 
	         ConsigneeName, 
	         ShipperName, 
	         AgentName, 
	         IncotermCode, 
	         AccountManagerName, 
	         VesselName, 
	         CustomsAgentName, 
	         SpecialServicesTypeName, 
	         ForwarderName, 
	         ShipmentNumber, 
	         SupplyDateTime, 
	         OriginPortId, 
	         DestinationPortId, 
	         GatewayId, 
	         CasualImporterName, 
	         CasualSupplierName, 
	         ShipmentLevelCode, 
	         ShipmentLevelName, 
	         PODate, 
	         BookingConfirmationNumber, 
	         OriginPortName, 
	         DestinationPortName, 
	         GatewayName, 
	         DirectionName, 
	         DirectionId, 
	         CarrierId, 
	         CarrierName, 
	         IsCancelled, 
	         SecurityKey, 
	         ShipmentId, 
	         Quantity, 
	         GrossWeight, 
	         Volume, 
	         CustomerId, 
	         CustomerName, 
	         OriginPortCode, 
	         DestinationPortCode, 
	         GatewayCode, 
	         IncotermName, 
	         LastExceptionDescription, 
	         LastExceptionDate, 
	         OnHandDate, 
	         IsOperationalClosed, 
	         OnHandNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
				entityPOCO.OrderNumber = entityPM.OrderNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
				entityPOCO.TransportModeId = entityPM.TransportModeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
				entityPOCO.ConsigneeId = entityPM.ConsigneeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
				entityPOCO.ShipperId = entityPM.ShipperId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
				entityPOCO.AgentId = entityPM.AgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
				entityPOCO.IncotermId = entityPM.IncotermId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountManagerId))
            {
				entityPOCO.AccountManagerId = entityPM.AccountManagerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PONumber))
            {
				entityPOCO.PONumber = entityPM.PONumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
				entityPOCO.DescriptionOfGoods = entityPM.DescriptionOfGoods;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
				entityPOCO.Master = entityPM.Master;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
				entityPOCO.House = entityPM.House;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierNumber))
            {
				entityPOCO.CarrierNumber = entityPM.CarrierNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VesselId))
            {
				entityPOCO.VesselId = entityPM.VesselId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
				entityPOCO.ETD = entityPM.ETD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
				entityPOCO.ETA = entityPM.ETA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ATD))
            {
				entityPOCO.ATD = entityPM.ATD;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ATA))
            {
				entityPOCO.ATA = entityPM.ATA;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAgentId))
            {
				entityPOCO.CustomsAgentId = entityPM.CustomsAgentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesTypeId))
            {
				entityPOCO.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReferences))
            {
				entityPOCO.CustomerReferences = entityPM.CustomerReferences;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReadyForPickup))
            {
				entityPOCO.IsReadyForPickup = entityPM.IsReadyForPickup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupEstimatedDateTime))
            {
				entityPOCO.PickupEstimatedDateTime = entityPM.PickupEstimatedDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupActualDateTime))
            {
				entityPOCO.PickupActualDateTime = entityPM.PickupActualDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderId))
            {
				entityPOCO.ForwarderId = entityPM.ForwarderId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingConfirmationDate))
            {
				entityPOCO.BookingConfirmationDate = entityPM.BookingConfirmationDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
				entityPOCO.ShipmentNumber = entityPM.ShipmentNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SupplyDateTime))
            {
				entityPOCO.SupplyDateTime = entityPM.SupplyDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
				entityPOCO.OriginPortId = entityPM.OriginPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
				entityPOCO.DestinationPortId = entityPM.DestinationPortId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatewayId))
            {
				entityPOCO.GatewayId = entityPM.GatewayId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterName))
            {
				entityPOCO.CasualImporterName = entityPM.CasualImporterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
				entityPOCO.CasualSupplierName = entityPM.CasualSupplierName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
				entityPOCO.ShipmentLevelCode = entityPM.ShipmentLevelCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PODate))
            {
				entityPOCO.PODate = entityPM.PODate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingConfirmationNumber))
            {
				entityPOCO.BookingConfirmationNumber = entityPM.BookingConfirmationNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
				entityPOCO.DirectionId = entityPM.DirectionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
				entityPOCO.CarrierId = entityPM.CarrierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
				entityPOCO.IsCancelled = entityPM.IsCancelled;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityKey))
            {
				entityPOCO.SecurityKey = entityPM.SecurityKey;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
				entityPOCO.Quantity = entityPM.Quantity;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
				entityPOCO.GrossWeight = entityPM.GrossWeight;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
				entityPOCO.Volume = entityPM.Volume;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
				entityPOCO.CustomerId = entityPM.CustomerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExceptionDescription))
            {
				entityPOCO.LastExceptionDescription = entityPM.LastExceptionDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExceptionDate))
            {
				entityPOCO.LastExceptionDate = entityPM.LastExceptionDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OnHandDate))
            {
				entityPOCO.OnHandDate = entityPM.OnHandDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOperationalClosed))
            {
				entityPOCO.IsOperationalClosed = entityPM.IsOperationalClosed;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OnHandNumber))
            {
				entityPOCO.OnHandNumber = entityPM.OnHandNumber;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderNumber))
            {
					entityPM.OrderNumber = entityPOCO.OrderNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TransportModeId))
            {
					entityPM.TransportModeId = entityPOCO.TransportModeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsigneeId))
            {
					entityPM.ConsigneeId = entityPOCO.ConsigneeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipperId))
            {
					entityPM.ShipperId = entityPOCO.ShipperId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AgentId))
            {
					entityPM.AgentId = entityPOCO.AgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IncotermId))
            {
					entityPM.IncotermId = entityPOCO.IncotermId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AccountManagerId))
            {
					entityPM.AccountManagerId = entityPOCO.AccountManagerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PONumber))
            {
					entityPM.PONumber = entityPOCO.PONumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DescriptionOfGoods))
            {
					entityPM.DescriptionOfGoods = entityPOCO.DescriptionOfGoods;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Master))
            {
					entityPM.Master = entityPOCO.Master;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.House))
            {
					entityPM.House = entityPOCO.House;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarrierNumber))
            {
					entityPM.CarrierNumber = entityPOCO.CarrierNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VesselId))
            {
					entityPM.VesselId = entityPOCO.VesselId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETD))
            {
					entityPM.ETD = entityPOCO.ETD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ETA))
            {
					entityPM.ETA = entityPOCO.ETA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ATD))
            {
					entityPM.ATD = entityPOCO.ATD;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ATA))
            {
					entityPM.ATA = entityPOCO.ATA;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsAgentId))
            {
					entityPM.CustomsAgentId = entityPOCO.CustomsAgentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SpecialServicesTypeId))
            {
					entityPM.SpecialServicesTypeId = entityPOCO.SpecialServicesTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerReferences))
            {
					entityPM.CustomerReferences = entityPOCO.CustomerReferences;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsReadyForPickup))
            {
					entityPM.IsReadyForPickup = entityPOCO.IsReadyForPickup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupEstimatedDateTime))
            {
					entityPM.PickupEstimatedDateTime = entityPOCO.PickupEstimatedDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupActualDateTime))
            {
					entityPM.PickupActualDateTime = entityPOCO.PickupActualDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwarderId))
            {
					entityPM.ForwarderId = entityPOCO.ForwarderId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingConfirmationDate))
            {
					entityPM.BookingConfirmationDate = entityPOCO.BookingConfirmationDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentNumber))
            {
					entityPM.ShipmentNumber = entityPOCO.ShipmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SupplyDateTime))
            {
					entityPM.SupplyDateTime = entityPOCO.SupplyDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OriginPortId))
            {
					entityPM.OriginPortId = entityPOCO.OriginPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DestinationPortId))
            {
					entityPM.DestinationPortId = entityPOCO.DestinationPortId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GatewayId))
            {
					entityPM.GatewayId = entityPOCO.GatewayId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualImporterName))
            {
					entityPM.CasualImporterName = entityPOCO.CasualImporterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CasualSupplierName))
            {
					entityPM.CasualSupplierName = entityPOCO.CasualSupplierName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentLevelCode))
            {
					entityPM.ShipmentLevelCode = entityPOCO.ShipmentLevelCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PODate))
            {
					entityPM.PODate = entityPOCO.PODate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BookingConfirmationNumber))
            {
					entityPM.BookingConfirmationNumber = entityPOCO.BookingConfirmationNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DirectionId))
            {
					entityPM.DirectionId = entityPOCO.DirectionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CarrierId))
            {
					entityPM.CarrierId = entityPOCO.CarrierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCancelled))
            {
					entityPM.IsCancelled = entityPOCO.IsCancelled;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SecurityKey))
            {
					entityPM.SecurityKey = entityPOCO.SecurityKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Quantity))
            {
					entityPM.Quantity = entityPOCO.Quantity;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GrossWeight))
            {
					entityPM.GrossWeight = entityPOCO.GrossWeight;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Volume))
            {
					entityPM.Volume = entityPOCO.Volume;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomerId))
            {
					entityPM.CustomerId = entityPOCO.CustomerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastExceptionDescription))
            {
					entityPM.LastExceptionDescription = entityPOCO.LastExceptionDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastExceptionDate))
            {
					entityPM.LastExceptionDate = entityPOCO.LastExceptionDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OnHandDate))
            {
					entityPM.OnHandDate = entityPOCO.OnHandDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsOperationalClosed))
            {
					entityPM.IsOperationalClosed = entityPOCO.IsOperationalClosed;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OnHandNumber))
            {
					entityPM.OnHandNumber = entityPOCO.OnHandNumber;
            }

		}

		public void PMToOldPM(ShipmentOrderPM entityPM, ShipmentOrderPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderNumber))
            {
                oldEntityPM.OrderNumber = entityPM.OrderNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TransportModeId))
            {
                oldEntityPM.TransportModeId = entityPM.TransportModeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConsigneeId))
            {
                oldEntityPM.ConsigneeId = entityPM.ConsigneeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipperId))
            {
                oldEntityPM.ShipperId = entityPM.ShipperId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AgentId))
            {
                oldEntityPM.AgentId = entityPM.AgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IncotermId))
            {
                oldEntityPM.IncotermId = entityPM.IncotermId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AccountManagerId))
            {
                oldEntityPM.AccountManagerId = entityPM.AccountManagerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PONumber))
            {
                oldEntityPM.PONumber = entityPM.PONumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DescriptionOfGoods))
            {
                oldEntityPM.DescriptionOfGoods = entityPM.DescriptionOfGoods;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Master))
            {
                oldEntityPM.Master = entityPM.Master;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.House))
            {
                oldEntityPM.House = entityPM.House;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierNumber))
            {
                oldEntityPM.CarrierNumber = entityPM.CarrierNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VesselId))
            {
                oldEntityPM.VesselId = entityPM.VesselId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETD))
            {
                oldEntityPM.ETD = entityPM.ETD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ETA))
            {
                oldEntityPM.ETA = entityPM.ETA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ATD))
            {
                oldEntityPM.ATD = entityPM.ATD;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ATA))
            {
                oldEntityPM.ATA = entityPM.ATA;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsAgentId))
            {
                oldEntityPM.CustomsAgentId = entityPM.CustomsAgentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SpecialServicesTypeId))
            {
                oldEntityPM.SpecialServicesTypeId = entityPM.SpecialServicesTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerReferences))
            {
                oldEntityPM.CustomerReferences = entityPM.CustomerReferences;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsReadyForPickup))
            {
                oldEntityPM.IsReadyForPickup = entityPM.IsReadyForPickup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupEstimatedDateTime))
            {
                oldEntityPM.PickupEstimatedDateTime = entityPM.PickupEstimatedDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupActualDateTime))
            {
                oldEntityPM.PickupActualDateTime = entityPM.PickupActualDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwarderId))
            {
                oldEntityPM.ForwarderId = entityPM.ForwarderId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingConfirmationDate))
            {
                oldEntityPM.BookingConfirmationDate = entityPM.BookingConfirmationDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNumber))
            {
                oldEntityPM.ShipmentNumber = entityPM.ShipmentNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SupplyDateTime))
            {
                oldEntityPM.SupplyDateTime = entityPM.SupplyDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OriginPortId))
            {
                oldEntityPM.OriginPortId = entityPM.OriginPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DestinationPortId))
            {
                oldEntityPM.DestinationPortId = entityPM.DestinationPortId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GatewayId))
            {
                oldEntityPM.GatewayId = entityPM.GatewayId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualImporterName))
            {
                oldEntityPM.CasualImporterName = entityPM.CasualImporterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CasualSupplierName))
            {
                oldEntityPM.CasualSupplierName = entityPM.CasualSupplierName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentLevelCode))
            {
                oldEntityPM.ShipmentLevelCode = entityPM.ShipmentLevelCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PODate))
            {
                oldEntityPM.PODate = entityPM.PODate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BookingConfirmationNumber))
            {
                oldEntityPM.BookingConfirmationNumber = entityPM.BookingConfirmationNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DirectionId))
            {
                oldEntityPM.DirectionId = entityPM.DirectionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CarrierId))
            {
                oldEntityPM.CarrierId = entityPM.CarrierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCancelled))
            {
                oldEntityPM.IsCancelled = entityPM.IsCancelled;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SecurityKey))
            {
                oldEntityPM.SecurityKey = entityPM.SecurityKey;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Quantity))
            {
                oldEntityPM.Quantity = entityPM.Quantity;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GrossWeight))
            {
                oldEntityPM.GrossWeight = entityPM.GrossWeight;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Volume))
            {
                oldEntityPM.Volume = entityPM.Volume;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomerId))
            {
                oldEntityPM.CustomerId = entityPM.CustomerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExceptionDescription))
            {
                oldEntityPM.LastExceptionDescription = entityPM.LastExceptionDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastExceptionDate))
            {
                oldEntityPM.LastExceptionDate = entityPM.LastExceptionDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OnHandDate))
            {
                oldEntityPM.OnHandDate = entityPM.OnHandDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsOperationalClosed))
            {
                oldEntityPM.IsOperationalClosed = entityPM.IsOperationalClosed;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OnHandNumber))
            {
                oldEntityPM.OnHandNumber = entityPM.OnHandNumber;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ShipmentOrderPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.DescriptionOfGoods)) //T4 find type == nText 
            {
                entityPM.DescriptionOfGoods = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DescriptionOfGoods));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.LastExceptionDescription)) //T4 find type == nText 
            {
                entityPM.LastExceptionDescription = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LastExceptionDescription));
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
		
		private void BuildSearchFieldsGenerated(ShipmentOrderPM entityPM, ShipmentOrder entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 