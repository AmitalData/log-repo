
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
   
   public partial class NDMessageActionCodeDataMapping: IMapping<NDMessageActionCodePM, NDMessageActionCode>,IMappingEncodeBase64NVARCHARFields<NDMessageActionCodePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         StartDate, 
	         EndDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         StartDate, 
	         EndDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(NDMessageActionCodePM entityPM, NDMessageActionCode entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(NDMessageActionCodePM entityPM, NDMessageActionCode entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

		}

		public void PMToOldPM(NDMessageActionCodePM entityPM, NDMessageActionCodePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(NDMessageActionCodePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.LocalName)) //T4 find type == nText 
            {
                entityPM.LocalName = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.LocalName));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
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
		
		private void BuildSearchFieldsGenerated(NDMessageActionCodePM entityPM, NDMessageActionCode entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 