
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
   
   public partial class LBPTeamMemberDataMapping: IMapping<LBPTeamMemberPM, LBPTeamMember>,IMappingEncodeBase64NVARCHARFields<LBPTeamMemberPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         MemberUserId, 
	         TeamId, 
	         AddDate, 
	         AddedByUserId, 
	         MemberTeamId,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Id, 
	         Tenant, 
	         MemberUserId, 
	         TeamId, 
	         AddDate, 
	         AddedByUserId, 
	         MemberTeamId,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(LBPTeamMemberPM entityPM, LBPTeamMember entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MemberUserId))
            {
				entityPOCO.MemberUserId = entityPM.MemberUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamId))
            {
				entityPOCO.TeamId = entityPM.TeamId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
				entityPOCO.AddDate = entityPM.AddDate;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
				entityPOCO.AddedByUserId = entityPM.AddedByUserId;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MemberTeamId))
            {
				entityPOCO.MemberTeamId = entityPM.MemberTeamId;
			}
			}

		public void POCOToPM(LBPTeamMemberPM entityPM, LBPTeamMember entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Id))
            {
					entityPM.Id = entityPOCO.Id;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MemberUserId))
            {
					entityPM.MemberUserId = entityPOCO.MemberUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.TeamId))
            {
					entityPM.TeamId = entityPOCO.TeamId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddDate))
            {
					entityPM.AddDate = entityPOCO.AddDate;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.AddedByUserId))
            {
					entityPM.AddedByUserId = entityPOCO.AddedByUserId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.MemberTeamId))
            {
					entityPM.MemberTeamId = entityPOCO.MemberTeamId;
            }

		}

		public void PMToOldPM(LBPTeamMemberPM entityPM, LBPTeamMemberPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MemberUserId))
            {
                oldEntityPM.MemberUserId = entityPM.MemberUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.TeamId))
            {
                oldEntityPM.TeamId = entityPM.TeamId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddDate))
            {
                oldEntityPM.AddDate = entityPM.AddDate;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.AddedByUserId))
            {
                oldEntityPM.AddedByUserId = entityPM.AddedByUserId;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.MemberTeamId))
            {
                oldEntityPM.MemberTeamId = entityPM.MemberTeamId;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(LBPTeamMemberPM entityPM)
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
	 