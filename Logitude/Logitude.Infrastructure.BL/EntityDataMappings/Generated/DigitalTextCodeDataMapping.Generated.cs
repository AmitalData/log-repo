
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class DigitalTextCodeDataMapping: IMapping<DigitalTextCodePM, DigitalTextCode>,IMappingEncodeBase64NVARCHARFields<DigitalTextCodePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         Labels, 
	         ProfileId, 
	         LanguageCode,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         Labels, 
	         ObjectTableName, 
	         ProfileId, 
	         ProfileCode, 
	         LanguageCode,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DigitalTextCodePM entityPM, DigitalTextCode entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
				entityPOCO.CreateDate = entityPM.CreateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
				entityPOCO.UpdateDate = entityPM.UpdateDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
				entityPOCO.ObjectTableId = entityPM.ObjectTableId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Labels))
            {
				entityPOCO.Labels = entityPM.Labels;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
				entityPOCO.ProfileId = entityPM.ProfileId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LanguageCode))
            {
				entityPOCO.LanguageCode = entityPM.LanguageCode;
			}
			}

		public void POCOToPM(DigitalTextCodePM entityPM, DigitalTextCode entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CreateDate))
            {
					entityPM.CreateDate = entityPOCO.CreateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UpdateDate))
            {
					entityPM.UpdateDate = entityPOCO.UpdateDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ObjectTableId))
            {
					entityPM.ObjectTableId = entityPOCO.ObjectTableId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Labels))
            {
					entityPM.Labels = entityPOCO.Labels;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfileId))
            {
					entityPM.ProfileId = entityPOCO.ProfileId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.LanguageCode))
            {
					entityPM.LanguageCode = entityPOCO.LanguageCode;
            }

		}

		public void PMToOldPM(DigitalTextCodePM entityPM, DigitalTextCodePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CreateDate))
            {
                oldEntityPM.CreateDate = entityPM.CreateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UpdateDate))
            {
                oldEntityPM.UpdateDate = entityPM.UpdateDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ObjectTableId))
            {
                oldEntityPM.ObjectTableId = entityPM.ObjectTableId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Labels))
            {
                oldEntityPM.Labels = entityPM.Labels;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
                oldEntityPM.ProfileId = entityPM.ProfileId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.LanguageCode))
            {
                oldEntityPM.LanguageCode = entityPM.LanguageCode;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DigitalTextCodePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.Labels)) //T4 find type == nText 
            {
                entityPM.Labels = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Labels));
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
	 