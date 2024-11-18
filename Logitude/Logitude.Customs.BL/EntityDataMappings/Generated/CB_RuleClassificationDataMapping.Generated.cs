
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
   
   public partial class CB_RuleClassificationDataMapping: IMapping<CB_RuleClassificationPM, CB_RuleClassification>,IMappingEncodeBase64NVARCHARFields<CB_RuleClassificationPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CB_ID, 
	         CustomsItemID, 
	         ID, 
	         CustomsBookType, 
	         Rules, 
	         ParentID, 
	         Index,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CB_ID, 
	         CustomsItemID, 
	         ID, 
	         CustomsBookType, 
	         Rules, 
	         ParentID, 
	         Index,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_RuleClassificationPM entityPM, CB_RuleClassification entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookType))
            {
				entityPOCO.CustomsBookType = entityPM.CustomsBookType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Rules))
            {
				entityPOCO.Rules = entityPM.Rules;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentID))
            {
				entityPOCO.ParentID = entityPM.ParentID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
				entityPOCO.Index = entityPM.Index;
			}
			}

		public void POCOToPM(CB_RuleClassificationPM entityPM, CB_RuleClassification entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookType))
            {
					entityPM.CustomsBookType = entityPOCO.CustomsBookType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Rules))
            {
					entityPM.Rules = entityPOCO.Rules;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentID))
            {
					entityPM.ParentID = entityPOCO.ParentID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Index))
            {
					entityPM.Index = entityPOCO.Index;
            }

		}

		public void PMToOldPM(CB_RuleClassificationPM entityPM, CB_RuleClassificationPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookType))
            {
                oldEntityPM.CustomsBookType = entityPM.CustomsBookType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Rules))
            {
                oldEntityPM.Rules = entityPM.Rules;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentID))
            {
                oldEntityPM.ParentID = entityPM.ParentID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Index))
            {
                oldEntityPM.Index = entityPM.Index;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_RuleClassificationPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Rules)) //T4 find type == nText 
            {
                entityPM.Rules = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Rules));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Index)) //T4 find type == nText 
            {
                entityPM.Index = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Index));
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
	 