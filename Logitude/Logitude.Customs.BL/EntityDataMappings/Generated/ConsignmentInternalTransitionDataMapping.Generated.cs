
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
   
   public partial class ConsignmentInternalTransitionDataMapping: IMapping<ConsignmentInternalTransitionPM, ConsignmentInternalTransition>,IMappingEncodeBase64NVARCHARFields<ConsignmentInternalTransitionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         ConsignmentNumber, 
	         Tenant, 
	         SiteCode, 
	         LineNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         ConsignmentNumber, 
	         Tenant, 
	         SiteCode, 
	         LineNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConsignmentInternalTransitionPM entityPM, ConsignmentInternalTransition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SiteCode))
            {
				entityPOCO.SiteCode = entityPM.SiteCode;
			}
			}

		public void POCOToPM(ConsignmentInternalTransitionPM entityPM, ConsignmentInternalTransition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsignmentNumber))
            {
					entityPM.ConsignmentNumber = entityPOCO.ConsignmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SiteCode))
            {
					entityPM.SiteCode = entityPOCO.SiteCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

		}

		public void PMToOldPM(ConsignmentInternalTransitionPM entityPM, ConsignmentInternalTransitionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SiteCode))
            {
                oldEntityPM.SiteCode = entityPM.SiteCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConsignmentInternalTransitionPM entityPM)
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
	 