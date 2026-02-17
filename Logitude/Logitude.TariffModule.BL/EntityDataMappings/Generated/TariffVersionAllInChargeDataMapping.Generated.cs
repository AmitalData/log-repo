
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffVersionAllInChargeDataMapping: IMapping<TariffVersionAllInChargePM, TariffVersionAllInCharge>,IMappingEncodeBase64NVARCHARFields<TariffVersionAllInChargePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Version, 
	         ChargesTypeId, 
	         AddedByUserId, 
	         AddDate, 
	         TariffId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         Version, 
	         ChargesTypeId, 
	         AddedByUserId, 
	         AddDate, 
	         TariffId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TariffVersionAllInChargePM entityPM, TariffVersionAllInCharge entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
				entityPOCO.Version = entityPM.Version;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargesTypeId))
            {
				entityPOCO.ChargesTypeId = entityPM.ChargesTypeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
				entityPOCO.AddedByUserId = entityPM.AddedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
				entityPOCO.AddDate = entityPM.AddDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
				entityPOCO.TariffId = entityPM.TariffId;
			}
			}

		public void POCOToPM(TariffVersionAllInChargePM entityPM, TariffVersionAllInCharge entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Version))
            {
					entityPM.Version = entityPOCO.Version;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ChargesTypeId))
            {
					entityPM.ChargesTypeId = entityPOCO.ChargesTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedByUserId))
            {
					entityPM.AddedByUserId = entityPOCO.AddedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddDate))
            {
					entityPM.AddDate = entityPOCO.AddDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TariffId))
            {
					entityPM.TariffId = entityPOCO.TariffId;
            }

		}

		public void PMToOldPM(TariffVersionAllInChargePM entityPM, TariffVersionAllInChargePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Version))
            {
                oldEntityPM.Version = entityPM.Version;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ChargesTypeId))
            {
                oldEntityPM.ChargesTypeId = entityPM.ChargesTypeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
                oldEntityPM.AddedByUserId = entityPM.AddedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
                oldEntityPM.AddDate = entityPM.AddDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TariffId))
            {
                oldEntityPM.TariffId = entityPM.TariffId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TariffVersionAllInChargePM entityPM)
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
	 