
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
   
   public partial class CustomsItemDataMapping: IMapping<CustomsItemPM, CustomsItem>,IMappingEncodeBase64NVARCHARFields<CustomsItemPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CustomsBookTypeID, 
	         FullClassification, 
	         CustomsItemCategoryID, 
	         CustomsItemHierarchicLocatioID, 
	         ComputedCheckDigit, 
	         ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CustomsBookTypeID, 
	         FullClassification, 
	         CustomsItemCategoryID, 
	         CustomsItemHierarchicLocatioID, 
	         ComputedCheckDigit, 
	         ClassificationWithCheckDigit, 
	         ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsItemPM entityPM, CustomsItem entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
				entityPOCO.CustomsBookTypeID = entityPM.CustomsBookTypeID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullClassification))
            {
				entityPOCO.FullClassification = entityPM.FullClassification;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemCategoryID))
            {
				entityPOCO.CustomsItemCategoryID = entityPM.CustomsItemCategoryID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemHierarchicLocatioID))
            {
				entityPOCO.CustomsItemHierarchicLocatioID = entityPM.CustomsItemHierarchicLocatioID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComputedCheckDigit))
            {
				entityPOCO.ComputedCheckDigit = entityPM.ComputedCheckDigit;
			}
			}

		public void POCOToPM(CustomsItemPM entityPM, CustomsItem entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsBookTypeID))
            {
					entityPM.CustomsBookTypeID = entityPOCO.CustomsBookTypeID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FullClassification))
            {
					entityPM.FullClassification = entityPOCO.FullClassification;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemCategoryID))
            {
					entityPM.CustomsItemCategoryID = entityPOCO.CustomsItemCategoryID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemHierarchicLocatioID))
            {
					entityPM.CustomsItemHierarchicLocatioID = entityPOCO.CustomsItemHierarchicLocatioID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ComputedCheckDigit))
            {
					entityPM.ComputedCheckDigit = entityPOCO.ComputedCheckDigit;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

		}

		public void PMToOldPM(CustomsItemPM entityPM, CustomsItemPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsBookTypeID))
            {
                oldEntityPM.CustomsBookTypeID = entityPM.CustomsBookTypeID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FullClassification))
            {
                oldEntityPM.FullClassification = entityPM.FullClassification;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemCategoryID))
            {
                oldEntityPM.CustomsItemCategoryID = entityPM.CustomsItemCategoryID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemHierarchicLocatioID))
            {
                oldEntityPM.CustomsItemHierarchicLocatioID = entityPM.CustomsItemHierarchicLocatioID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ComputedCheckDigit))
            {
                oldEntityPM.ComputedCheckDigit = entityPM.ComputedCheckDigit;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsItemPM entityPM)
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
	 