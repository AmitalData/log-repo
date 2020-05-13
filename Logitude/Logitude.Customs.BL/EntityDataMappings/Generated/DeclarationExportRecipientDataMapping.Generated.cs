
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
   
   public partial class DeclarationExportRecipientDataMapping: IMapping<DeclarationExportRecipientPM, DeclarationExportRecipient>,IMappingEncodeBase64NVARCHARFields<DeclarationExportRecipientPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         LineNumber, 
	         RecipientName, 
	         RecipientAddress, 
	         RecipientIssueCountryCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         LineNumber, 
	         RecipientName, 
	         RecipientAddress, 
	         RecipientIssueCountryCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationExportRecipientPM entityPM, DeclarationExportRecipient entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientName))
            {
				entityPOCO.RecipientName = entityPM.RecipientName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientAddress))
            {
				entityPOCO.RecipientAddress = entityPM.RecipientAddress;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientIssueCountryCode))
            {
				entityPOCO.RecipientIssueCountryCode = entityPM.RecipientIssueCountryCode;
			}
			}

		public void POCOToPM(DeclarationExportRecipientPM entityPM, DeclarationExportRecipient entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RecipientName))
            {
					entityPM.RecipientName = entityPOCO.RecipientName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RecipientAddress))
            {
					entityPM.RecipientAddress = entityPOCO.RecipientAddress;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RecipientIssueCountryCode))
            {
					entityPM.RecipientIssueCountryCode = entityPOCO.RecipientIssueCountryCode;
            }

		}

		public void PMToOldPM(DeclarationExportRecipientPM entityPM, DeclarationExportRecipientPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientName))
            {
                oldEntityPM.RecipientName = entityPM.RecipientName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientAddress))
            {
                oldEntityPM.RecipientAddress = entityPM.RecipientAddress;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RecipientIssueCountryCode))
            {
                oldEntityPM.RecipientIssueCountryCode = entityPM.RecipientIssueCountryCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationExportRecipientPM entityPM)
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
	 