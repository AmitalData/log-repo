
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{
   
   public partial class UserPinnedDashboardDataMapping: IMapping<UserPinnedDashboardPM, UserPinnedDashboard>,IMappingEncodeBase64NVARCHARFields<UserPinnedDashboardPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UserId, 
	         Dashboards,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         UserId, 
	         Dashboards,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(UserPinnedDashboardPM entityPM, UserPinnedDashboard entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
				entityPOCO.UserId = entityPM.UserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Dashboards))
            {
				entityPOCO.Dashboards = entityPM.Dashboards;
			}
			}

		public void POCOToPM(UserPinnedDashboardPM entityPM, UserPinnedDashboard entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.UserId))
            {
					entityPM.UserId = entityPOCO.UserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Dashboards))
            {
					entityPM.Dashboards = entityPOCO.Dashboards;
            }

		}

		public void PMToOldPM(UserPinnedDashboardPM entityPM, UserPinnedDashboardPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.UserId))
            {
                oldEntityPM.UserId = entityPM.UserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Dashboards))
            {
                oldEntityPM.Dashboards = entityPM.Dashboards;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(UserPinnedDashboardPM entityPM)
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
	 