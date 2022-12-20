
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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityDataMappings
{
   
   public partial class OperatorDataMapping: IMapping<OperatorPM, Operator>,IMappingEncodeBase64NVARCHARFields<OperatorPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Code, 
	         Name, 
	         SearchFields, 
	         Sign, 
	         CategoryCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Code, 
	         Name, 
	         SearchFields, 
	         Sign, 
	         CategoryCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OperatorPM entityPM, Operator entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sign))
            {
				entityPOCO.Sign = entityPM.Sign;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CategoryCode))
            {
				entityPOCO.CategoryCode = entityPM.CategoryCode;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(OperatorPM entityPM, Operator entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Sign))
            {
					entityPM.Sign = entityPOCO.Sign;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CategoryCode))
            {
					entityPM.CategoryCode = entityPOCO.CategoryCode;
            }

		}

		public void PMToOldPM(OperatorPM entityPM, OperatorPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Sign))
            {
                oldEntityPM.Sign = entityPM.Sign;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CategoryCode))
            {
                oldEntityPM.CategoryCode = entityPM.CategoryCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OperatorPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Sign)) //T4 find type == nText 
            {
                entityPM.Sign = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Sign));
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
		
		private void BuildSearchFieldsGenerated(OperatorPM entityPM, Operator entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 