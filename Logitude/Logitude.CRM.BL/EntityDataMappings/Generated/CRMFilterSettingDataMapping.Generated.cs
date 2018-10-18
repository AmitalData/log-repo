
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
   
   public partial class CRMFilterSettingDataMapping: IMapping<CRMFilterSettingPM, CRMFilterSetting>,IMappingEncodeBase64NVARCHARFields<CRMFilterSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UserId, 
	         ControlNameSpace, 
	         FilterName, 
	         FilterValue,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UserId, 
	         ControlNameSpace, 
	         FilterName, 
	         FilterValue,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CRMFilterSettingPM entityPM, CRMFilterSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
				entityPOCO.UserId = entityPM.UserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControlNameSpace))
            {
				entityPOCO.ControlNameSpace = entityPM.ControlNameSpace;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterName))
            {
				entityPOCO.FilterName = entityPM.FilterName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterValue))
            {
				entityPOCO.FilterValue = entityPM.FilterValue;
			}
			}

		public void POCOToPM(CRMFilterSettingPM entityPM, CRMFilterSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ControlNameSpace))
            {
					entityPM.ControlNameSpace = entityPOCO.ControlNameSpace;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FilterName))
            {
					entityPM.FilterName = entityPOCO.FilterName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FilterValue))
            {
					entityPM.FilterValue = entityPOCO.FilterValue;
            }

		}

		public void PMToOldPM(CRMFilterSettingPM entityPM, CRMFilterSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
                oldEntityPM.UserId = entityPM.UserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ControlNameSpace))
            {
                oldEntityPM.ControlNameSpace = entityPM.ControlNameSpace;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterName))
            {
                oldEntityPM.FilterName = entityPM.FilterName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FilterValue))
            {
                oldEntityPM.FilterValue = entityPM.FilterValue;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CRMFilterSettingPM entityPM)
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
	 