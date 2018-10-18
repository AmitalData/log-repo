
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
   
   public partial class CorrespondencesAttachmentDataMapping: IMapping<CorrespondencesAttachmentPM, CorrespondencesAttachment>,IMappingEncodeBase64NVARCHARFields<CorrespondencesAttachmentPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DocumentFilingId, 
	         CorrespondenceId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DocumentFilingId, 
	         CorrespondenceId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentFilingId))
            {
				entityPOCO.DocumentFilingId = entityPM.DocumentFilingId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrespondenceId))
            {
				entityPOCO.CorrespondenceId = entityPM.CorrespondenceId;
			}
			}

		public void POCOToPM(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachment entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentFilingId))
            {
					entityPM.DocumentFilingId = entityPOCO.DocumentFilingId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CorrespondenceId))
            {
					entityPM.CorrespondenceId = entityPOCO.CorrespondenceId;
            }

		}

		public void PMToOldPM(CorrespondencesAttachmentPM entityPM, CorrespondencesAttachmentPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentFilingId))
            {
                oldEntityPM.DocumentFilingId = entityPM.DocumentFilingId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CorrespondenceId))
            {
                oldEntityPM.CorrespondenceId = entityPM.CorrespondenceId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CorrespondencesAttachmentPM entityPM)
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
	 