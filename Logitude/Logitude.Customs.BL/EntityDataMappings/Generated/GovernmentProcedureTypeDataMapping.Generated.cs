
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
   
   public partial class GovernmentProcedureTypeDataMapping: IMapping<GovernmentProcedureTypePM, GovernmentProcedureType>,IMappingEncodeBase64NVARCHARFields<GovernmentProcedureTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         LocalName, 
	         EnglishName, 
	         SearchFields, 
	         Inactive, 
	         IsImport, 
	         IndexOrder, 
	         IsExport,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         LocalName, 
	         EnglishName, 
	         SearchFields, 
	         Inactive, 
	         IsImport, 
	         IndexOrder, 
	         IsExport,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
				entityPOCO.LocalName = entityPM.LocalName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
				entityPOCO.EnglishName = entityPM.EnglishName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
				entityPOCO.Inactive = entityPM.Inactive;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
				entityPOCO.IsImport = entityPM.IsImport;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndexOrder))
            {
				entityPOCO.IndexOrder = entityPM.IndexOrder;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
				entityPOCO.IsExport = entityPM.IsExport;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LocalName))
            {
					entityPM.LocalName = entityPOCO.LocalName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishName))
            {
					entityPM.EnglishName = entityPOCO.EnglishName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Inactive))
            {
					entityPM.Inactive = entityPOCO.Inactive;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsImport))
            {
					entityPM.IsImport = entityPOCO.IsImport;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IndexOrder))
            {
					entityPM.IndexOrder = entityPOCO.IndexOrder;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsExport))
            {
					entityPM.IsExport = entityPOCO.IsExport;
            }

		}

		public void PMToOldPM(GovernmentProcedureTypePM entityPM, GovernmentProcedureTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LocalName))
            {
                oldEntityPM.LocalName = entityPM.LocalName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishName))
            {
                oldEntityPM.EnglishName = entityPM.EnglishName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Inactive))
            {
                oldEntityPM.Inactive = entityPM.Inactive;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsImport))
            {
                oldEntityPM.IsImport = entityPM.IsImport;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IndexOrder))
            {
                oldEntityPM.IndexOrder = entityPM.IndexOrder;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsExport))
            {
                oldEntityPM.IsExport = entityPM.IsExport;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(GovernmentProcedureTypePM entityPM)
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
		
		private void BuildSearchFieldsGenerated(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 