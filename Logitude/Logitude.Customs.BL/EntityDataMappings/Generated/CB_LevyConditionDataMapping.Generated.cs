
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
   
   public partial class CB_LevyConditionDataMapping: IMapping<CB_LevyConditionPM, CB_LevyCondition>,IMappingEncodeBase64NVARCHARFields<CB_LevyConditionPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         LevyConditionNumber, 
	         LevyGoodsDescription, 
	         CustomsItemID, 
	         VendorID, 
	         CountryGroupID, 
	         IsCountriesGroup, 
	         CountryID, 
	         TradeLevyID, 
	         StartDate, 
	         EndDate, 
	         CB_ID,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         LevyConditionNumber, 
	         LevyGoodsDescription, 
	         CustomsItemID, 
	         VendorID, 
	         CountryGroupID, 
	         IsCountriesGroup, 
	         CountryID, 
	         TradeLevyID, 
	         StartDate, 
	         EndDate, 
	         CB_ID,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_LevyConditionPM entityPM, CB_LevyCondition entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
				entityPOCO.ID = entityPM.ID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LevyConditionNumber))
            {
				entityPOCO.LevyConditionNumber = entityPM.LevyConditionNumber;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LevyGoodsDescription))
            {
				entityPOCO.LevyGoodsDescription = entityPM.LevyGoodsDescription;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
				entityPOCO.CustomsItemID = entityPM.CustomsItemID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorID))
            {
				entityPOCO.VendorID = entityPM.VendorID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryGroupID))
            {
				entityPOCO.CountryGroupID = entityPM.CountryGroupID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCountriesGroup))
            {
				entityPOCO.IsCountriesGroup = entityPM.IsCountriesGroup;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryID))
            {
				entityPOCO.CountryID = entityPM.CountryID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeLevyID))
            {
				entityPOCO.TradeLevyID = entityPM.TradeLevyID;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
				entityPOCO.StartDate = entityPM.StartDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
				entityPOCO.EndDate = entityPM.EndDate;
			}
			}

		public void POCOToPM(CB_LevyConditionPM entityPM, CB_LevyCondition entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LevyConditionNumber))
            {
					entityPM.LevyConditionNumber = entityPOCO.LevyConditionNumber;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LevyGoodsDescription))
            {
					entityPM.LevyGoodsDescription = entityPOCO.LevyGoodsDescription;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CustomsItemID))
            {
					entityPM.CustomsItemID = entityPOCO.CustomsItemID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorID))
            {
					entityPM.VendorID = entityPOCO.VendorID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryGroupID))
            {
					entityPM.CountryGroupID = entityPOCO.CountryGroupID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsCountriesGroup))
            {
					entityPM.IsCountriesGroup = entityPOCO.IsCountriesGroup;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CountryID))
            {
					entityPM.CountryID = entityPOCO.CountryID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TradeLevyID))
            {
					entityPM.TradeLevyID = entityPOCO.TradeLevyID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.StartDate))
            {
					entityPM.StartDate = entityPOCO.StartDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EndDate))
            {
					entityPM.EndDate = entityPOCO.EndDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CB_ID))
            {
					entityPM.CB_ID = entityPOCO.CB_ID;
            }

		}

		public void PMToOldPM(CB_LevyConditionPM entityPM, CB_LevyConditionPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ID))
            {
                oldEntityPM.ID = entityPM.ID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LevyConditionNumber))
            {
                oldEntityPM.LevyConditionNumber = entityPM.LevyConditionNumber;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LevyGoodsDescription))
            {
                oldEntityPM.LevyGoodsDescription = entityPM.LevyGoodsDescription;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CustomsItemID))
            {
                oldEntityPM.CustomsItemID = entityPM.CustomsItemID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorID))
            {
                oldEntityPM.VendorID = entityPM.VendorID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryGroupID))
            {
                oldEntityPM.CountryGroupID = entityPM.CountryGroupID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsCountriesGroup))
            {
                oldEntityPM.IsCountriesGroup = entityPM.IsCountriesGroup;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CountryID))
            {
                oldEntityPM.CountryID = entityPM.CountryID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TradeLevyID))
            {
                oldEntityPM.TradeLevyID = entityPM.TradeLevyID;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.StartDate))
            {
                oldEntityPM.StartDate = entityPM.StartDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EndDate))
            {
                oldEntityPM.EndDate = entityPM.EndDate;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_LevyConditionPM entityPM)
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
	 