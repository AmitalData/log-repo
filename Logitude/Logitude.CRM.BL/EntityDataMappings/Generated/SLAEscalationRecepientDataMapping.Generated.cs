
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
   
   public partial class SLAEscalationRecepientDataMapping: IMapping<SLAEscalationRecepientPM, SLAEscalationRecepient>,IMappingEncodeBase64NVARCHARFields<SLAEscalationRecepientPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAEscalationId, 
	         PreDefinitionId, 
	         UserId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         SLAEscalationId, 
	         PreDefinitionId, 
	         UserId, 
	         PreDefinitionName, 
	         UserName, 
	         UserEmail,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(SLAEscalationRecepientPM entityPM, SLAEscalationRecepient entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAEscalationId))
            {
				entityPOCO.SLAEscalationId = entityPM.SLAEscalationId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreDefinitionId))
            {
				entityPOCO.PreDefinitionId = entityPM.PreDefinitionId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
				entityPOCO.UserId = entityPM.UserId;
			}
			}

		public void POCOToPM(SLAEscalationRecepientPM entityPM, SLAEscalationRecepient entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SLAEscalationId))
            {
					entityPM.SLAEscalationId = entityPOCO.SLAEscalationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.PreDefinitionId))
            {
					entityPM.PreDefinitionId = entityPOCO.PreDefinitionId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

		}

		public void PMToOldPM(SLAEscalationRecepientPM entityPM, SLAEscalationRecepientPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SLAEscalationId))
            {
                oldEntityPM.SLAEscalationId = entityPM.SLAEscalationId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.PreDefinitionId))
            {
                oldEntityPM.PreDefinitionId = entityPM.PreDefinitionId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
                oldEntityPM.UserId = entityPM.UserId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(SLAEscalationRecepientPM entityPM)
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
	 