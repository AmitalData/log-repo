
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.Repsitories;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class TeamMemberBusinessRoleDataMapping: IMapping<TeamMemberBusinessRolePM, TeamMemberBusinessRole>
   {

        public void CustomPMToPOCO(TeamMemberBusinessRolePM entityPM, TeamMemberBusinessRole entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TeamMemberId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.AddedByUserId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.BusinessRoleId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.TeamMemberId = entityPM.TeamMemberId;
            entityPOCO.AddedByUserId = entityPM.AddedByUserId;
            entityPOCO.BusinessRoleId = entityPM.BusinessRoleId;
        }

        public void CustomPOCOToPM(TeamMemberBusinessRolePM entityPM, TeamMemberBusinessRole entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.RoleName);
            if (!string.IsNullOrEmpty(entityPOCO.BusinessRoleId))
            {
                BusinessRoleRepository repository = new BusinessRoleRepository(entityPOCO.Tenant);
                BusinessRole role = repository.GetSingle(entityPOCO.BusinessRoleId, entityPOCO.Tenant);
                if (role != null)
                {
                    entityPM.RoleName = role.Name;
                }
            }
        }
   }

}
   