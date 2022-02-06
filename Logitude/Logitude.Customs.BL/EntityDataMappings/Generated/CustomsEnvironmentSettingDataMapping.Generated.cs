
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
   
   public partial class CustomsEnvironmentSettingDataMapping: IMapping<CustomsEnvironmentSettingPM, CustomsEnvironmentSetting>,IMappingEncodeBase64NVARCHARFields<CustomsEnvironmentSettingPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         EnvironmentCode, 
	         UseRabbitMQ, 
	         RabbitHost, 
	         RabbitUserName, 
	         RabbitPassword,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         EnvironmentCode, 
	         UseRabbitMQ, 
	         RabbitHost, 
	         RabbitUserName, 
	         RabbitPassword,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsEnvironmentSettingPM entityPM, CustomsEnvironmentSetting entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseRabbitMQ))
            {
				entityPOCO.UseRabbitMQ = entityPM.UseRabbitMQ;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitHost))
            {
				entityPOCO.RabbitHost = entityPM.RabbitHost;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitUserName))
            {
				entityPOCO.RabbitUserName = entityPM.RabbitUserName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitPassword))
            {
				entityPOCO.RabbitPassword = entityPM.RabbitPassword;
			}
			}

		public void POCOToPM(CustomsEnvironmentSettingPM entityPM, CustomsEnvironmentSetting entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnvironmentCode))
            {
					entityPM.EnvironmentCode = entityPOCO.EnvironmentCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UseRabbitMQ))
            {
					entityPM.UseRabbitMQ = entityPOCO.UseRabbitMQ;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RabbitHost))
            {
					entityPM.RabbitHost = entityPOCO.RabbitHost;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RabbitUserName))
            {
					entityPM.RabbitUserName = entityPOCO.RabbitUserName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RabbitPassword))
            {
					entityPM.RabbitPassword = entityPOCO.RabbitPassword;
            }

		}

		public void PMToOldPM(CustomsEnvironmentSettingPM entityPM, CustomsEnvironmentSettingPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UseRabbitMQ))
            {
                oldEntityPM.UseRabbitMQ = entityPM.UseRabbitMQ;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitHost))
            {
                oldEntityPM.RabbitHost = entityPM.RabbitHost;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitUserName))
            {
                oldEntityPM.RabbitUserName = entityPM.RabbitUserName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RabbitPassword))
            {
                oldEntityPM.RabbitPassword = entityPM.RabbitPassword;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsEnvironmentSettingPM entityPM)
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
	 