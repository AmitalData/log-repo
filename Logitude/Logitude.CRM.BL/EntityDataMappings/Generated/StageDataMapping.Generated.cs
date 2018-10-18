
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class StageDataMapping: IMapping<StagePM, Stage>,IMappingEncodeBase64NVARCHARFields<StagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Code, 
	         Name, 
	         Probability, 
	         SearchFields, 
	         Tenant, 
	         IsSelectable, 
	         MaxDays, 
	         InActive,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Code, 
	         Name, 
	         Probability, 
	         SearchFields, 
	         Tenant, 
	         IsSelectable, 
	         MaxDays, 
	         InActive,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(StagePM entityPM, Stage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
				entityPOCO.Code = entityPM.Code;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
				entityPOCO.Name = entityPM.Name;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Probability))
            {
				entityPOCO.Probability = entityPM.Probability;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSelectable))
            {
				entityPOCO.IsSelectable = entityPM.IsSelectable;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaxDays))
            {
				entityPOCO.MaxDays = entityPM.MaxDays;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
				entityPOCO.InActive = entityPM.InActive;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(StagePM entityPM, Stage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Code))
            {
					entityPM.Code = entityPOCO.Code;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Name))
            {
					entityPM.Name = entityPOCO.Name;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Probability))
            {
					entityPM.Probability = entityPOCO.Probability;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSelectable))
            {
					entityPM.IsSelectable = entityPOCO.IsSelectable;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MaxDays))
            {
					entityPM.MaxDays = entityPOCO.MaxDays;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InActive))
            {
					entityPM.InActive = entityPOCO.InActive;
            }

		}

		public void PMToOldPM(StagePM entityPM, StagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Code))
            {
                oldEntityPM.Code = entityPM.Code;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Name))
            {
                oldEntityPM.Name = entityPM.Name;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Probability))
            {
                oldEntityPM.Probability = entityPM.Probability;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSelectable))
            {
                oldEntityPM.IsSelectable = entityPM.IsSelectable;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MaxDays))
            {
                oldEntityPM.MaxDays = entityPM.MaxDays;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InActive))
            {
                oldEntityPM.InActive = entityPM.InActive;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(StagePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Name)) //T4 find type == nText 
            {
                entityPM.Name = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Name));
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
		
		private void BuildSearchFieldsGenerated(StagePM entityPM, Stage entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 