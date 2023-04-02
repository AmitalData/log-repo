
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
   
   public partial class DefaultValueDataMapping: IMapping<DefaultValuePM, DefaultValue>,IMappingEncodeBase64NVARCHARFields<DefaultValuePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DefaultTypeId, 
	         Distr, 
	         BranchId, 
	         CardId, 
	         ShortValue, 
	         Value,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         DefaultTypeId, 
	         Distr, 
	         BranchId, 
	         CardId, 
	         ShortValue, 
	         Value,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DefaultValuePM entityPM, DefaultValue entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShortValue))
            {
				entityPOCO.ShortValue = entityPM.ShortValue;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Value))
            {
				entityPOCO.Value = entityPM.Value;
			}
			}

		public void POCOToPM(DefaultValuePM entityPM, DefaultValue entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DefaultTypeId))
            {
					entityPM.DefaultTypeId = entityPOCO.DefaultTypeId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Distr))
            {
					entityPM.Distr = entityPOCO.Distr;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BranchId))
            {
					entityPM.BranchId = entityPOCO.BranchId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.CardId))
            {
					entityPM.CardId = entityPOCO.CardId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.ShortValue))
            {
					entityPM.ShortValue = entityPOCO.ShortValue;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Value))
            {
					entityPM.Value = entityPOCO.Value;
            }

		}

		public void PMToOldPM(DefaultValuePM entityPM, DefaultValuePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.ShortValue))
            {
                oldEntityPM.ShortValue = entityPM.ShortValue;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Value))
            {
                oldEntityPM.Value = entityPM.Value;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DefaultValuePM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.ShortValue)) //T4 find type == nText 
            {
                entityPM.ShortValue = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.ShortValue));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.Value)) //T4 find type == nText 
            {
                entityPM.Value = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.Value));
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
	 