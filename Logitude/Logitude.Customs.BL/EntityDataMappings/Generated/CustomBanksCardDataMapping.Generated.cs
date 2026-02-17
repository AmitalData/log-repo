
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
   
   public partial class CustomBanksCardDataMapping: IMapping<CustomBanksCardPM, CustomBanksCard>,IMappingEncodeBase64NVARCHARFields<CustomBanksCardPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CustomBankId, 
	         CardId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CustomBankId, 
	         CustomsBankName, 
	         CardId, 
	         CardName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomBanksCardPM entityPM, CustomBanksCard entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomBankId))
            {
				entityPOCO.CustomBankId = entityPM.CustomBankId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CardId))
            {
				entityPOCO.CardId = entityPM.CardId;
			}
			}

		public void POCOToPM(CustomBanksCardPM entityPM, CustomBanksCard entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomBankId))
            {
					entityPM.CustomBankId = entityPOCO.CustomBankId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CardId))
            {
					entityPM.CardId = entityPOCO.CardId;
            }

		}

		public void PMToOldPM(CustomBanksCardPM entityPM, CustomBanksCardPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomBankId))
            {
                oldEntityPM.CustomBankId = entityPM.CustomBankId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CardId))
            {
                oldEntityPM.CardId = entityPM.CardId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomBanksCardPM entityPM)
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
	 