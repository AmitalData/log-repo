
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffSettingDataMapping: IMapping<TariffSettingPM, TariffSetting>,IMappingEncodeBase64NVARCHARFields<TariffSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         DefaultPriceSteps, 
	         Tenant, 
	         DefaultWarningPercentage,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         DefaultPriceSteps, 
	         Tenant, 
	         DefaultWarningPercentage,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffSettingPM entityPM, TariffSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultPriceSteps))
            {
				entityPOCO.DefaultPriceSteps = entityPM.DefaultPriceSteps;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultWarningPercentage))
            {
				entityPOCO.DefaultWarningPercentage = entityPM.DefaultWarningPercentage;
			}
			}

		public void POCOToPM(TariffSettingPM entityPM, TariffSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultPriceSteps))
            {
					entityPM.DefaultPriceSteps = entityPOCO.DefaultPriceSteps;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultWarningPercentage))
            {
					entityPM.DefaultWarningPercentage = entityPOCO.DefaultWarningPercentage;
            }

		}

		public void PMToOldPM(TariffSettingPM entityPM, TariffSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultPriceSteps))
            {
                oldEntityPM.DefaultPriceSteps = entityPM.DefaultPriceSteps;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultWarningPercentage))
            {
                oldEntityPM.DefaultWarningPercentage = entityPM.DefaultWarningPercentage;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffSettingPM entityPM)
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
	 