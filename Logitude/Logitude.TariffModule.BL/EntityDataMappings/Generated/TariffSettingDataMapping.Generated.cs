
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
	         AirDefaultStepsId, 
	         LCLDefaultStepsId, 
	         ContainerDefaults, 
	         DefaultCurrencyId, 
	         AirUnitOfMeasurementCode, 
	         LCLUnitOfMeasurementCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         DefaultPriceSteps, 
	         Tenant, 
	         DefaultWarningPercentage, 
	         AirDefaultStepsId, 
	         LCLDefaultStepsId, 
	         AirDefaultSteps, 
	         LCLDefaultSteps, 
	         ContainerDefaults, 
	         DefaultCurrencyId, 
	         AirUnitOfMeasurementCode, 
	         LCLUnitOfMeasurementCode,
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirDefaultStepsId))
            {
				entityPOCO.AirDefaultStepsId = entityPM.AirDefaultStepsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LCLDefaultStepsId))
            {
				entityPOCO.LCLDefaultStepsId = entityPM.LCLDefaultStepsId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerDefaults))
            {
				entityPOCO.ContainerDefaults = entityPM.ContainerDefaults;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultCurrencyId))
            {
				entityPOCO.DefaultCurrencyId = entityPM.DefaultCurrencyId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirUnitOfMeasurementCode))
            {
				entityPOCO.AirUnitOfMeasurementCode = entityPM.AirUnitOfMeasurementCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LCLUnitOfMeasurementCode))
            {
				entityPOCO.LCLUnitOfMeasurementCode = entityPM.LCLUnitOfMeasurementCode;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirDefaultStepsId))
            {
					entityPM.AirDefaultStepsId = entityPOCO.AirDefaultStepsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LCLDefaultStepsId))
            {
					entityPM.LCLDefaultStepsId = entityPOCO.LCLDefaultStepsId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ContainerDefaults))
            {
					entityPM.ContainerDefaults = entityPOCO.ContainerDefaults;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultCurrencyId))
            {
					entityPM.DefaultCurrencyId = entityPOCO.DefaultCurrencyId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AirUnitOfMeasurementCode))
            {
					entityPM.AirUnitOfMeasurementCode = entityPOCO.AirUnitOfMeasurementCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LCLUnitOfMeasurementCode))
            {
					entityPM.LCLUnitOfMeasurementCode = entityPOCO.LCLUnitOfMeasurementCode;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirDefaultStepsId))
            {
                oldEntityPM.AirDefaultStepsId = entityPM.AirDefaultStepsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LCLDefaultStepsId))
            {
                oldEntityPM.LCLDefaultStepsId = entityPM.LCLDefaultStepsId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ContainerDefaults))
            {
                oldEntityPM.ContainerDefaults = entityPM.ContainerDefaults;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultCurrencyId))
            {
                oldEntityPM.DefaultCurrencyId = entityPM.DefaultCurrencyId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AirUnitOfMeasurementCode))
            {
                oldEntityPM.AirUnitOfMeasurementCode = entityPM.AirUnitOfMeasurementCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LCLUnitOfMeasurementCode))
            {
                oldEntityPM.LCLUnitOfMeasurementCode = entityPM.LCLUnitOfMeasurementCode;
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
	 