
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
   
   public partial class CB_CustomsItemExclusionDataMapping: IMapping<CB_CustomsItemExclusionPM, CB_CustomsItemExclusion>,IMappingEncodeBase64NVARCHARFields<CB_CustomsItemExclusionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityRequirementID, 
	         CustomsItemID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityRequirementID, 
	         CustomsItemID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_CustomsItemExclusionPM entityPM, CB_CustomsItemExclusion entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementID))
            {
				entityPOCO.RegularityRequirementID = entityPM.RegularityRequirementID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			}

		public void POCOToPM(CB_CustomsItemExclusionPM entityPM, CB_CustomsItemExclusion entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegularityRequirementID))
            {
					entityPM.RegularityRequirementID = entityPOCO.RegularityRequirementID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

		}

		public void PMToOldPM(CB_CustomsItemExclusionPM entityPM, CB_CustomsItemExclusionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementID))
            {
                oldEntityPM.RegularityRequirementID = entityPM.RegularityRequirementID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_CustomsItemExclusionPM entityPM)
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
	 