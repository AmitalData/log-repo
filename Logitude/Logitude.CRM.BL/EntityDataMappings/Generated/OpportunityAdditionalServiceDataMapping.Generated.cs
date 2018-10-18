
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
   
   public partial class OpportunityAdditionalServiceDataMapping: IMapping<OpportunityAdditionalServicePM, OpportunityAdditionalService>,IMappingEncodeBase64NVARCHARFields<OpportunityAdditionalServicePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         AdditionalServiceId, 
	         Tenant, 
	         Notes, 
	         NotesRightToLeft,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         OpportunityId, 
	         AdditionalServiceId, 
	         Tenant, 
	         EnglishName, 
	         Notes, 
	         NotesRightToLeft,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OpportunityAdditionalServicePM entityPM, OpportunityAdditionalService entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
				entityPOCO.Notes = entityPM.Notes;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesRightToLeft))
            {
				entityPOCO.NotesRightToLeft = entityPM.NotesRightToLeft;
			}
			}

		public void POCOToPM(OpportunityAdditionalServicePM entityPM, OpportunityAdditionalService entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AdditionalServiceId))
            {
					entityPM.AdditionalServiceId = entityPOCO.AdditionalServiceId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Notes))
            {
					entityPM.Notes = entityPOCO.Notes;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.NotesRightToLeft))
            {
					entityPM.NotesRightToLeft = entityPOCO.NotesRightToLeft;
            }

		}

		public void PMToOldPM(OpportunityAdditionalServicePM entityPM, OpportunityAdditionalServicePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Notes))
            {
                oldEntityPM.Notes = entityPM.Notes;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.NotesRightToLeft))
            {
                oldEntityPM.NotesRightToLeft = entityPM.NotesRightToLeft;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(OpportunityAdditionalServicePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Notes)) //T4 find type == nText 
            {
                entityPM.Notes = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Notes));
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
	 