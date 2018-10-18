
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class NotificationTenantDefinitionDataMapping: IMapping<NotificationTenantDefinitionPM, NotificationTenantDefinition>,IMappingEncodeBase64NVARCHARFields<NotificationTenantDefinitionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         DefaultAssigneeId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Code, 
	         DefaultAssigneeId, 
	         DefaultAssigneeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(NotificationTenantDefinitionPM entityPM, NotificationTenantDefinition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
				entityPOCO.Code = entityPM.Code;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultAssigneeId))
            {
				entityPOCO.DefaultAssigneeId = entityPM.DefaultAssigneeId;
			}
			}

		public void POCOToPM(NotificationTenantDefinitionPM entityPM, NotificationTenantDefinition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultAssigneeId))
            {
					entityPM.DefaultAssigneeId = entityPOCO.DefaultAssigneeId;
            }

		}

		public void PMToOldPM(NotificationTenantDefinitionPM entityPM, NotificationTenantDefinitionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                oldEntityPM.Code = entityPM.Code;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultAssigneeId))
            {
                oldEntityPM.DefaultAssigneeId = entityPM.DefaultAssigneeId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(NotificationTenantDefinitionPM entityPM)
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
	 