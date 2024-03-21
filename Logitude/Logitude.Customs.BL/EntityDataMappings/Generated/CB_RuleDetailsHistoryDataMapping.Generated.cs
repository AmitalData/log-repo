
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
   
   public partial class CB_RuleDetailsHistoryDataMapping: IMapping<CB_RuleDetailsHistoryPM, CB_RuleDetailsHistory>,IMappingEncodeBase64NVARCHARFields<CB_RuleDetailsHistoryPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         Title, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         RuleID, 
	         Rules, 
	         EnglishRules, 
	         OrderinalPostion, 
	         Parent_RuleDetailsHistoryID, 
	         ChangeRequestTypePriority, 
	         RulesRTF, 
	         EnglishRulesRTF, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         Title, 
	         StartDate, 
	         EndDate, 
	         EntityStatusID, 
	         RuleID, 
	         Rules, 
	         EnglishRules, 
	         OrderinalPostion, 
	         Parent_RuleDetailsHistoryID, 
	         ChangeRequestTypePriority, 
	         RulesRTF, 
	         EnglishRulesRTF, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_RuleDetailsHistoryPM entityPM, CB_RuleDetailsHistory entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RuleID))
            {
				entityPOCO.RuleID = entityPM.RuleID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Rules))
            {
				entityPOCO.Rules = entityPM.Rules;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishRules))
            {
				entityPOCO.EnglishRules = entityPM.EnglishRules;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderinalPostion))
            {
				entityPOCO.OrderinalPostion = entityPM.OrderinalPostion;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Parent_RuleDetailsHistoryID))
            {
				entityPOCO.Parent_RuleDetailsHistoryID = entityPM.Parent_RuleDetailsHistoryID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RulesRTF))
            {
				entityPOCO.RulesRTF = entityPM.RulesRTF;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishRulesRTF))
            {
				entityPOCO.EnglishRulesRTF = entityPM.EnglishRulesRTF;
			}
			}

		public void POCOToPM(CB_RuleDetailsHistoryPM entityPM, CB_RuleDetailsHistory entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RuleID))
            {
					entityPM.RuleID = entityPOCO.RuleID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Rules))
            {
					entityPM.Rules = entityPOCO.Rules;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishRules))
            {
					entityPM.EnglishRules = entityPOCO.EnglishRules;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OrderinalPostion))
            {
					entityPM.OrderinalPostion = entityPOCO.OrderinalPostion;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Parent_RuleDetailsHistoryID))
            {
					entityPM.Parent_RuleDetailsHistoryID = entityPOCO.Parent_RuleDetailsHistoryID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RulesRTF))
            {
					entityPM.RulesRTF = entityPOCO.RulesRTF;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishRulesRTF))
            {
					entityPM.EnglishRulesRTF = entityPOCO.EnglishRulesRTF;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_RuleDetailsHistoryPM entityPM, CB_RuleDetailsHistoryPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RuleID))
            {
                oldEntityPM.RuleID = entityPM.RuleID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Rules))
            {
                oldEntityPM.Rules = entityPM.Rules;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishRules))
            {
                oldEntityPM.EnglishRules = entityPM.EnglishRules;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OrderinalPostion))
            {
                oldEntityPM.OrderinalPostion = entityPM.OrderinalPostion;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Parent_RuleDetailsHistoryID))
            {
                oldEntityPM.Parent_RuleDetailsHistoryID = entityPM.Parent_RuleDetailsHistoryID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RulesRTF))
            {
                oldEntityPM.RulesRTF = entityPM.RulesRTF;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishRulesRTF))
            {
                oldEntityPM.EnglishRulesRTF = entityPM.EnglishRulesRTF;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_RuleDetailsHistoryPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Rules)) //T4 find type == nText 
            {
                entityPM.Rules = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Rules));
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
	 