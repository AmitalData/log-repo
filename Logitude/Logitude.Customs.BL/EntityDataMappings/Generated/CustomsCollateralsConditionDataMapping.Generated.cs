
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
   
   public partial class CustomsCollateralsConditionDataMapping: IMapping<CustomsCollateralsConditionPM, CustomsCollateralsCondition>,IMappingEncodeBase64NVARCHARFields<CustomsCollateralsConditionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CustomsCollateralId, 
	         Tenant, 
	         ConditionCode, 
	         RequestedAmount,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CustomsCollateralId, 
	         Tenant, 
	         ConditionCode, 
	         RequestedAmount, 
	         ConditionName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsCollateralsConditionPM entityPM, CustomsCollateralsCondition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestedAmount))
            {
				entityPOCO.RequestedAmount = entityPM.RequestedAmount;
			}
			}

		public void POCOToPM(CustomsCollateralsConditionPM entityPM, CustomsCollateralsCondition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsCollateralId))
            {
					entityPM.CustomsCollateralId = entityPOCO.CustomsCollateralId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConditionCode))
            {
					entityPM.ConditionCode = entityPOCO.ConditionCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequestedAmount))
            {
					entityPM.RequestedAmount = entityPOCO.RequestedAmount;
            }

		}

		public void PMToOldPM(CustomsCollateralsConditionPM entityPM, CustomsCollateralsConditionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequestedAmount))
            {
                oldEntityPM.RequestedAmount = entityPM.RequestedAmount;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsCollateralsConditionPM entityPM)
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
	 