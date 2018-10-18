
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
   
   public partial class DecCargoSplitCargoIdentifierDataMapping: IMapping<DecCargoSplitCargoIdentifierPM, DecCargoSplitCargoIdentifier>,IMappingEncodeBase64NVARCHARFields<DecCargoSplitCargoIdentifierPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         LineNumber, 
	         CargoIdentifierKey1, 
	         CargoIdentifierKey2, 
	         CargoIdentifierKey3,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         LineNumber, 
	         CargoIdentifierKey1, 
	         CargoIdentifierKey2, 
	         CargoIdentifierKey3,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DecCargoSplitCargoIdentifierPM entityPM, DecCargoSplitCargoIdentifier entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey1))
            {
				entityPOCO.CargoIdentifierKey1 = entityPM.CargoIdentifierKey1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey2))
            {
				entityPOCO.CargoIdentifierKey2 = entityPM.CargoIdentifierKey2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey3))
            {
				entityPOCO.CargoIdentifierKey3 = entityPM.CargoIdentifierKey3;
			}
			}

		public void POCOToPM(DecCargoSplitCargoIdentifierPM entityPM, DecCargoSplitCargoIdentifier entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationCargoSplitId))
            {
					entityPM.DeclarationCargoSplitId = entityPOCO.DeclarationCargoSplitId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LineNumber))
            {
					entityPM.LineNumber = entityPOCO.LineNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey1))
            {
					entityPM.CargoIdentifierKey1 = entityPOCO.CargoIdentifierKey1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey2))
            {
					entityPM.CargoIdentifierKey2 = entityPOCO.CargoIdentifierKey2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CargoIdentifierKey3))
            {
					entityPM.CargoIdentifierKey3 = entityPOCO.CargoIdentifierKey3;
            }

		}

		public void PMToOldPM(DecCargoSplitCargoIdentifierPM entityPM, DecCargoSplitCargoIdentifierPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey1))
            {
                oldEntityPM.CargoIdentifierKey1 = entityPM.CargoIdentifierKey1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey2))
            {
                oldEntityPM.CargoIdentifierKey2 = entityPM.CargoIdentifierKey2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CargoIdentifierKey3))
            {
                oldEntityPM.CargoIdentifierKey3 = entityPM.CargoIdentifierKey3;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DecCargoSplitCargoIdentifierPM entityPM)
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
	 