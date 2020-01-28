
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
   
   public partial class CustomDocumentTypeDataMapping: IMapping<CustomDocumentTypePM, CustomDocumentType>,IMappingEncodeBase64NVARCHARFields<CustomDocumentTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         PointerLevel, 
	         AutoSetOriginalDocumentTrue, 
	         IsCourierManadatory, 
	         IsManadatory,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         EnglishName, 
	         LocalName, 
	         SearchFields, 
	         Inactive, 
	         PointerLevel, 
	         AutoSetOriginalDocumentTrue, 
	         PointerLevelName, 
	         IsCourierManadatory, 
	         IsManadatory,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PointerLevel))
            {
				entityPOCO.PointerLevel = entityPM.PointerLevel;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutoSetOriginalDocumentTrue))
            {
				entityPOCO.AutoSetOriginalDocumentTrue = entityPM.AutoSetOriginalDocumentTrue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierManadatory))
            {
				entityPOCO.IsCourierManadatory = entityPM.IsCourierManadatory;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsManadatory))
            {
				entityPOCO.IsManadatory = entityPM.IsManadatory;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PointerLevel))
            {
					entityPM.PointerLevel = entityPOCO.PointerLevel;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AutoSetOriginalDocumentTrue))
            {
					entityPM.AutoSetOriginalDocumentTrue = entityPOCO.AutoSetOriginalDocumentTrue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCourierManadatory))
            {
					entityPM.IsCourierManadatory = entityPOCO.IsCourierManadatory;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsManadatory))
            {
					entityPM.IsManadatory = entityPOCO.IsManadatory;
            }

		}

		public void PMToOldPM(CustomDocumentTypePM entityPM, CustomDocumentTypePM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PointerLevel))
            {
                oldEntityPM.PointerLevel = entityPM.PointerLevel;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AutoSetOriginalDocumentTrue))
            {
                oldEntityPM.AutoSetOriginalDocumentTrue = entityPM.AutoSetOriginalDocumentTrue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCourierManadatory))
            {
                oldEntityPM.IsCourierManadatory = entityPM.IsCourierManadatory;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsManadatory))
            {
                oldEntityPM.IsManadatory = entityPM.IsManadatory;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomDocumentTypePM entityPM)
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
		
		private void BuildSearchFieldsGenerated(CustomDocumentTypePM entityPM, CustomDocumentType entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 