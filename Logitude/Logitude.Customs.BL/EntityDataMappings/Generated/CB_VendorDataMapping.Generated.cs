
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
   
   public partial class CB_VendorDataMapping: IMapping<CB_VendorPM, CB_Vendor>,IMappingEncodeBase64NVARCHARFields<CB_VendorPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         ID, 
	         Title, 
	         State, 
	         EnglishCountryName, 
	         VendorSingleStringAddress,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         ID, 
	         Title, 
	         State, 
	         EnglishCountryName, 
	         VendorSingleStringAddress,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_VendorPM entityPM, CB_Vendor entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
				entityPOCO.Title = entityPM.Title;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.State))
            {
				entityPOCO.State = entityPM.State;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCountryName))
            {
				entityPOCO.EnglishCountryName = entityPM.EnglishCountryName;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorSingleStringAddress))
            {
				entityPOCO.VendorSingleStringAddress = entityPM.VendorSingleStringAddress;
			}
			}

		public void POCOToPM(CB_VendorPM entityPM, CB_Vendor entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ID))
            {
					entityPM.ID = entityPOCO.ID;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Title))
            {
					entityPM.Title = entityPOCO.Title;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.State))
            {
					entityPM.State = entityPOCO.State;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EnglishCountryName))
            {
					entityPM.EnglishCountryName = entityPOCO.EnglishCountryName;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.VendorSingleStringAddress))
            {
					entityPM.VendorSingleStringAddress = entityPOCO.VendorSingleStringAddress;
            }

		}

		public void PMToOldPM(CB_VendorPM entityPM, CB_VendorPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Title))
            {
                oldEntityPM.Title = entityPM.Title;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.State))
            {
                oldEntityPM.State = entityPM.State;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EnglishCountryName))
            {
                oldEntityPM.EnglishCountryName = entityPM.EnglishCountryName;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.VendorSingleStringAddress))
            {
                oldEntityPM.VendorSingleStringAddress = entityPM.VendorSingleStringAddress;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_VendorPM entityPM)
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
	 