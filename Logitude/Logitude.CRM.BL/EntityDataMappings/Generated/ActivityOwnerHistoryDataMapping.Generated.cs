
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class ActivityOwnerHistoryDataMapping: IMapping<ActivityOwnerHistoryPM, ActivityOwnerHistory>,IMappingEncodeBase64NVARCHARFields<ActivityOwnerHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ActivityId, 
	         OwnerId, 
	         ModifiedDate, 
	         NeedSynchronization,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ActivityId, 
	         OwnerId, 
	         ModifiedDate, 
	         NeedSynchronization,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ActivityOwnerHistoryPM entityPM, ActivityOwnerHistory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
				entityPOCO.ActivityId = entityPM.ActivityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
				entityPOCO.OwnerId = entityPM.OwnerId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ModifiedDate))
            {
				entityPOCO.ModifiedDate = entityPM.ModifiedDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedSynchronization))
            {
				entityPOCO.NeedSynchronization = entityPM.NeedSynchronization;
			}
			}

		public void POCOToPM(ActivityOwnerHistoryPM entityPM, ActivityOwnerHistory entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityId))
            {
					entityPM.ActivityId = entityPOCO.ActivityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OwnerId))
            {
					entityPM.OwnerId = entityPOCO.OwnerId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ModifiedDate))
            {
					entityPM.ModifiedDate = entityPOCO.ModifiedDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NeedSynchronization))
            {
					entityPM.NeedSynchronization = entityPOCO.NeedSynchronization;
            }

		}

		public void PMToOldPM(ActivityOwnerHistoryPM entityPM, ActivityOwnerHistoryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
                oldEntityPM.ActivityId = entityPM.ActivityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OwnerId))
            {
                oldEntityPM.OwnerId = entityPM.OwnerId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ModifiedDate))
            {
                oldEntityPM.ModifiedDate = entityPM.ModifiedDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NeedSynchronization))
            {
                oldEntityPM.NeedSynchronization = entityPM.NeedSynchronization;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ActivityOwnerHistoryPM entityPM)
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
	 