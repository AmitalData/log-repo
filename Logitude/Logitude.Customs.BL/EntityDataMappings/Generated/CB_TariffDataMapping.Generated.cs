
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
   
   public partial class CB_TariffDataMapping: IMapping<CB_TariffPM, CB_Tariff>,IMappingEncodeBase64NVARCHARFields<CB_TariffPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TradeAgreementID, 
	         CustomsItemID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         TradeAgreementID, 
	         CustomsItemID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_TariffPM entityPM, CB_Tariff entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementID))
            {
				entityPOCO.TradeAgreementID = entityPM.TradeAgreementID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			}

		public void POCOToPM(CB_TariffPM entityPM, CB_Tariff entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementID))
            {
					entityPM.TradeAgreementID = entityPOCO.TradeAgreementID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

		}

		public void PMToOldPM(CB_TariffPM entityPM, CB_TariffPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementID))
            {
                oldEntityPM.TradeAgreementID = entityPM.TradeAgreementID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_TariffPM entityPM)
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
	 