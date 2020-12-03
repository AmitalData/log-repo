
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
   
   public partial class CargoSealDataMapping: IMapping<CargoSealPM, CargoSeal>,IMappingEncodeBase64NVARCHARFields<CargoSealPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         CargoSealIdentifierId, 
	         Tenant, 
	         SealNumber, 
	         Remarks, 
	         SealCompletenessStateCode, 
	         SealTypeCode, 
	         UpdateReasonCode, 
	         UpdateTypeCode, 
	         Id,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         CargoSealIdentifierId, 
	         Tenant, 
	         SealNumber, 
	         Remarks, 
	         SealCompletenessStateCode, 
	         SealCompletenessStateName, 
	         SealTypeCode, 
	         SealTypeName, 
	         UpdateReasonCode, 
	         UpdateReasonName, 
	         UpdateTypeCode, 
	         UpdateTypeName, 
	         Id, 
	         CanToAdd,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CargoSealPM entityPM, CargoSeal entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoSealIdentifierId))
            {
				entityPOCO.CargoSealIdentifierId = entityPM.CargoSealIdentifierId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
				entityPOCO.SealNumber = entityPM.SealNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Remarks))
            {
				entityPOCO.Remarks = entityPM.Remarks;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealCompletenessStateCode))
            {
				entityPOCO.SealCompletenessStateCode = entityPM.SealCompletenessStateCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealTypeCode))
            {
				entityPOCO.SealTypeCode = entityPM.SealTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateReasonCode))
            {
				entityPOCO.UpdateReasonCode = entityPM.UpdateReasonCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateTypeCode))
            {
				entityPOCO.UpdateTypeCode = entityPM.UpdateTypeCode;
			}
			}

		public void POCOToPM(CargoSealPM entityPM, CargoSeal entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoSealIdentifierId))
            {
					entityPM.CargoSealIdentifierId = entityPOCO.CargoSealIdentifierId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealNumber))
            {
					entityPM.SealNumber = entityPOCO.SealNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Remarks))
            {
					entityPM.Remarks = entityPOCO.Remarks;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealCompletenessStateCode))
            {
					entityPM.SealCompletenessStateCode = entityPOCO.SealCompletenessStateCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SealTypeCode))
            {
					entityPM.SealTypeCode = entityPOCO.SealTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateReasonCode))
            {
					entityPM.UpdateReasonCode = entityPOCO.UpdateReasonCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateTypeCode))
            {
					entityPM.UpdateTypeCode = entityPOCO.UpdateTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

		}

		public void PMToOldPM(CargoSealPM entityPM, CargoSealPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoSealIdentifierId))
            {
                oldEntityPM.CargoSealIdentifierId = entityPM.CargoSealIdentifierId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealNumber))
            {
                oldEntityPM.SealNumber = entityPM.SealNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Remarks))
            {
                oldEntityPM.Remarks = entityPM.Remarks;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealCompletenessStateCode))
            {
                oldEntityPM.SealCompletenessStateCode = entityPM.SealCompletenessStateCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SealTypeCode))
            {
                oldEntityPM.SealTypeCode = entityPM.SealTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateReasonCode))
            {
                oldEntityPM.UpdateReasonCode = entityPM.UpdateReasonCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateTypeCode))
            {
                oldEntityPM.UpdateTypeCode = entityPM.UpdateTypeCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CargoSealPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Remarks)) //T4 find type == nText 
            {
                entityPM.Remarks = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Remarks));
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
	 