
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
   
   public partial class AvailableStatusFieldDataMapping: IMapping<AvailableStatusFieldPM, AvailableStatusField>,IMappingEncodeBase64NVARCHARFields<AvailableStatusFieldPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         FieldCode, 
	         IsAvailable,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         FieldCode, 
	         IsAvailable,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(AvailableStatusFieldPM entityPM, AvailableStatusField entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAvailable))
            {
				entityPOCO.IsAvailable = entityPM.IsAvailable;
			}
			}

		public void POCOToPM(AvailableStatusFieldPM entityPM, AvailableStatusField entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldCode))
            {
					entityPM.FieldCode = entityPOCO.FieldCode;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsAvailable))
            {
					entityPM.IsAvailable = entityPOCO.IsAvailable;
            }

		}

		public void PMToOldPM(AvailableStatusFieldPM entityPM, AvailableStatusFieldPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsAvailable))
            {
                oldEntityPM.IsAvailable = entityPM.IsAvailable;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(AvailableStatusFieldPM entityPM)
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
	 