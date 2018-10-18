
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class EmployeeGroupLineDataMapping: IMapping<EmployeeGroupLinePM, EmployeeGroupLine>,IMappingEncodeBase64NVARCHARFields<EmployeeGroupLinePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         EmployeeGroupId, 
	         UserId, 
	         IsDefaultOwner,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         EmployeeGroupId, 
	         UserId, 
	         IsDefaultOwner,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(EmployeeGroupLinePM entityPM, EmployeeGroupLine entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
				entityPOCO.EmployeeGroupId = entityPM.EmployeeGroupId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
				entityPOCO.UserId = entityPM.UserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDefaultOwner))
            {
				entityPOCO.IsDefaultOwner = entityPM.IsDefaultOwner;
			}
			}

		public void POCOToPM(EmployeeGroupLinePM entityPM, EmployeeGroupLine entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.EmployeeGroupId))
            {
					entityPM.EmployeeGroupId = entityPOCO.EmployeeGroupId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.IsDefaultOwner))
            {
					entityPM.IsDefaultOwner = entityPOCO.IsDefaultOwner;
            }

		}

		public void PMToOldPM(EmployeeGroupLinePM entityPM, EmployeeGroupLinePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.EmployeeGroupId))
            {
                oldEntityPM.EmployeeGroupId = entityPM.EmployeeGroupId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
                oldEntityPM.UserId = entityPM.UserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.IsDefaultOwner))
            {
                oldEntityPM.IsDefaultOwner = entityPM.IsDefaultOwner;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(EmployeeGroupLinePM entityPM)
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
	 