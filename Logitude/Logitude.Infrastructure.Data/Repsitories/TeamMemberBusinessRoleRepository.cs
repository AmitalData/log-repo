 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class TeamMemberBusinessRoleRepository:IRepository<TeamMemberBusinessRole>
   {
		public List<TeamMemberBusinessRole> GetMulti(EntityKeyFields entityKeys)
        {
            LBPTeamMemberKeys myEntityKeys = entityKeys as LBPTeamMemberKeys;
            return (from a in context.TeamMemberBusinessRoles where a.TeamMemberId == myEntityKeys.Id select a).ToList();
        }

        public IQueryable<TeamMemberBusinessRole> GetAllByBusinessRoleId(string businessRoleId, int tenant)
        {
            return from a in context.TeamMemberBusinessRoles.Include("LBPTeamMember")
                   where a.Tenant == tenant && a.BusinessRoleId == businessRoleId
                   select a;
        }
   }
}
   