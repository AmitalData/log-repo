
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
   
   public partial class CB_PreferenceDataMapping: IMapping<CB_PreferencePM, CB_Preference>,IMappingEncodeBase64NVARCHARFields<CB_PreferencePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BackgroundColor, 
	         TextColor, 
	         UserId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         BackgroundColor, 
	         TextColor, 
	         UserId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(CB_PreferencePM entityPM, CB_Preference entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackgroundColor))
            {
				entityPOCO.BackgroundColor = entityPM.BackgroundColor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextColor))
            {
				entityPOCO.TextColor = entityPM.TextColor;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
				entityPOCO.UserId = entityPM.UserId;
			}
			}

		public void POCOToPM(CB_PreferencePM entityPM, CB_Preference entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BackgroundColor))
            {
					entityPM.BackgroundColor = entityPOCO.BackgroundColor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TextColor))
            {
					entityPM.TextColor = entityPOCO.TextColor;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

		}

		public void PMToOldPM(CB_PreferencePM entityPM, CB_PreferencePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BackgroundColor))
            {
                oldEntityPM.BackgroundColor = entityPM.BackgroundColor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TextColor))
            {
                oldEntityPM.TextColor = entityPM.TextColor;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
                oldEntityPM.UserId = entityPM.UserId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(CB_PreferencePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.BackgroundColor)) //T4 find type == nText 
            {
                entityPM.BackgroundColor = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.BackgroundColor));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.TextColor)) //T4 find type == nText 
            {
                entityPM.TextColor = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.TextColor));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.UserId)) //T4 find type == nText 
            {
                entityPM.UserId = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.UserId));
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
	 