
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
   
   public partial class RequiredGuaranteeTypeDataMapping: IMapping<RequiredGuaranteeTypePM, RequiredGuaranteeType>,IMappingEncodeBase64NVARCHARFields<RequiredGuaranteeTypePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         GuaranteeId, 
	         GuaranteeTypeCode, 
	         GuaranteeAmount,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         GuaranteeId, 
	         GuaranteeTypeCode, 
	         GuaranteeAmount, 
	         GuaranteeTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(RequiredGuaranteeTypePM entityPM, RequiredGuaranteeType entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeId))
            {
				entityPOCO.GuaranteeId = entityPM.GuaranteeId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeTypeCode))
            {
				entityPOCO.GuaranteeTypeCode = entityPM.GuaranteeTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeAmount))
            {
				entityPOCO.GuaranteeAmount = entityPM.GuaranteeAmount;
			}
			}

		public void POCOToPM(RequiredGuaranteeTypePM entityPM, RequiredGuaranteeType entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GuaranteeId))
            {
					entityPM.GuaranteeId = entityPOCO.GuaranteeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GuaranteeTypeCode))
            {
					entityPM.GuaranteeTypeCode = entityPOCO.GuaranteeTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.GuaranteeAmount))
            {
					entityPM.GuaranteeAmount = entityPOCO.GuaranteeAmount;
            }

		}

		public void PMToOldPM(RequiredGuaranteeTypePM entityPM, RequiredGuaranteeTypePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeId))
            {
                oldEntityPM.GuaranteeId = entityPM.GuaranteeId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeTypeCode))
            {
                oldEntityPM.GuaranteeTypeCode = entityPM.GuaranteeTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.GuaranteeAmount))
            {
                oldEntityPM.GuaranteeAmount = entityPM.GuaranteeAmount;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(RequiredGuaranteeTypePM entityPM)
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
	 