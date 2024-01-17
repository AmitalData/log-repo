
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
   
   public partial class CB_CustomsItemDetailsHistoryDataMapping: IMapping<CB_CustomsItemDetailsHistoryPM, CB_CustomsItemDetailsHistory>,IMappingEncodeBase64NVARCHARFields<CB_CustomsItemDetailsHistoryPM>
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
	         EnglishGoodsDescription, 
	         GoodsDescription, 
	         GoodsDescriptionRTF, 
	         EnglishGoodsDescriptionRTF, 
	         CustomsItemID, 
	         ChangeRequestTypePriority,
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
	         EnglishGoodsDescription, 
	         GoodsDescription, 
	         GoodsDescriptionRTF, 
	         EnglishGoodsDescriptionRTF, 
	         CustomsItemID, 
	         ChangeRequestTypePriority,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_CustomsItemDetailsHistoryPM entityPM, CB_CustomsItemDetailsHistory entityPOCO)
        {
			 
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishGoodsDescription))
            {
				entityPOCO.EnglishGoodsDescription = entityPM.EnglishGoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsDescription))
            {
				entityPOCO.GoodsDescription = entityPM.GoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsDescriptionRTF))
            {
				entityPOCO.GoodsDescriptionRTF = entityPM.GoodsDescriptionRTF;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishGoodsDescriptionRTF))
            {
				entityPOCO.EnglishGoodsDescriptionRTF = entityPM.EnglishGoodsDescriptionRTF;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
				entityPOCO.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
			}
			}

		public void POCOToPM(CB_CustomsItemDetailsHistoryPM entityPM, CB_CustomsItemDetailsHistory entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishGoodsDescription))
            {
					entityPM.EnglishGoodsDescription = entityPOCO.EnglishGoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsDescription))
            {
					entityPM.GoodsDescription = entityPOCO.GoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GoodsDescriptionRTF))
            {
					entityPM.GoodsDescriptionRTF = entityPOCO.GoodsDescriptionRTF;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishGoodsDescriptionRTF))
            {
					entityPM.EnglishGoodsDescriptionRTF = entityPOCO.EnglishGoodsDescriptionRTF;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChangeRequestTypePriority))
            {
					entityPM.ChangeRequestTypePriority = entityPOCO.ChangeRequestTypePriority;
            }

		}

		public void PMToOldPM(CB_CustomsItemDetailsHistoryPM entityPM, CB_CustomsItemDetailsHistoryPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishGoodsDescription))
            {
                oldEntityPM.EnglishGoodsDescription = entityPM.EnglishGoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsDescription))
            {
                oldEntityPM.GoodsDescription = entityPM.GoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GoodsDescriptionRTF))
            {
                oldEntityPM.GoodsDescriptionRTF = entityPM.GoodsDescriptionRTF;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishGoodsDescriptionRTF))
            {
                oldEntityPM.EnglishGoodsDescriptionRTF = entityPM.EnglishGoodsDescriptionRTF;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChangeRequestTypePriority))
            {
                oldEntityPM.ChangeRequestTypePriority = entityPM.ChangeRequestTypePriority;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_CustomsItemDetailsHistoryPM entityPM)
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
	 