
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class LastRunDetailDataMapping: IMapping<LastRunDetailPM, LastRunDetail>,IMappingEncodeBase64NVARCHARFields<LastRunDetailPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         LastRunDate, 
	         LastRunByUserId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         LastRunDate, 
	         LastRunByUserId, 
	         LastRunByUserName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(LastRunDetailPM entityPM, LastRunDetail entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRunDate))
            {
				entityPOCO.LastRunDate = entityPM.LastRunDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRunByUserId))
            {
				entityPOCO.LastRunByUserId = entityPM.LastRunByUserId;
			}
			}

		public void POCOToPM(LastRunDetailPM entityPM, LastRunDetail entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastRunDate))
            {
					entityPM.LastRunDate = entityPOCO.LastRunDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastRunByUserId))
            {
					entityPM.LastRunByUserId = entityPOCO.LastRunByUserId;
            }

		}

		public void PMToOldPM(LastRunDetailPM entityPM, LastRunDetailPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRunDate))
            {
                oldEntityPM.LastRunDate = entityPM.LastRunDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastRunByUserId))
            {
                oldEntityPM.LastRunByUserId = entityPM.LastRunByUserId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(LastRunDetailPM entityPM)
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
	 