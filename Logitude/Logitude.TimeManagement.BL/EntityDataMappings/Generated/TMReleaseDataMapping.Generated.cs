
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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.EntityDataMappings
{
   
   public partial class TMReleaseDataMapping: IMapping<TMReleasePM, TMRelease>,IMappingEncodeBase64NVARCHARFields<TMReleasePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ReleaseName, 
	         FromDate, 
	         ToDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         ReleaseName, 
	         FromDate, 
	         ToDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TMReleasePM entityPM, TMRelease entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseName))
            {
				entityPOCO.ReleaseName = entityPM.ReleaseName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
				entityPOCO.FromDate = entityPM.FromDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
				entityPOCO.ToDate = entityPM.ToDate;
			}
			}

		public void POCOToPM(TMReleasePM entityPM, TMRelease entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ReleaseName))
            {
					entityPM.ReleaseName = entityPOCO.ReleaseName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FromDate))
            {
					entityPM.FromDate = entityPOCO.FromDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ToDate))
            {
					entityPM.ToDate = entityPOCO.ToDate;
            }

		}

		public void PMToOldPM(TMReleasePM entityPM, TMReleasePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ReleaseName))
            {
                oldEntityPM.ReleaseName = entityPM.ReleaseName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FromDate))
            {
                oldEntityPM.FromDate = entityPM.FromDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ToDate))
            {
                oldEntityPM.ToDate = entityPM.ToDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TMReleasePM entityPM)
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
	 