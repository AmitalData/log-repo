
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
	         Id, 
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
	         ClearanceDone, 
	         PickupDate, 
	         ClearanceDate, 
	         CreateDate, 
	         SecurityKey, 
	         ConsigneeName, 
	         ShipperName, 
	         CustomerReference,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
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
	         ClearanceDone, 
	         PickupDate, 
	         ClearanceDate, 
	         CreateDate, 
	         SecurityKey, 
	         ConsigneeName, 
	         ShipperName, 
	         CustomerReference,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDone))
            {
				entityPOCO.ClearanceDone = entityPM.ClearanceDone;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDate))
            {
				entityPOCO.PickupDate = entityPM.PickupDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDate))
            {
				entityPOCO.ClearanceDate = entityPM.ClearanceDate;
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
			}

		public void POCOToPM(CargoTrackingShipmentPM entityPM, CargoTrackingShipment entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClearanceDone))
            {
					entityPM.ClearanceDone = entityPOCO.ClearanceDone;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PickupDate))
            {
					entityPM.PickupDate = entityPOCO.PickupDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClearanceDate))
            {
					entityPM.ClearanceDate = entityPOCO.ClearanceDate;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDone))
            {
                oldEntityPM.ClearanceDone = entityPM.ClearanceDone;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PickupDate))
            {
                oldEntityPM.PickupDate = entityPM.PickupDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClearanceDate))
            {
                oldEntityPM.ClearanceDate = entityPM.ClearanceDate;
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
			
		}

	    public void EncodeBase64NVARCHARFields(CargoTrackingShipmentPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

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
	 