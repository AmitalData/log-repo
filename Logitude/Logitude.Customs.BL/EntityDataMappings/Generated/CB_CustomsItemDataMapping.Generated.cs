
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
   
   public partial class CB_CustomsItemDataMapping: IMapping<CB_CustomsItemPM, CB_CustomsItem>,IMappingEncodeBase64NVARCHARFields<CB_CustomsItemPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         FullClassification, 
	         Parent_CustomsItemID, 
	         ComputedCheckDigit, 
	         CustomsBookTypeID, 
	         CustomsItemCategoryID, 
	         CustomsItemHierarchicLocationID, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         CreateDate, 
	         UpdateDate, 
	         FullClassification, 
	         Parent_CustomsItemID, 
	         ComputedCheckDigit, 
	         CustomsBookTypeID, 
	         CustomsItemCategoryID, 
	         CustomsItemHierarchicLocationID, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_CustomsItemPM entityPM, CB_CustomsItem entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullClassification))
            {
				entityPOCO.FullClassification = entityPM.FullClassification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Parent_CustomsItemID))
            {
				entityPOCO.Parent_CustomsItemID = entityPM.Parent_CustomsItemID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComputedCheckDigit))
            {
				entityPOCO.ComputedCheckDigit = entityPM.ComputedCheckDigit;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
				entityPOCO.CustomsBookTypeID = entityPM.CustomsBookTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemCategoryID))
            {
				entityPOCO.CustomsItemCategoryID = entityPM.CustomsItemCategoryID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemHierarchicLocationID))
            {
				entityPOCO.CustomsItemHierarchicLocationID = entityPM.CustomsItemHierarchicLocationID;
			}
			}

		public void POCOToPM(CB_CustomsItemPM entityPM, CB_CustomsItem entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullClassification))
            {
					entityPM.FullClassification = entityPOCO.FullClassification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Parent_CustomsItemID))
            {
					entityPM.Parent_CustomsItemID = entityPOCO.Parent_CustomsItemID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ComputedCheckDigit))
            {
					entityPM.ComputedCheckDigit = entityPOCO.ComputedCheckDigit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookTypeID))
            {
					entityPM.CustomsBookTypeID = entityPOCO.CustomsBookTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemCategoryID))
            {
					entityPM.CustomsItemCategoryID = entityPOCO.CustomsItemCategoryID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemHierarchicLocationID))
            {
					entityPM.CustomsItemHierarchicLocationID = entityPOCO.CustomsItemHierarchicLocationID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_CustomsItemPM entityPM, CB_CustomsItemPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullClassification))
            {
                oldEntityPM.FullClassification = entityPM.FullClassification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Parent_CustomsItemID))
            {
                oldEntityPM.Parent_CustomsItemID = entityPM.Parent_CustomsItemID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComputedCheckDigit))
            {
                oldEntityPM.ComputedCheckDigit = entityPM.ComputedCheckDigit;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
                oldEntityPM.CustomsBookTypeID = entityPM.CustomsBookTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemCategoryID))
            {
                oldEntityPM.CustomsItemCategoryID = entityPM.CustomsItemCategoryID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemHierarchicLocationID))
            {
                oldEntityPM.CustomsItemHierarchicLocationID = entityPM.CustomsItemHierarchicLocationID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_CustomsItemPM entityPM)
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
	 