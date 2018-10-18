
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
   
   public partial class ActivityInviteeDataMapping: IMapping<ActivityInviteePM, ActivityInvitee>,IMappingEncodeBase64NVARCHARFields<ActivityInviteePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         ActivityId, 
	         ContactId, 
	         Tenant, 
	         Email, 
	         IsRequired,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         ActivityId, 
	         ContactId, 
	         Tenant, 
	         Email, 
	         IsRequired, 
	         ContactName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ActivityInviteePM entityPM, ActivityInvitee entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
				entityPOCO.ActivityId = entityPM.ActivityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
				entityPOCO.ContactId = entityPM.ContactId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Email))
            {
				entityPOCO.Email = entityPM.Email;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRequired))
            {
				entityPOCO.IsRequired = entityPM.IsRequired;
			}
			}

		public void POCOToPM(ActivityInviteePM entityPM, ActivityInvitee entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ActivityId))
            {
					entityPM.ActivityId = entityPOCO.ActivityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContactId))
            {
					entityPM.ContactId = entityPOCO.ContactId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Email))
            {
					entityPM.Email = entityPOCO.Email;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsRequired))
            {
					entityPM.IsRequired = entityPOCO.IsRequired;
            }

		}

		public void PMToOldPM(ActivityInviteePM entityPM, ActivityInviteePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ActivityId))
            {
                oldEntityPM.ActivityId = entityPM.ActivityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContactId))
            {
                oldEntityPM.ContactId = entityPM.ContactId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Email))
            {
                oldEntityPM.Email = entityPM.Email;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsRequired))
            {
                oldEntityPM.IsRequired = entityPM.IsRequired;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ActivityInviteePM entityPM)
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
	 