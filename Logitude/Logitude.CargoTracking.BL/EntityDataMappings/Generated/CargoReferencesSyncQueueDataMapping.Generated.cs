
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
   
   public partial class CargoReferencesSyncQueueDataMapping: IMapping<CargoReferencesSyncQueuePM, CargoReferencesSyncQueue>,IMappingEncodeBase64NVARCHARFields<CargoReferencesSyncQueuePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         ForwardingShipmentId, 
	         ShipmentNeedUpdateType, 
	         Tenant,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         ForwardingShipmentId, 
	         ShipmentNeedUpdateType, 
	         Tenant,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentId))
            {
				entityPOCO.ForwardingShipmentId = entityPM.ForwardingShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNeedUpdateType))
            {
				entityPOCO.ShipmentNeedUpdateType = entityPM.ShipmentNeedUpdateType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ForwardingShipmentId))
            {
					entityPM.ForwardingShipmentId = entityPOCO.ForwardingShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentNeedUpdateType))
            {
					entityPM.ShipmentNeedUpdateType = entityPOCO.ShipmentNeedUpdateType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

		}

		public void PMToOldPM(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueuePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ForwardingShipmentId))
            {
                oldEntityPM.ForwardingShipmentId = entityPM.ForwardingShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentNeedUpdateType))
            {
                oldEntityPM.ShipmentNeedUpdateType = entityPM.ShipmentNeedUpdateType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoReferencesSyncQueuePM entityPM)
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
	 