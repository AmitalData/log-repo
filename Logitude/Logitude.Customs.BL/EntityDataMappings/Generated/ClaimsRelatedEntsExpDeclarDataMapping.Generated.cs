
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
   
   public partial class ClaimsRelatedEntsExpDeclarDataMapping: IMapping<ClaimsRelatedEntsExpDeclarPM, ClaimsRelatedEntsExpDeclar>,IMappingEncodeBase64NVARCHARFields<ClaimsRelatedEntsExpDeclarPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         ExportDeclarationNumber,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ClaimId, 
	         Tenant, 
	         CounterKey, 
	         ExportDeclarationNumber,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntsExpDeclar entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			}

		public void POCOToPM(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntsExpDeclar entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClaimId))
            {
					entityPM.ClaimId = entityPOCO.ClaimId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CounterKey))
            {
					entityPM.CounterKey = entityPOCO.CounterKey;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ExportDeclarationNumber))
            {
					entityPM.ExportDeclarationNumber = entityPOCO.ExportDeclarationNumber;
            }

		}

		public void PMToOldPM(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntsExpDeclarPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ClaimsRelatedEntsExpDeclarPM entityPM)
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
	 