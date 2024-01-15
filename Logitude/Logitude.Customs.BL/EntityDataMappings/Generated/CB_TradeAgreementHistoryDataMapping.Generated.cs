
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
   
   public partial class CB_TradeAgreementHistoryDataMapping: IMapping<CB_TradeAgreementHistoryPM, CB_TradeAgreementHistory>,IMappingEncodeBase64NVARCHARFields<CB_TradeAgreementHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         Version, 
	         TradeAgreementID, 
	         ChangeRequestTypePriority,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         Version, 
	         TradeAgreementID, 
	         ChangeRequestTypePriority,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_TradeAgreementHistoryPM entityPM, CB_TradeAgreementHistory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
				entityPOCO.EntityStatusID = entityPM.EntityStatusID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
				entityPOCO.Version = entityPM.Version;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementID))
            {
				entityPOCO.TradeAgreementID = entityPM.TradeAgreementID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			}

		public void POCOToPM(CB_TradeAgreementHistoryPM entityPM, CB_TradeAgreementHistory entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EntityStatusID))
            {
					entityPM.EntityStatusID = entityPOCO.EntityStatusID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Version))
            {
					entityPM.Version = entityPOCO.Version;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeAgreementID))
            {
					entityPM.TradeAgreementID = entityPOCO.TradeAgreementID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

		}

		public void PMToOldPM(CB_TradeAgreementHistoryPM entityPM, CB_TradeAgreementHistoryPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EntityStatusID))
            {
                oldEntityPM.EntityStatusID = entityPM.EntityStatusID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
                oldEntityPM.Version = entityPM.Version;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeAgreementID))
            {
                oldEntityPM.TradeAgreementID = entityPM.TradeAgreementID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_TradeAgreementHistoryPM entityPM)
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
	 