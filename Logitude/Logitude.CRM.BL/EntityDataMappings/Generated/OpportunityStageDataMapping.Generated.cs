
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
   
   public partial class OpportunityStageDataMapping: IMapping<OpportunityStagePM, OpportunityStage>,IMappingEncodeBase64NVARCHARFields<OpportunityStagePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         OpportunityId, 
	         FromStageId, 
	         ToStageId, 
	         StartDate, 
	         EndDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         OpportunityId, 
	         FromStageId, 
	         ToStageId, 
	         StartDate, 
	         EndDate, 
	         LastStageDate, 
	         OwnerId, 
	         OpportunityTypeId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(OpportunityStagePM entityPM, OpportunityStage entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
				entityPOCO.OpportunityId = entityPM.OpportunityId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromStageId))
            {
				entityPOCO.FromStageId = entityPM.FromStageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToStageId))
            {
				entityPOCO.ToStageId = entityPM.ToStageId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			}

		public void POCOToPM(OpportunityStagePM entityPM, OpportunityStage entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.OpportunityId))
            {
					entityPM.OpportunityId = entityPOCO.OpportunityId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromStageId))
            {
					entityPM.FromStageId = entityPOCO.FromStageId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToStageId))
            {
					entityPM.ToStageId = entityPOCO.ToStageId;
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

		public void PMToOldPM(OpportunityStagePM entityPM, OpportunityStagePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.OpportunityId))
            {
                oldEntityPM.OpportunityId = entityPM.OpportunityId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromStageId))
            {
                oldEntityPM.FromStageId = entityPM.FromStageId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToStageId))
            {
                oldEntityPM.ToStageId = entityPM.ToStageId;
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

	    public void EncodeBase64NVARCHARFields(OpportunityStagePM entityPM)
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
	 