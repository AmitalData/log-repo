
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
   
   public partial class TeamMemberBusinessRoleDataMapping: IMapping<TeamMemberBusinessRolePM, TeamMemberBusinessRole>,IMappingEncodeBase64NVARCHARFields<TeamMemberBusinessRolePM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TeamMemberId, 
	         AddedByUserId, 
	         AddDate, 
	         BusinessRoleId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         TeamMemberId, 
	         AddedByUserId, 
	         AddDate, 
	         BusinessRoleId, 
	         RoleName,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(TeamMemberBusinessRolePM entityPM, TeamMemberBusinessRole entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamMemberId))
            {
				entityPOCO.TeamMemberId = entityPM.TeamMemberId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
				entityPOCO.AddedByUserId = entityPM.AddedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
				entityPOCO.AddDate = entityPM.AddDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessRoleId))
            {
				entityPOCO.BusinessRoleId = entityPM.BusinessRoleId;
			}
			}

		public void POCOToPM(TeamMemberBusinessRolePM entityPM, TeamMemberBusinessRole entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TeamMemberId))
            {
					entityPM.TeamMemberId = entityPOCO.TeamMemberId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedByUserId))
            {
					entityPM.AddedByUserId = entityPOCO.AddedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddDate))
            {
					entityPM.AddDate = entityPOCO.AddDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.BusinessRoleId))
            {
					entityPM.BusinessRoleId = entityPOCO.BusinessRoleId;
            }

		}

		public void PMToOldPM(TeamMemberBusinessRolePM entityPM, TeamMemberBusinessRolePM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamMemberId))
            {
                oldEntityPM.TeamMemberId = entityPM.TeamMemberId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
                oldEntityPM.AddedByUserId = entityPM.AddedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
                oldEntityPM.AddDate = entityPM.AddDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.BusinessRoleId))
            {
                oldEntityPM.BusinessRoleId = entityPM.BusinessRoleId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(TeamMemberBusinessRolePM entityPM)
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
	 