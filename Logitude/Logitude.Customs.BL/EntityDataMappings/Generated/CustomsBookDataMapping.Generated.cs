
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
   
   public partial class CustomsBookDataMapping: IMapping<CustomsBookPM, CustomsBook>,IMappingEncodeBase64NVARCHARFields<CustomsBookPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         LastUpdateByUserId, 
	         LastUpdateDate,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         LastUpdateByUserId, 
	         LastUpdateDate,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CustomsBookPM entityPM, CustomsBook entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateByUserId))
            {
				entityPOCO.LastUpdateByUserId = entityPM.LastUpdateByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
				entityPOCO.LastUpdateDate = entityPM.LastUpdateDate;
			}
			}

		public void POCOToPM(CustomsBookPM entityPM, CustomsBook entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateByUserId))
            {
					entityPM.LastUpdateByUserId = entityPOCO.LastUpdateByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LastUpdateDate))
            {
					entityPM.LastUpdateDate = entityPOCO.LastUpdateDate;
            }

		}

		public void PMToOldPM(CustomsBookPM entityPM, CustomsBookPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateByUserId))
            {
                oldEntityPM.LastUpdateByUserId = entityPM.LastUpdateByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LastUpdateDate))
            {
                oldEntityPM.LastUpdateDate = entityPM.LastUpdateDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CustomsBookPM entityPM)
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
	 