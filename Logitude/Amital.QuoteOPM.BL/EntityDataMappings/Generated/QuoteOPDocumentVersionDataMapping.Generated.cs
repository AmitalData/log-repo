
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPDocumentVersionDataMapping: IMapping<QuoteOPDocumentVersionPM, QuoteOPDocumentVersion>,IMappingEncodeBase64NVARCHARFields<QuoteOPDocumentVersionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         QuoteOPId, 
	         VersionNumber, 
	         Tenant, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         VersionType, 
	         DocumentId, 
	         IsSent, 
	         QuoteOPTemplateId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         QuoteOPId, 
	         VersionNumber, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         CreatedByUserId, 
	         UpdatedByUserId, 
	         SendDate, 
	         VersionType, 
	         DocumentId, 
	         IsSent, 
	         QuoteOPTemplateId, 
	         VersionTypeName, 
	         FileSize, 
	         FileName, 
	         Extension, 
	         DisplayVersionTypeName, 
	         UpdateByUserName, 
	         CreatedByUserName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(QuoteOPDocumentVersionPM entityPM, QuoteOPDocumentVersion entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
				entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
				entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionType))
            {
				entityPOCO.VersionType = entityPM.VersionType;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentId))
            {
				entityPOCO.DocumentId = entityPM.DocumentId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSent))
            {
				entityPOCO.IsSent = entityPM.IsSent;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateId))
            {
				entityPOCO.QuoteOPTemplateId = entityPM.QuoteOPTemplateId;
			}
			}

		public void POCOToPM(QuoteOPDocumentVersionPM entityPM, QuoteOPDocumentVersion entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPId))
            {
					entityPM.QuoteOPId = entityPOCO.QuoteOPId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VersionNumber))
            {
					entityPM.VersionNumber = entityPOCO.VersionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreatedByUserId))
            {
					entityPM.CreatedByUserId = entityPOCO.CreatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdatedByUserId))
            {
					entityPM.UpdatedByUserId = entityPOCO.UpdatedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VersionType))
            {
					entityPM.VersionType = entityPOCO.VersionType;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DocumentId))
            {
					entityPM.DocumentId = entityPOCO.DocumentId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsSent))
            {
					entityPM.IsSent = entityPOCO.IsSent;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.QuoteOPTemplateId))
            {
					entityPM.QuoteOPTemplateId = entityPOCO.QuoteOPTemplateId;
            }

		}

		public void PMToOldPM(QuoteOPDocumentVersionPM entityPM, QuoteOPDocumentVersionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreatedByUserId))
            {
                oldEntityPM.CreatedByUserId = entityPM.CreatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdatedByUserId))
            {
                oldEntityPM.UpdatedByUserId = entityPM.UpdatedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VersionType))
            {
                oldEntityPM.VersionType = entityPM.VersionType;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DocumentId))
            {
                oldEntityPM.DocumentId = entityPM.DocumentId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsSent))
            {
                oldEntityPM.IsSent = entityPM.IsSent;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.QuoteOPTemplateId))
            {
                oldEntityPM.QuoteOPTemplateId = entityPM.QuoteOPTemplateId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(QuoteOPDocumentVersionPM entityPM)
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
	 