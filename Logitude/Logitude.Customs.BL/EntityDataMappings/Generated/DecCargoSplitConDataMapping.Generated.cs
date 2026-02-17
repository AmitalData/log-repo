
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
   
   public partial class DecCargoSplitConDataMapping: IMapping<DecCargoSplitConPM, DecCargoSplitCon>,IMappingEncodeBase64NVARCHARFields<DecCargoSplitConPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         LineNumber, 
	         ImporterCode, 
	         ConditionCode, 
	         ProcedureCurrentCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         DeclarationCargoSplitId, 
	         Tenant, 
	         LineNumber, 
	         ImporterCode, 
	         ConditionCode, 
	         ProcedureCurrentCode, 
	         ConditionName, 
	         ProcedureCurrentName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DecCargoSplitConPM entityPM, DecCargoSplitCon entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterCode))
            {
				entityPOCO.ImporterCode = entityPM.ImporterCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConditionCode))
            {
				entityPOCO.ConditionCode = entityPM.ConditionCode;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcedureCurrentCode))
            {
				entityPOCO.ProcedureCurrentCode = entityPM.ProcedureCurrentCode;
			}
			}

		public void POCOToPM(DecCargoSplitConPM entityPM, DecCargoSplitCon entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ImporterCode))
            {
					entityPM.ImporterCode = entityPOCO.ImporterCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ConditionCode))
            {
					entityPM.ConditionCode = entityPOCO.ConditionCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProcedureCurrentCode))
            {
					entityPM.ProcedureCurrentCode = entityPOCO.ProcedureCurrentCode;
            }

		}

		public void PMToOldPM(DecCargoSplitConPM entityPM, DecCargoSplitConPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ImporterCode))
            {
                oldEntityPM.ImporterCode = entityPM.ImporterCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ConditionCode))
            {
                oldEntityPM.ConditionCode = entityPM.ConditionCode;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProcedureCurrentCode))
            {
                oldEntityPM.ProcedureCurrentCode = entityPM.ProcedureCurrentCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DecCargoSplitConPM entityPM)
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
	 