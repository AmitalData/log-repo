
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
   
   public partial class DigitalFieldSecurityDataMapping: IMapping<DigitalFieldSecurityPM, DigitalFieldSecurity>,IMappingEncodeBase64NVARCHARFields<DigitalFieldSecurityPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         DefaultSettings, 
	         ProfileId, 
	         ParentObjectTableId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         CreateDate, 
	         UpdateDate, 
	         ObjectTableId, 
	         DefaultSettings, 
	         ProfileId, 
	         ProfileName, 
	         ObjectTableName, 
	         ProfileCode, 
	         ParentObjectTableId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DigitalFieldSecurityPM entityPM, DigitalFieldSecurity entityPOCO)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultSettings))
            {
				entityPOCO.DefaultSettings = entityPM.DefaultSettings;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
				entityPOCO.ProfileId = entityPM.ProfileId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentObjectTableId))
            {
				entityPOCO.ParentObjectTableId = entityPM.ParentObjectTableId;
			}
			}

		public void POCOToPM(DigitalFieldSecurityPM entityPM, DigitalFieldSecurity entityPOCO)
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

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultSettings))
            {
					entityPM.DefaultSettings = entityPOCO.DefaultSettings;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ProfileId))
            {
					entityPM.ProfileId = entityPOCO.ProfileId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ParentObjectTableId))
            {
					entityPM.ParentObjectTableId = entityPOCO.ParentObjectTableId;
            }

		}

		public void PMToOldPM(DigitalFieldSecurityPM entityPM, DigitalFieldSecurityPM oldEntityPM)
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
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.DefaultSettings))
            {
                oldEntityPM.DefaultSettings = entityPM.DefaultSettings;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ProfileId))
            {
                oldEntityPM.ProfileId = entityPM.ProfileId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ParentObjectTableId))
            {
                oldEntityPM.ParentObjectTableId = entityPM.ParentObjectTableId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DigitalFieldSecurityPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.DefaultSettings)) //T4 find type == nText 
            {
                entityPM.DefaultSettings = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.DefaultSettings));
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
	 