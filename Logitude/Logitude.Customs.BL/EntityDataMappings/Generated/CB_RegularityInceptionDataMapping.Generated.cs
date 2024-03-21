
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
   
   public partial class CB_RegularityInceptionDataMapping: IMapping<CB_RegularityInceptionPM, CB_RegularityInception>,IMappingEncodeBase64NVARCHARFields<CB_RegularityInceptionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityRequirementID, 
	         InterConditionsRelationshipID, 
	         IsPersonalImportIncluded, 
	         RequirementGoodsDescription, 
	         RegularityRequirementWarnID, 
	         IsCarnetIncluded, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         RegularityRequirementID, 
	         InterConditionsRelationshipID, 
	         IsPersonalImportIncluded, 
	         RequirementGoodsDescription, 
	         RegularityRequirementWarnID, 
	         IsCarnetIncluded, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_RegularityInceptionPM entityPM, CB_RegularityInception entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementID))
            {
				entityPOCO.RegularityRequirementID = entityPM.RegularityRequirementID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterConditionsRelationshipID))
            {
				entityPOCO.InterConditionsRelationshipID = entityPM.InterConditionsRelationshipID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPersonalImportIncluded))
            {
				entityPOCO.IsPersonalImportIncluded = entityPM.IsPersonalImportIncluded;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequirementGoodsDescription))
            {
				entityPOCO.RequirementGoodsDescription = entityPM.RequirementGoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementWarnID))
            {
				entityPOCO.RegularityRequirementWarnID = entityPM.RegularityRequirementWarnID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarnetIncluded))
            {
				entityPOCO.IsCarnetIncluded = entityPM.IsCarnetIncluded;
			}
			}

		public void POCOToPM(CB_RegularityInceptionPM entityPM, CB_RegularityInception entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegularityRequirementID))
            {
					entityPM.RegularityRequirementID = entityPOCO.RegularityRequirementID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.InterConditionsRelationshipID))
            {
					entityPM.InterConditionsRelationshipID = entityPOCO.InterConditionsRelationshipID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsPersonalImportIncluded))
            {
					entityPM.IsPersonalImportIncluded = entityPOCO.IsPersonalImportIncluded;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RequirementGoodsDescription))
            {
					entityPM.RequirementGoodsDescription = entityPOCO.RequirementGoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.RegularityRequirementWarnID))
            {
					entityPM.RegularityRequirementWarnID = entityPOCO.RegularityRequirementWarnID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCarnetIncluded))
            {
					entityPM.IsCarnetIncluded = entityPOCO.IsCarnetIncluded;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_RegularityInceptionPM entityPM, CB_RegularityInceptionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementID))
            {
                oldEntityPM.RegularityRequirementID = entityPM.RegularityRequirementID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.InterConditionsRelationshipID))
            {
                oldEntityPM.InterConditionsRelationshipID = entityPM.InterConditionsRelationshipID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsPersonalImportIncluded))
            {
                oldEntityPM.IsPersonalImportIncluded = entityPM.IsPersonalImportIncluded;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RequirementGoodsDescription))
            {
                oldEntityPM.RequirementGoodsDescription = entityPM.RequirementGoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.RegularityRequirementWarnID))
            {
                oldEntityPM.RegularityRequirementWarnID = entityPM.RegularityRequirementWarnID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCarnetIncluded))
            {
                oldEntityPM.IsCarnetIncluded = entityPM.IsCarnetIncluded;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_RegularityInceptionPM entityPM)
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
	 