
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
   
   public partial class ConsignmentPackDangerDataMapping: IMapping<ConsignmentPackDangerPM, ConsignmentPackDanger>,IMappingEncodeBase64NVARCHARFields<ConsignmentPackDangerPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         LineNumber, 
	         DangerousLineNo, 
	         UNCode, 
	         DangerousGoodsPackingReqCode, 
	         FlashpointTemperature, 
	         StorageTemperature, 
	         ClassificationFourDigit,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationId, 
	         Tenant, 
	         ConsignmentNumber, 
	         LineNumber, 
	         DangerousLineNo, 
	         UNCode, 
	         UNName, 
	         DangerousGoodsPackingReqCode, 
	         FlashpointTemperature, 
	         StorageTemperature, 
	         ClassificationFourDigit,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ConsignmentPackDangerPM entityPM, ConsignmentPackDanger entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UNCode))
            {
				entityPOCO.UNCode = entityPM.UNCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousGoodsPackingReqCode))
            {
				entityPOCO.DangerousGoodsPackingReqCode = entityPM.DangerousGoodsPackingReqCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlashpointTemperature))
            {
				entityPOCO.FlashpointTemperature = entityPM.FlashpointTemperature;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageTemperature))
            {
				entityPOCO.StorageTemperature = entityPM.StorageTemperature;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationFourDigit))
            {
				entityPOCO.ClassificationFourDigit = entityPM.ClassificationFourDigit;
			}
			}

		public void POCOToPM(ConsignmentPackDangerPM entityPM, ConsignmentPackDanger entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConsignmentNumber))
            {
					entityPM.ConsignmentNumber = entityPOCO.ConsignmentNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousLineNo))
            {
					entityPM.DangerousLineNo = entityPOCO.DangerousLineNo;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UNCode))
            {
					entityPM.UNCode = entityPOCO.UNCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DangerousGoodsPackingReqCode))
            {
					entityPM.DangerousGoodsPackingReqCode = entityPOCO.DangerousGoodsPackingReqCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FlashpointTemperature))
            {
					entityPM.FlashpointTemperature = entityPOCO.FlashpointTemperature;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StorageTemperature))
            {
					entityPM.StorageTemperature = entityPOCO.StorageTemperature;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ClassificationFourDigit))
            {
					entityPM.ClassificationFourDigit = entityPOCO.ClassificationFourDigit;
            }

		}

		public void PMToOldPM(ConsignmentPackDangerPM entityPM, ConsignmentPackDangerPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UNCode))
            {
                oldEntityPM.UNCode = entityPM.UNCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DangerousGoodsPackingReqCode))
            {
                oldEntityPM.DangerousGoodsPackingReqCode = entityPM.DangerousGoodsPackingReqCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FlashpointTemperature))
            {
                oldEntityPM.FlashpointTemperature = entityPM.FlashpointTemperature;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StorageTemperature))
            {
                oldEntityPM.StorageTemperature = entityPM.StorageTemperature;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ClassificationFourDigit))
            {
                oldEntityPM.ClassificationFourDigit = entityPM.ClassificationFourDigit;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ConsignmentPackDangerPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ClassificationFourDigit)) //T4 find type == nText 
            {
                entityPM.ClassificationFourDigit = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ClassificationFourDigit));
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
	 