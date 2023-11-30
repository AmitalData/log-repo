
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
	         ShipmentId, 
	         ShipmentType, 
	         Tenant, 
	         SyncTo,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         ShipmentId, 
	         ShipmentType, 
	         Tenant, 
	         SyncTo,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
				entityPOCO.ShipmentId = entityPM.ShipmentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentType))
            {
				entityPOCO.ShipmentType = entityPM.ShipmentType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SyncTo))
            {
				entityPOCO.SyncTo = entityPM.SyncTo;
			}
			}

		public void POCOToPM(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueue entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentId))
            {
					entityPM.ShipmentId = entityPOCO.ShipmentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShipmentType))
            {
					entityPM.ShipmentType = entityPOCO.ShipmentType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SyncTo))
            {
					entityPM.SyncTo = entityPOCO.SyncTo;
            }

		}

		public void PMToOldPM(CargoReferencesSyncQueuePM entityPM, CargoReferencesSyncQueuePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentId))
            {
                oldEntityPM.ShipmentId = entityPM.ShipmentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShipmentType))
            {
                oldEntityPM.ShipmentType = entityPM.ShipmentType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SyncTo))
            {
                oldEntityPM.SyncTo = entityPM.SyncTo;
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
	 