
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
   
   public partial class ExportDeclarationClosingDataDataMapping: IMapping<ExportDeclarationClosingDataPM, ExportDeclarationClosingData>,IMappingEncodeBase64NVARCHARFields<ExportDeclarationClosingDataPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         FinalThirdCargoId, 
	         Tenant, 
	         DeclarationId, 
	         FinalCargoTypeCode, 
	         FinalManifestNumber, 
	         FinalSecondCargoId, 
	         LoadingDateTime, 
	         FinalShipCode, 
	         FinalLoadingSite,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         FinalThirdCargoId, 
	         Tenant, 
	         DeclarationId, 
	         FinalCargoTypeCode, 
	         FinalManifestNumber, 
	         FinalSecondCargoId, 
	         LoadingDateTime, 
	         FinalShipCode, 
	         FinalLoadingSite, 
	         FinalLoadingSiteName, 
	         FinalCargoTypeName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalThirdCargoId))
            {
				entityPOCO.FinalThirdCargoId = entityPM.FinalThirdCargoId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalCargoTypeCode))
            {
				entityPOCO.FinalCargoTypeCode = entityPM.FinalCargoTypeCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalManifestNumber))
            {
				entityPOCO.FinalManifestNumber = entityPM.FinalManifestNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalSecondCargoId))
            {
				entityPOCO.FinalSecondCargoId = entityPM.FinalSecondCargoId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingDateTime))
            {
				entityPOCO.LoadingDateTime = entityPM.LoadingDateTime;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalShipCode))
            {
				entityPOCO.FinalShipCode = entityPM.FinalShipCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalLoadingSite))
            {
				entityPOCO.FinalLoadingSite = entityPM.FinalLoadingSite;
			}
			}

		public void POCOToPM(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalThirdCargoId))
            {
					entityPM.FinalThirdCargoId = entityPOCO.FinalThirdCargoId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalCargoTypeCode))
            {
					entityPM.FinalCargoTypeCode = entityPOCO.FinalCargoTypeCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalManifestNumber))
            {
					entityPM.FinalManifestNumber = entityPOCO.FinalManifestNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalSecondCargoId))
            {
					entityPM.FinalSecondCargoId = entityPOCO.FinalSecondCargoId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LoadingDateTime))
            {
					entityPM.LoadingDateTime = entityPOCO.LoadingDateTime;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalShipCode))
            {
					entityPM.FinalShipCode = entityPOCO.FinalShipCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FinalLoadingSite))
            {
					entityPM.FinalLoadingSite = entityPOCO.FinalLoadingSite;
            }

		}

		public void PMToOldPM(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingDataPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalThirdCargoId))
            {
                oldEntityPM.FinalThirdCargoId = entityPM.FinalThirdCargoId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalCargoTypeCode))
            {
                oldEntityPM.FinalCargoTypeCode = entityPM.FinalCargoTypeCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalManifestNumber))
            {
                oldEntityPM.FinalManifestNumber = entityPM.FinalManifestNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalSecondCargoId))
            {
                oldEntityPM.FinalSecondCargoId = entityPM.FinalSecondCargoId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LoadingDateTime))
            {
                oldEntityPM.LoadingDateTime = entityPM.LoadingDateTime;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalShipCode))
            {
                oldEntityPM.FinalShipCode = entityPM.FinalShipCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FinalLoadingSite))
            {
                oldEntityPM.FinalLoadingSite = entityPM.FinalLoadingSite;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(ExportDeclarationClosingDataPM entityPM)
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
	 